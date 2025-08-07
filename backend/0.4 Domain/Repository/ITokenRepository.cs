using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface ITokenRepository
    {
        Task<bool> AddAsync(NotificationToken token);
        Task<bool> UpdateAsync(NotificationToken token);
        Task<bool> DeleteExpiredTokensAsync(TimeSpan expirationTime);
        IQueryable<NotificationToken> GetAsQueryable();
    }
}