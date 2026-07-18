using ClinicProjectApplication.Auth.Commands.PurgeTokens;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace ClinicProjectInfrastructure.Services
{
    //public class TokenCleanupService(IServiceScopeFactory factory, ILogger<TokenCleanupService> log)
    //: BackgroundService
    //{
    //    protected override async Task ExecuteAsync(CancellationToken ct)
    //    {
    //        while (!ct.IsCancellationRequested)
    //        {
    //            await Task.Delay(TimeSpan.FromHours(24), ct);
    //            try
    //            {
    //                using var scope = factory.CreateScope();
    //                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    //                await mediator.Send(new PurgeExpiredTokensCommand(), ct);
    //                log.LogInformation("Purged expired refresh tokens at {Time}", DateTime.UtcNow);
    //            }
    //            catch (Exception ex) when (ex is not OperationCanceledException)
    //            {
    //                log.LogError(ex, "Token cleanup failed");
    //            }
    //        }
    //    }
    //}
}
