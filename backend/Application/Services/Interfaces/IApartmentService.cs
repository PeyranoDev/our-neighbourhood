using Application.Schemas.Requests;
using Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface IApartmentService
    {
        Task<Apartment> CreateApartmentAsync(ApartmentForCreateDTO apartment);
        Task<List<Apartment>> GetAllApartmentsAsync();
        Task<Apartment?> GetApartmentByIdAsync(int id);
        Task<Apartment?> UpdateApartmentAsync(Apartment apartment);
        Task DeleteApartmentAsync(int id);
        Task<Apartment> GetApartmentsByUserIdAsync(int userId);
    }
}