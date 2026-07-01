
using ClinicProjectApi.Dtos;
using ClinicProjectApplication.Auth.Commands.DeleteUser;
using ClinicProjectApplication.Auth.Commands.ForegotPassword;
using ClinicProjectApplication.Auth.Commands.LoginUser;
using ClinicProjectApplication.Auth.Commands.Mfa;
using ClinicProjectApplication.Auth.Commands.Mfa.Commands;
using ClinicProjectApplication.Auth.Commands.Mfa.Queries;
using ClinicProjectApplication.Auth.Commands.RefreshTokens;
using ClinicProjectApplication.Auth.Commands.RegisterUser;
using ClinicProjectApplication.Auth.Commands.RevokeTokens;
using ClinicProjectApplication.Auth.ResetPassword;
using ClinicProjectApplication.Commands.RevokeTokens;
using ClinicProjectApplication.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace DefaultAuthenticationApi.Controllers
{
    
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IMediator mediator, IHttpContextAccessor httpContext) : ControllerBase
    {
        private string? IpAddress =>
            Request.Headers.TryGetValue("X-Forwarded-For", out var fwd)
                ? fwd.FirstOrDefault()
                : httpContext.HttpContext?.Connection.RemoteIpAddress?.ToString();
        //private string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
        private string? CurrentUserId => User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        // ── POST api/auth/register ────────────────────────────────────────────────

        [HttpPost("register")]
        [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Register(
            [FromBody] Dtos.RegisterRequest req, CancellationToken ct)
        {
            var result = await mediator.Send(
                new RegisterUserCommand(
                 
                    req.Email, req.Password,
                    IpAddress), ct);

         //   SetRefreshCookie(result.newRefresh, result.Expires);
            return CreatedAtAction(nameof(Register),"Please check your email for confirmation");
        }

        // ── POST api/auth/login ───────────────────────────────────────────────────
        //   [RateLimiter("auth")]
        [HttpPost("login")]
        [ProducesResponseType(typeof(AccessTokenResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MfaRequiredResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
      [FromBody] LoginRequest req, CancellationToken ct)
        {
            var result = await mediator.Send(
                new LoginUserCommand(req.Email, req.Password, IpAddress), ct);

            if (!result.IsSuccess)
                return Unauthorized(new { message = result.ErrorMessage });

            if (result.Data.MfaRequired)
                return Ok(new MfaRequiredResponseDto(result.Data.MfaToken!));

            var tokens = result.Data.Tokens!;
            SetRefreshCookie(tokens.RefreshToken, tokens.RefreshTokenExpires);
            return Ok(new AccessTokenResponseDto(tokens.AccessToken));
        }
        [HttpPost("login/mfa")]
        [ProducesResponseType(typeof(AccessTokenResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> VerifyMfa(
      [FromBody] VerifyMfaRequest req, CancellationToken ct)
        {
            var result = await mediator.Send(
                new VerifyMfaCommand(req.MfaToken, req.Code, IpAddress), ct);

            if (!result.IsSuccess)
                return Unauthorized(new { message = result.ErrorMessage });

            var tokens = result.Data!.Tokens!;
            SetRefreshCookie(tokens.RefreshToken, tokens.RefreshTokenExpires);
            return Ok(new AccessTokenResponseDto(tokens.AccessToken));
        }

        [HttpPost("login/mfa/recovery")]
        [ProducesResponseType(typeof(AccessTokenResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RedeemRecoveryCode(
            [FromBody] RedeemRecoveryCodeRequest req, CancellationToken ct)
        {
            var result = await mediator.Send(
                new RedeemRecoveryCodeCommand(req.MfaToken, req.RecoveryCode, IpAddress), ct);

            if (!result.IsSuccess)
                return Unauthorized(new { message = result.ErrorMessage });

            var tokens = result.Data!.Tokens!;
            SetRefreshCookie(tokens.RefreshToken, tokens.RefreshTokenExpires);
            return Ok(new AccessTokenResponseDto(tokens.AccessToken));
        }
        // ── POST api/auth/refresh ─────────────────────────────────────────────────

        //[HttpPost("refresh")]
        //[ProducesResponseType(typeof(AccessTokenResponseDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //public async Task<IActionResult> Refresh(CancellationToken ct)
        //{
        //    var refreshToken = Request.Cookies["refresh_token"];
        //    if (string.IsNullOrEmpty(refreshToken))
        //        return Unauthorized(new { message = "Refresh token cookie is missing." });

        //    var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", string.Empty);
        //    if (string.IsNullOrEmpty(accessToken))
        //        return Unauthorized(new { message = "Access token is missing." });

        //    var result = await mediator.Send(new RefreshTokenCommand(accessToken, refreshToken, IpAddress), ct);

        //    SetRefreshCookie(result.RefreshToken, result.RefreshTokenExpires);
        //    return Ok(new AccessTokenResponseDto(result.AccessToken));
        //}
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AccessTokenResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh(CancellationToken ct)
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(new { message = "Refresh token cookie is missing." });

            var result = await mediator.Send(new RefreshTokenCommand(refreshToken, IpAddress), ct);

            SetRefreshCookie(result.RefreshToken, result.RefreshTokenExpires);
            return Ok(new AccessTokenResponseDto(result.AccessToken));
        }

        // ── POST api/auth/revoke ──────────────────────────────────────────────────

        [Authorize]
        [HttpPost("revoke")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Revoke(CancellationToken ct)
        {
            var refreshToken = Request.Cookies["refresh_token"]
                ?? throw new UnauthorizedException("Refresh token cookie is missing.");

            await mediator.Send(new RevokeTokenCommand(refreshToken, IpAddress), ct);

            ClearRefreshCookie();
            return NoContent();
        }

        // ── POST api/auth/revoke-all ──────────────────────────────────────────────

        [Authorize]
        [HttpPost("revoke-all")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RevokeAll(CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedException("Invalid token claims.");

            await mediator.Send(new RevokeAllTokensCommand(userId, IpAddress), ct);

            ClearRefreshCookie();
            return NoContent();
        }

        // ── POST api/auth/logout ──────────────────────────────────────────────────

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Logout()
        {
            ClearRefreshCookie();
            return NoContent();
        }
        [Authorize]
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task< IActionResult> Delete(string email)
        {
           var result=  await mediator.Send(new DeleteUserCommand(email));
         return  result.IsSuccess ?  Ok(result) : BadRequest(result.ErrorMessage);
           
        }

        [AllowAnonymous]
        [HttpGet("confirm-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmail([FromQuery]string email,string token)
        {
            var result = await mediator.Send(new ConfirmEmailCommand(email,token));
            return result.IsSuccess ? Ok(result) : BadRequest(result.ErrorMessage);

        }

        [AllowAnonymous]
        [HttpPost("foregot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForegotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await mediator.Send(new ForgotPasswordCommand(request.Email));
            return result.IsSuccess ? Ok(result) : BadRequest(result.ErrorMessage);

        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await mediator.Send(new ResetPasswordCommand(request.Email,request.Token,request.NewPassword));
            return result.IsSuccess ? Ok(result) : BadRequest(result.ErrorMessage);

        }
        [Authorize]
        [HttpGet("mfa/setup")]
        public async Task<IActionResult> GetMfaSetup(CancellationToken ct)
        {
            var result = await mediator.Send(new GenerateMfaSetupCommand(CurrentUserId), ct);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }
        [Authorize]
        [HttpPost("mfa/enable")]
        public async Task<IActionResult> EnableMfa([FromBody] EnableMfaRequest req, CancellationToken ct)
        {
            var result = await mediator.Send(new EnableMfaCommand(CurrentUserId, req.Code), ct);
            return result.IsSuccess
                ? Ok(new { recoveryCodes = result.Data })
                : BadRequest(result.ErrorMessage);
        }

        [Authorize]
        [HttpPost("mfa/disable")]
        public async Task<IActionResult> DisableMfa([FromBody] DisableMfaRequest req, CancellationToken ct)
        {
            var result = await mediator.Send(new DisableMfaCommand(CurrentUserId, req.Password), ct);
            return result.IsSuccess ? Ok() : BadRequest(result.ErrorMessage);
        }

        [Authorize]
        [HttpGet("mfa/status")]
        public async Task<IActionResult> GetMfaStatus(CancellationToken ct)
        {
            var result = await mediator.Send(new GetMfaStatusQuery(CurrentUserId), ct);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }
        // ── Private helpers ───────────────────────────────────────────────────────

        private void SetRefreshCookie(string token, DateTime expires) =>
            Response.Cookies.Append("refresh_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = expires,
                Path = "/api/auth",   // scoped — not sent on every request
            });

        private void ClearRefreshCookie() =>
            Response.Cookies.Delete("refresh_token", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/api/auth",   // must match SetRefreshCookie path to actually delete
            });

        public record ResetPasswordRequest(string Email, string Token, string NewPassword);
         public record ForgotPasswordRequest(string Email);
        //public record MfaRequiredResponseDto(string MfaToken);
        //public record MfaRequiredResponseDto(bool MfaRequired, string MfaToken)
        //{
        //    public MfaRequiredResponseDto(string mfaToken) : this(true, mfaToken) { }
        //}
        public record VerifyMfaRequest(string MfaToken, string Code);
        public record RedeemRecoveryCodeRequest(string MfaToken, string RecoveryCode);
        public record MfaRequiredResponseDto(bool MfaRequired, string MfaToken)
        {
            public MfaRequiredResponseDto(string mfaToken) : this(true, mfaToken) { }
        }
        public record EnableMfaRequest(string Code);
        public record DisableMfaRequest(string Password);
    }
}
