using Domain.Entities;
using Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repository
{
    /// <summary>
    /// Repositorio específico de Vehicle: hereda el genérico con Specification Pattern
    /// y añade métodos propios.
    /// </summary>
    public interface IVehicleRepository : IBaseRepository<Vehicle>
    {
        Task<IList<Vehicle>> GetVehiclesPerUserIdAsync(int userId);
        Task<bool> HasActiveRequestAsync(int vehicleId);
        Task<Request?> GetLastActiveRequestAsync(int vehicleId);
        Task<IList<Vehicle>> GetVehiclesPerUserIdWithLastRequestAsync(int userId);
        Task<IList<Vehicle>> GetVehiclesWithActiveRequestsAsync();
    }
}