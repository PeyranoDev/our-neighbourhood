using Application.Schemas.Requests;
using Application.Schemas.Responses;
using Application.Services.Interfaces;
using Application.Specifications;
using AutoMapper;
using Domain.Common.Exceptions;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services.Implementations
{
    public class TowerService : ITowerService
    {
        private readonly ITowerRepository _towerRepo;
        private readonly IMapper _mapper;

        public TowerService(ITowerRepository towerRepo, IMapper mapper)
        {
            _towerRepo = towerRepo;
            _mapper = mapper;
        }

        public async Task<PagedResponse<TowerForUserResponseDTO>> GetPublicTowerListAsync(
            TowerFilterParams filter)
        {
            var spec = new TowerByFilterSpecification(filter);

            var towers = await _towerRepo.ListAsync(spec);
            var total = await _towerRepo.CountAsync(spec);

            var dtos = _mapper.Map<List<TowerForUserResponseDTO>>(towers);
            return new PagedResponse<TowerForUserResponseDTO>(
                dtos,
                total,
                filter.PageNumber,
                filter.PageSize
            );
        }

        public async Task<TowerForUserResponseDTO> GetTowerByIdAsync(int id)
        {
            var tower = await _towerRepo.GetByIdAsync(id)
                        ?? throw new NotFoundException("Tower", id);
            return _mapper.Map<TowerForUserResponseDTO>(tower);
        }

        public async Task<TowerForUserResponseDTO> CreateTowerAsync(TowerForCreateDTO dto)
        {
            if (await _towerRepo.GetByNameAsync(dto.Name) is not null)
                throw new TowerAlreadyExistsException(dto.Name);

            var tower = _mapper.Map<Tower>(dto);
            var created = await _towerRepo.AddAsync(tower);
            return _mapper.Map<TowerForUserResponseDTO>(created);
        }

        public async Task UpdateTowerAsync(int id, TowerForUpdateDTO dto)
        {
            var existing = await _towerRepo.GetByIdAsync(id)
                           ?? throw new NotFoundException("Tower", id);

            if (!string.IsNullOrWhiteSpace(dto.Name) &&
                !dto.Name.Equals(existing.Name, System.StringComparison.OrdinalIgnoreCase) &&
                await _towerRepo.GetByNameAsync(dto.Name) is not null)
            {
                throw new TowerAlreadyExistsException(dto.Name);
            }

            _mapper.Map(dto, existing);
            await _towerRepo.UpdateAsync(existing);
        }

        public async Task DeleteTowerAsync(int id)
        {
            var tower = await _towerRepo.GetByIdAsync(id)
                        ?? throw new NotFoundException("Tower", id);

            if (tower.Apartments.Any())
                throw new TowerInUseException(id);

            await _towerRepo.DeleteAsync(tower);
        }
    }
}