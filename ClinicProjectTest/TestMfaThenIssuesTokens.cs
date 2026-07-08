using ClinicProjectApplication.Auth.Commands.LoginUser;
using ClinicProjectApplication.Auth.Commands.Mfa;
using ClinicProjectApplication.Auth.Commands.Mfa.Commands;
using MediatR;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectTest
{
    public class TestMfaThenIssuesTokens
    {
        private readonly IMediator _mediator;
        public TestMfaThenIssuesTokens(IMediator mediator)
        {
            _mediator = mediator;
        }
        //[Fact]
        //public async Task Login_WithMfaEnabled_RequiresMfaThenIssuesTokens()
        //{
        //    // arrange: create user, enable MFA, get the SharedKey via your setup endpoint/handler
        //    var setupResult = await _mediator.Send(new GenerateMfaSetupCommand("testUserId"));
        //    var key = Base32Encoding.ToBytes(setupResult.Data!.SharedKey);
        //    var totp = new Totp(key);
        //    var code = totp.ComputeTotp();

        //    await _mediator.Send(new EnableMfaCommand("testUserId", code));

        //    // act: login should now require MFA
        //    var loginResult = await _mediator.Send(new LoginUserCommand("testEmail", "fathi111", "127.0.0.1"));
        //    Assert.True(loginResult.Data!.MfaRequired);

        //    // act: verify with a fresh code
        //    var mfaCode = totp.ComputeTotp();
        //    var verifyResult = await _mediator.Send(
        //        new VerifyMfaCommand(loginResult.Data.MfaToken!, mfaCode, "127.0.0.1"));

        //    // assert
        //    Assert.True(verifyResult.IsSuccess);
        //    Assert.NotNull(verifyResult.Data!.Tokens);
        //}
    }
}
