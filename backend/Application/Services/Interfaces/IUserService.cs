using Application.Schemas.Requests;
using Application.Schemas.Responses;
using Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserForResponse> CreateUserAsync(UserForCreateDTO dto);
        Task<User> CreateUserAsyncWithInvitation(RegisterWithTokenDto dto, int roleId, int? apartmentId);
        void DeleteUserAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<UserForResponse?> GetByIdAsync(int id);
        Task<PagedResponse<UserForResponse>> GetUsersPagedAsync(UserFilterParams filters, PaginationParams pagination);
        void UpdateUserAsync(UserForUpdateDTO dto, int userId);
        Task<bool> UsernameExistsAsync(string username);
        Task<User?> ValidateAsync(CredentialsDTO dto);
    }
}