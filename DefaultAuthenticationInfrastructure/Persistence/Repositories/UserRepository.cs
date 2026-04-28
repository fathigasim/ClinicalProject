using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ClinicProjectInfrastructure.Persistence.Repositories
{
    // Infrastructure/Persistence/Repositories/UserRepository.cs
    public class UserRepository(AppDbContext db) : IUserRepository
    {
        public async Task<ApplicationUser?> GetByIdAsync(string userId, CancellationToken ct)
           =>await db.users.
               Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Id == userId, ct);
        public Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken ct)
            => db.users.
                Include(u => u.RefreshTokens)
                 .FirstOrDefaultAsync(u => u.Email == email, ct);

        public Task<ApplicationUser?> GetByRefreshTokenAsync(string token, CancellationToken ct)
            => db.users
               .Include(u => u.RefreshTokens)
                 .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token), ct);

        public async Task UpdateAsync(ApplicationUser user, CancellationToken ct)
        {
            db.Users.Update(user);
            await db.SaveChangesAsync(ct);
        }

        public async Task PurgeExpiredTokensAsync(CancellationToken ct)
        {
            var cutoff = DateTime.UtcNow;
            // Bulk delete — no entity load needed
            await db.Set<RefreshToken>()
                    .Where(t => t.Revoked != null || t.Expires <= cutoff)
                    .ExecuteDeleteAsync(ct);
        }
    }
}

