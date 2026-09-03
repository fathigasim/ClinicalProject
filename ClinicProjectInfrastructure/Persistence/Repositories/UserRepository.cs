using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectInfrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserRepository(AppDbContext db,UserManager<ApplicationUser> userManager) : IUserRepository
{
    public async Task<ApplicationUser?> GetByIdAsync(string userId, CancellationToken ct)
        => await db.users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == userId, ct);

    public Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken ct)
        => db.users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<ApplicationUser?> GetByRefreshTokenAsync(string token, CancellationToken ct)
        => db.users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token), ct);

    public async Task SaveAsync(CancellationToken ct)
        => await db.SaveChangesAsync(ct);

    //public async Task PurgeExpiredTokensAsync(CancellationToken ct)
    //    => await db.Set<RefreshToken>()
    //        .Where(t => t.Revoked != null || t.Expires <= DateTime.UtcNow)
    //        .ExecuteDeleteAsync(ct);
    public async Task PurgeExpiredTokensAsync(CancellationToken ct)
    {
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM RefreshToken WHERE Revoked IS NOT NULL OR Expires <= {DateTime.UtcNow}",
            ct);
    }

    public Task UpdateAsync(ApplicationUser user, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<ApplicationUser?> ForgotPasswordAsync(string email, CancellationToken ct)
      => db.users
          .Include(u => u.RefreshTokens)
          .FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<IdentityResult> RedeemTwoFactorRecoveryCodeAsync(ApplicationUser user, string code)
    {
        return userManager.RedeemTwoFactorRecoveryCodeAsync(user, code);
    }

}