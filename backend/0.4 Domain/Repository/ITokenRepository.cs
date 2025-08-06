﻿using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface ITokenRepository
    {
        Task<bool> AddAsync(NotificationToken token);
        Task<bool> UpdateAsync(NotificationToken token);
        Task<bool> UpdateLastUsedAsync(int tokenId);
        Task<bool> DeleteExpiredTokensAsync(TimeSpan expirationTime);
        Task<NotificationToken?> GetByTokenAsync(string token);
        Task<NotificationToken?> GetLatestByUserIdAsync(int userId);
    }
}