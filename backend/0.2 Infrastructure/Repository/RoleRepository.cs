using Domain.Entities;
using Domain.Repository;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AqualinaAPIContext _context;

        public RoleRepository(AqualinaAPIContext context)
        {
            _context = context;
        }

        public IQueryable<Role> GetAsQueryable()
        {
            return _context.Roles.AsNoTracking();
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task CreateAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Role role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }
}
