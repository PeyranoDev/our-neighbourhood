using Application.Schemas.Requests;

namespace Application.Services.Interfaces
{
    public interface IVehicleRequestService
    {
        Task<bool> CreateRequestAsync(int vehicleId, int userId);
        Task<bool> SecurityUpdateRequestAsync(RequestUpdateBySecurityDTO dto);
    }
}