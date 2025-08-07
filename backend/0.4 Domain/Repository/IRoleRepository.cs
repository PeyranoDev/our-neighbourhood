using Domain.Entities;

namespace Domain.Repository
{
    public interface IRoleRepository
    {
        IQueryable<Role> GetAsQueryable();
        Task<Role?> GetByIdAsync(int id);
        Task CreateAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(Role role);
    }
}