using Application.Schemas.Responses;
using Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface IInvitationService
    {
        Task<Invitation?> GetInvitationAsync(string token);
        Task<string> CreateInvitationAsync(CreateInvitationDto dto, int userId);
        Task<bool> IsTokenValidAsync(string token);
        Task<int> UpdateInvitationAsync(Invitation invitation);
    }
}