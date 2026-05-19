
using ClinicProjectApi.Dtos;
using ClinicProjectApplication.Auth.Commands.DeleteUser;
using ClinicProjectApplication.Auth.Commands.LoginUser;
using ClinicProjectApplication.Auth.Commands.RefreshTokens;
using ClinicProjectApplication.Auth.Commands.RegisterUser;
using ClinicProjectApplication.Auth.Commands.RevokeTokens;
using ClinicProjectApplication.Commands.RevokeTokens;
using ClinicProjectApplication.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest req, CancellationToken ct)
        {
            var result = await mediator.Send(
                new LoginUserCommand(req.Email, req.Password, IpAddress), ct);

            SetRefreshCookie(result.newRefresh, result.Expires);
            return Ok(new AccessTokenResponseDto(result.accessToken));
        }

        // ── POST api/auth/refresh ─────────────────────────────────────────────────

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh(CancellationToken ct)
        {
            var refreshToken = Request.Cookies["refresh_token"]
                ?? throw new UnauthorizedException("Refresh token cookie is missing.");

            var accessToken = Request.Headers.Authorization
                .FirstOrDefault()?.Replace("Bearer ", string.Empty)
                ?? throw new UnauthorizedException("Access token is missing.");

            var result = await mediator.Send(
                new RefreshTokenCommand(accessToken, refreshToken, IpAddress), ct);

            SetRefreshCookie(result.newRefresh, result.Expires);
            return Ok(new AccessTokenResponseDto(result.accessToken));
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
    }
}
