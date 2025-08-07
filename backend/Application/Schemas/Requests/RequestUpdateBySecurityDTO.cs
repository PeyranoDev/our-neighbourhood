using Domain.Common.Enum;

namespace Application.Schemas.Requests
{
    public class RequestUpdateBySecurityDTO
    {
        public required int VehicleId { get; set; }
        public required VehicleRequestStatusEnum VehicleRequestNewStatus { get; set; }
        public required int SecurityId { get; set; }
    }
}
