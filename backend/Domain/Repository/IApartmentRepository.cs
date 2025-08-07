using Domain.Entities;

namespace Domain.Repository
{
    public interface IApartmentRepository
    {
        Task<Apartment> CreateAsync(Apartment apartment);
        Task<List<Apartment>> GetApartmentsAsync();
        Task<Apartment?> GetByIdAsync(int id);
        Task<bool> IdentifierExistsAsync(string identifier);
        Task<Apartment?> UpdateAsync(Apartment apartment);
        Task<bool> DeleteAsync(int id);
        Task DetachUsersFromApartmentAsync(int apartmentId);
    }
}