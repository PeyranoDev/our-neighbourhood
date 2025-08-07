using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Common.Enum;
using Domain.Entities;
using Domain.Repository;
using Infrastructure.Specifications;

namespace Infrastructure.Repository
{
    public class VehicleRepository
        : BaseRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(AqualinaAPIContext context)
            : base(context)
        { }

        public async Task<IList<Vehicle>> GetVehiclesPerUserIdAsync(int userId)
        {
            // Ahora aguardamos la lista y la devolvemos como IList<Vehicle>
            var list = await _dbSet
                .Where(v => v.OwnerId == userId)
                .ToListAsync();

            return list;
        }

        public async Task<bool> HasActiveRequestAsync(int vehicleId)
        {
            var activeStatuses = new[]
            {
                VehicleRequestStatusEnum.Pending,
                VehicleRequestStatusEnum.InPreparation,
                VehicleRequestStatusEnum.AlmostReady,
                VehicleRequestStatusEnum.Ready
            };

            return await _context.Requests
                .AnyAsync(r => r.VehicleId == vehicleId
                            && activeStatuses.Contains(r.Status));
        }

        public async Task<Request?> GetLastActiveRequestAsync(int vehicleId)
        {
            var activeStatuses = new[]
            {
                VehicleRequestStatusEnum.Pending,
                VehicleRequestStatusEnum.InPreparation,
                VehicleRequestStatusEnum.AlmostReady,
                VehicleRequestStatusEnum.Ready
            };

            return await _context.Requests
                .Where(r => r.VehicleId == vehicleId
                         && activeStatuses.Contains(r.Status))
                .OrderByDescending(r => r.RequestedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<Vehicle>> GetVehiclesPerUserIdWithLastRequestAsync(int userId)
        {
            // Igual: await + return
            var list = await _dbSet
                .Where(v => v.OwnerId == userId)
                .Select(v => new Vehicle
                {
                    Id = v.Id,
                    Plate = v.Plate,
                    Model = v.Model,
                    Color = v.Color,
                    IsParked = v.IsParked,
                    IsActive = v.IsActive,
                    OwnerId = v.OwnerId,
                    Requests = v.Requests
                        .OrderByDescending(r => r.RequestedAt)
                        .Take(1)
                        .ToList()
                })
                .ToListAsync();

            return list;
        }

        public async Task<IList<Vehicle>> GetVehiclesWithActiveRequestsAsync()
        {
            var activeStatuses = new[]
            {
                VehicleRequestStatusEnum.Pending,
                VehicleRequestStatusEnum.InPreparation,
                VehicleRequestStatusEnum.AlmostReady,
                VehicleRequestStatusEnum.Ready
            };

            var list = await _dbSet
                .Where(v => v.Requests.Any(r => activeStatuses.Contains(r.Status)))
                .Include(v => v.Requests
                    .Where(r => activeStatuses.Contains(r.Status)))
                .ToListAsync();

            return list;
        }
    }
}