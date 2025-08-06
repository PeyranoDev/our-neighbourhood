﻿using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> UpdateAsync(User user);

        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> CreateAsync(User user);
        Task<int> DeleteAsync(User user);
        IQueryable<User> GetQueryable();
        Task<bool> UsernameExistsAsync(string username);
        Task<IList<User>> GetAllSecurityAsync();
        Task<List<User>> GetAllOnDutySecurityAsync();
        Task<User> GetUserWithNotificationTokenAsync(int userId);
        Task<User?> GetByUsernameWithTowerDataAsync(string username);
    }
}