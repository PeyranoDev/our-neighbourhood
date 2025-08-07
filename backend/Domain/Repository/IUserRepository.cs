using Domain.Common;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repository
{
    /// <summary>
    /// Extiende el repositorio genérico con métodos específicos de User.
    /// </summary>
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByUsernameWithTowerDataAsync(string username);
        Task<User?> GetUserWithNotificationTokenAsync(int userId);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<IList<User>> GetAllSecurityAsync();
        Task<List<User>> GetAllOnDutySecurityAsync();
    }
}