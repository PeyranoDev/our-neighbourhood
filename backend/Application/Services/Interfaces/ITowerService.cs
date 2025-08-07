using Application.Schemas.Requests;
using Application.Schemas.Responses;

namespace Application.Services.Interfaces
{
    public interface ITowerService
    {
        Task<TowerForUserResponseDTO> CreateTowerAsync(TowerForCreateDTO towerDto);
        Task DeleteTowerAsync(int id);
        Task<PagedResponse<TowerForUserResponseDTO>> GetPublicTowerListAsync(TowerFilterParams filterParams);
        Task<TowerForUserResponseDTO> GetTowerByIdAsync(int id);
        Task UpdateTowerAsync(int id, TowerForUpdateDTO towerDto);
    }
}