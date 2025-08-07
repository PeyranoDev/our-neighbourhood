using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ApartmentRepository : BaseRepository<Apartment>, IApartmentRepository
    {

        public ApartmentRepository(AqualinaAPIContext _context) : base(_context) { }

        public IQueryable<Apartment> GetAsQueryable()
        {
            return _context.Apartments.AsNoTracking();
        }
        public async Task<Apartment?> GetByIdAsync(int id)
        {
            return await _context.Apartments
                .Include(a => a.Users) 
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Apartment> CreateAsync(Apartment apartment)
        {
            _context.Apartments.Add(apartment);
            await _context.SaveChangesAsync();
            return apartment;
        }
        public async Task<Apartment?> UpdateAsync(Apartment apartment)
        {
            var existingApartment = await _context.Apartments.FindAsync(apartment.Id);
            if (existingApartment == null) return null;

            _context.Entry(existingApartment).CurrentValues.SetValues(apartment);

            await _context.SaveChangesAsync();
            return existingApartment;
        }
        public async Task<bool> IdentifierExistsAsync(string identifier)
        {
            return await _context.Apartments.AnyAsync(a => a.Identifier == identifier);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await DetachUsersFromApartmentAsync(id);

                var apartment = await _context.Apartments.FindAsync(id);
                if (apartment == null) return false;

                _context.Apartments.Remove(apartment);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task DetachUsersFromApartmentAsync(int apartmentId)
        {
            var users = await _context.Users
                .Where(u => u.ApartmentId == apartmentId)
                .ToListAsync();

            foreach (var user in users)
            {
                user.ApartmentId = null;
                user.Apartment = null;
            }

            await _context.SaveChangesAsync();
        }

    }
}
