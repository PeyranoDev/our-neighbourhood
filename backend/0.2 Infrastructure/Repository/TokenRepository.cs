using Domain.Entities;
using Domain.Repository;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AqualinaAPIContext _context;

        public TokenRepository(AqualinaAPIContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(NotificationToken token)
        {
            try
            {
                token.CreatedAt = DateTime.UtcNow;
                token.UpdatedAt = DateTime.UtcNow;

                await _context.NotificationTokens.AddAsync(token);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(NotificationToken token)
        {
            try
            {
                token.UpdatedAt = DateTime.UtcNow;
                _context.NotificationTokens.Update(token);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public IQueryable<NotificationToken> GetAsQueryable()
        {
            return _context.NotificationTokens.AsNoTracking();
        }

        public async Task<bool> DeleteExpiredTokensAsync(TimeSpan expirationTime)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow.Subtract(expirationTime);
                var expiredTokens = await _context.NotificationTokens
                    .Where(t => t.LastSeen < cutoffDate ||
                               (t.LastSeen == null && t.CreatedAt < cutoffDate))
                    .ToListAsync();

                _context.NotificationTokens.RemoveRange(expiredTokens);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}