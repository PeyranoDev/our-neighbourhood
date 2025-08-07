using Application.Helpers;
using Application.Schemas.Requests;
using Application.Schemas.Responses;
using Application.Services.Interfaces;
using Application.Specifications;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        public Task<IList<Vehicle>> GetVehiclesPerUserIdAsync(int userId)
            => _vehicleRepository.GetVehiclesPerUserIdAsync(userId);

        public async Task<PagedResponse<VehicleForResponseDTO>> GetVehiclesPagedAsync(
            VehicleFilterParams filters,
            PaginationParams pagination)
        {
            // 1) Construyo la spec
            var spec = new VehicleByFilterSpecification(filters, pagination);

            // 2) Listado y total
            var vehicles = await _vehicleRepository.ListAsync(spec);
            var total = await _vehicleRepository.CountAsync(spec);

            // 3) Mapeo y retorno
            var dtos = _mapper.Map<List<VehicleForResponseDTO>>(vehicles);
            return new PagedResponse<VehicleForResponseDTO>(
                dtos,
                total,
                pagination.PageNumber,
                pagination.PageSize
            );
        }

        public async Task<bool> DeleteVehicleAsync(int vehicleId)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null) return false;

            vehicle.IsActive = false;
            await _vehicleRepository.UpdateAsync(vehicle);
            return true;
        }

        public Task<bool> HasActiveRequestAsync(int vehicleId)
            => _vehicleRepository.HasActiveRequestAsync(vehicleId);

        public Task<Request?> GetLastActiveRequestAsync(int vehicleId)
            => _vehicleRepository.GetLastActiveRequestAsync(vehicleId);

        public Task<IList<Vehicle>> GetVehiclesPerUserIdWithLastRequestAsync(int userId)
            => _vehicleRepository.GetVehiclesPerUserIdWithLastRequestAsync(userId);

        public Task<IList<Vehicle>> GetVehiclesWithActiveRequestsAsync()
            => _vehicleRepository.GetVehiclesWithActiveRequestsAsync();

        public Task<Vehicle?> GetVehicleByIdAsync(int vehicleId)
            => _vehicleRepository.GetByIdAsync(vehicleId);
    }
}