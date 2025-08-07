using Domain.Entities;
using Application.Services.Interfaces;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<bool> RoleExistsAsync(int roleId)
        {
            return await _roleRepository.GetAsQueryable().AnyAsync(r => r.Id == roleId);
        }

        public async Task<Role?> GetRoleByType(string type)
        {
            return await _roleRepository.GetAsQueryable()
                .FirstOrDefaultAsync(r => r.Type.ToString() == type);
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _roleRepository.GetAsQueryable().ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }
    }
}
