using Domain.Common.Enum;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AqualinaAPIContext context) : base(context) {}

        public async Task<User?> GetByUsernameWithTowerDataAsync(string username)
            => await _dbSet
                .Include(u => u.Role)
                .Include(u => u.Apartment)
                    .ThenInclude(a => a.Tower)
                .Include(u => u.UserTowers)
                    .ThenInclude(ut => ut.Tower)
                .FirstOrDefaultAsync(u => u.Username == username);

        public Task<bool> UsernameExistsAsync(string username)
            => _dbSet.AnyAsync(u => u.Username == username);

        public Task<bool> EmailExistsAsync(string email)
            => _dbSet.AnyAsync(u => u.Email == email);

        public async Task<User?> GetUserWithNotificationTokenAsync(int userId)
            => await _dbSet
                .Include(u => u.NotificationTokens)
                .FirstOrDefaultAsync(u => u.Id == userId);

        public async Task<IList<User>> GetAllSecurityAsync()
            => await _dbSet
                .Include(u => u.Role)
                .Include(u => u.Apartment)
                .Include(u => u.UserTowers)
                .Where(u => u.Role.Type == UserRoleEnum.Security)
                .ToListAsync();

        public async Task<List<User>> GetAllOnDutySecurityAsync()
            => await _dbSet
                .Include(u => u.Role)
                .Include(u => u.UserTowers)
                .Where(u => u.Role.Type == UserRoleEnum.Security && u.IsOnDuty)
                .ToListAsync();
    }
}