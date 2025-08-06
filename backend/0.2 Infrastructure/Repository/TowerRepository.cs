using Application.Schemas.Requests;
using Domain.Common.Models;
using Domain.Entities;
using Domain.Repository;
using Application.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class TowerRepository : ITowerRepository
    {
        private readonly AqualinaAPIContext _context;
        public TowerRepository(AqualinaAPIContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Simplemente devuelve el DbSet como IQueryable.
        /// AsNoTracking es una optimización para consultas de solo lectura.
        /// </summary>
        public IQueryable<Tower> GetAsQueryable()
        {
            return _context.Towers.AsNoTracking();
        }

        // --- Los otros métodos (GetByIdAsync, CreateAsync, etc.) permanecen igual ---

        public async Task<Tower?> GetByIdAsync(int id)
        {
            return await _context.Towers
                .Include(t => t.Apartments)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<PagedResult<Tower>> GetPublicTowerListAsync(TowerFilterParams filterParams)
        {
            var query = _context.Towers.AsNoTracking()
                .ApplyFilters(filterParams)
                .ApplySorting(filterParams.SortBy, filterParams.SortOrder);

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
                .Take(filterParams.PageSize)
                .ToListAsync();

            return new PagedResult<Tower>
            {
                Items = data,
                TotalRecords = totalRecords
            };
        }

        public async Task<Tower?> GetByNameAsync(string name)
        {
            return await _context.Towers
                .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
        }

        public async Task CreateAsync(Tower tower)
        {
            await _context.Towers.AddAsync(tower);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tower tower)
        {
            _context.Towers.Update(tower);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tower tower)
        {
            _context.Towers.Remove(tower);
            await _context.SaveChangesAsync();
        }
    }
}
