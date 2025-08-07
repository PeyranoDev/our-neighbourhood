using Domain.Entities;

namespace Domain.Repository
{
    public interface IVehicleRepository
    {
        Task<bool> AddAsync(Vehicle vehicle);
        IQueryable<Vehicle> GetAsQueryable();
        Task<Vehicle?> GetByIdAsync(int id);
        Task<Request?> GetLastActiveRequestAsync(int vehicleId);
        Task<IList<Vehicle>> GetVehiclesPerUserIdAsync(int userId);
        Task<IList<Vehicle>> GetVehiclesPerUserIdWithLastRequestAsync(int userId);
        Task<IList<Vehicle>> GetVehiclesWithActiveRequestsAsync();
        Task<bool> HasActiveRequestAsync(int vehicleId);
        Task<bool> UpdateAsync(Vehicle vehicle);
    }
}