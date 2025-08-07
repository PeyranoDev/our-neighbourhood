using Application.Helpers;
using Application.Schemas.Requests;
using Application.Schemas.Responses;
using Application.Services.Interfaces;
using Application.Specifications;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Common.Exceptions;
using Domain.Entities;
using Domain.Repository;
using System.ComponentModel.DataAnnotations;

namespace Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IHashingService _hashingService;
        private readonly IMapper _mapper;
        private readonly IApartmentRepository _apartmentRepo;
        private readonly IRoleRepository _roleRepository;

        public UserService(
            IUserRepository userRepo,
            IHashingService hashingService,
            IMapper mapper,
            IApartmentRepository apartmentRepo = null,
            IRoleRepository roleRepository = null)
        {
            _userRepo = userRepo;
            _hashingService = hashingService;
            _mapper = mapper;
            _apartmentRepo = apartmentRepo;
            _roleRepository = roleRepository;
        }

        public async Task<User?> ValidateAsync(CredentialsDTO dto)
        {

            var user = await _userRepo.GetByUsernameWithTowerDataAsync(dto.Username);

            if (user == null)
                return null;

            bool ok = _hashingService.VerifyPassword(dto.Password, user.PasswordHash);
            return ok ? user : null;
        }

        public async void UpdateUserAsync(UserForUpdateDTO dto, int userId)
        {
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(dto, new ValidationContext(dto), validationResults, true))
            {
                throw new ValidationException(validationResults.First().ErrorMessage);
            }

            var existingUser = await _userRepo.GetByIdAsync(userId);
            if (existingUser == null)
                throw new UserNotFoundException(userId);

            _mapper.Map(dto, existingUser);

            if (dto.Apartment_Id.HasValue && _apartmentRepo != null)
            {
                existingUser.Apartment = await _apartmentRepo.GetByIdAsync(dto.Apartment_Id.Value);
            }

            await _userRepo.UpdateAsync(existingUser);
        }

        public async Task<UserForResponse?> GetByIdAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
                throw new UserNotFoundException(id);
            return _mapper.Map<UserForResponse>(user);
        }

        public async void DeleteUserAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
                throw new UserNotFoundException(id);

            await _userRepo.DeleteAsync(user);
        }

        public async Task<PagedResponse<UserForResponse>> GetUsersPagedAsync(
            UserFilterParams filters,
            PaginationParams pagination)
        {
            var spec = new UserByFilterSpecification(filters, pagination);

            var users = await _userRepo.ListAsync(spec);
            var total = await _userRepo.CountAsync(spec);

            var dtos = _mapper.Map<List<UserForResponse>>(users);
            return new PagedResponse<UserForResponse>(
                dtos,
                total,
                pagination.PageNumber,
                pagination.PageSize
            );
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userRepo.EmailExistsAsync(email);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _userRepo.UsernameExistsAsync(username);
        }

        public async Task<User> CreateUserAsyncWithInvitation(RegisterWithTokenDto dto, int roleId, int? apartmentId)
        {
            await ValidateRoleAndApartment(roleId, apartmentId);
            await ValidateEmailAndUsername(dto.Email, dto.Username);

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = _hashingService.HashPassword(dto.Password);
            user.RoleId = roleId;
            user.ApartmentId = roleId == 0 || roleId == 1 ? null : apartmentId;
            user.IsActive = true;

            return await _userRepo.AddAsync(user);
        }

        public async Task<UserForResponse> CreateUserAsync(UserForCreateDTO dto)
        {
            await ValidateRoleAndApartment(dto.RoleId, dto.ApartmentId);
            await ValidateEmailAndUsername(dto.Email, dto.Username);

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = _hashingService.HashPassword(dto.Password);
            user.ApartmentId = dto.RoleId == 0 || dto.RoleId == 1 ? null : dto.ApartmentId;
            user.IsActive = true;
            await _userRepo.AddAsync(user);

            return _mapper.Map<UserForResponse>(user);
        }

        private async Task ValidateRoleAndApartment(int roleId, int? apartmentId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
                throw new InvalidRoleException(roleId);

            if (roleId != 0 && roleId != 1 && !apartmentId.HasValue)
                throw new ApartmentRequiredException();

            if (apartmentId.HasValue && _apartmentRepo != null)
            {
                var apartment = await _apartmentRepo.GetByIdAsync(apartmentId.Value);
                if (apartment == null)
                    throw new ApartmentNotFoundException(apartmentId.Value);
            }
        }

        private async Task ValidateEmailAndUsername(string email, string username)
        {
            if (await UsernameExistsAsync(username))
                throw new UsernameAlreadyExistsException(username);

            if (await EmailExistsAsync(email))
                throw new EmailAlreadyExistsException(email);
        }
    }
}