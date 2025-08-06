using Application.Schemas.Requests;

namespace Application.Services.Interfaces
{
    public interface ITokenService
    {
        Task<bool> AddNotificationTokenAsync(NotificationTokenCreateDTO dto, int userId);
    }
}