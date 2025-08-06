using Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(int id);
        Task<Role?> GetRoleByType(string type);
        Task<bool> RoleExistsAsync(int roleId);
    }
}