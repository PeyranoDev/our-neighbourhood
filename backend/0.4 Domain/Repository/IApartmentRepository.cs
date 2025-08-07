using Domain.Entities;

namespace Domain.Repository
{
    public interface IApartmentRepository
    {
        Task<Apartment> CreateAsync(Apartment apartment);
        Task<Apartment?> GetByIdAsync(int id);
        IQueryable<Apartment> GetAsQueryable();
        Task<bool> IdentifierExistsAsync(string identifier);
        Task<Apartment?> UpdateAsync(Apartment apartment);
        Task<bool> DeleteAsync(int id);
        Task DetachUsersFromApartmentAsync(int apartmentId);
    }
}