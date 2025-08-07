using Application.Providers;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Repository;
using Domain.Common.Enum;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly INotificationSender _notificationSender;

        public NotificationService(
            IUserRepository userRepository,
            ITokenRepository tokenRepository,
            IVehicleRepository vehicleRepository,
            INotificationSender notificationSender)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _vehicleRepository = vehicleRepository;
            _notificationSender = notificationSender;
        }

        public async Task SendVehiclePreparationNotificationAsync(int vehicleId, int securityUserId)
        {
            var (vehicle, owner, token) = await GetValidatedVehicleOwnerAndToken(vehicleId);
            var securityUser = await GetUserWithValidation(securityUserId);

            await SendNotification(new NotificationRequest
            {
                UserId = vehicle.OwnerId,
                Title = $"{owner.Name}, tu {vehicle.Model} está siendo preparado!",
                Body = $"Está siendo preparado por {securityUser.Name}. En unos minutos te avisaremos cuando esté casi listo!",
                Type = "vehicle_preparing",
                Data = new Dictionary<string, string> { { "vehicleModel", vehicle.Model } }
            });
        }

        public async Task SendVehicleRequestNotificationForSecurity(int vehicleId, int userId)
        {
            var securityUsers = await _userRepository.GetAsQueryable()
                .Where(u => u.Role.Type == UserRoleEnum.Security && u.IsOnDuty)
                .ToListAsync();
            var tasks = securityUsers.Select(user =>
                SendSecurityNotification(user, vehicleId, userId)
            );

            await Task.WhenAll(tasks);
        }

        public async Task SendVehicleReadyNotificationForUser(int vehicleId)
        {
            var (vehicle, owner, _) = await GetValidatedVehicleOwnerAndToken(vehicleId);
            await SendNotification(new NotificationRequest
            {
                UserId = vehicle.OwnerId,
                Title = $"{owner.Name}, ¡tu vehículo está listo!",
                Body = $"Tu {vehicle.Model} está listo para ser recogido.",
                Type = "vehicle_ready"
            });
        }

        public async Task SendVehicleAlmostReadyNotificationForUser(int vehicleId)
        {
            var (vehicle, owner, _) = await GetValidatedVehicleOwnerAndToken(vehicleId);
            await SendNotification(new NotificationRequest
            {
                UserId = vehicle.OwnerId,
                Title = $"{owner.Name}, tu {vehicle.Model} está casi listo!",
                Body = "Por favor ve dirigiéndote al entrepiso para retirarlo.",
                Type = "vehicle_almost_ready"
            });
        }

        public async Task SendVehicleCancelledNotificationForUser(int vehicleId)
        {
            var (vehicle, owner, _) = await GetValidatedVehicleOwnerAndToken(vehicleId);
            await SendNotification(new NotificationRequest
            {
                UserId = vehicle.OwnerId,
                Title = $"{owner.Name}, tu {vehicle.Model} ha sido cancelado!",
                Body = "¡Haz click para ver más información!",
                Type = "vehicle_cancelled"
            });
        }

        #region Private Methods
        private async Task SendNotification(NotificationRequest request)
        {
            var token = await GetValidNotificationToken(request.UserId);
            if (token == null) return;

            var data = new Dictionary<string, string>
            {
                { "type", request.Type },
                { "vehicleId", request.UserId.ToString() }
            };

            if (request.Data != null)
            {
                foreach (var item in request.Data)
                    data[item.Key] = item.Value;
            }

            await _notificationSender.SendAsync(token.Token, request.Title, request.Body, data);
            await UpdateTokenLastUsed(token.Id);
        }

        private async Task SendSecurityNotification(User user, int vehicleId, int requestedById)
        {
            var token = await GetValidNotificationToken(user.Id);
            if (token == null) return;

            var data = new Dictionary<string, string>
            {
                { "type", "security_vehicle_request" },
                { "vehicleId", vehicleId.ToString() },
                { "requestedById", requestedById.ToString() }
            };

            await _notificationSender.SendAsync(
                token.Token,
                $"Hola {user.Name}, ¡nueva solicitud de vehículo!",
                "Revisa la aplicación para más detalles.",
                data
            );

            await UpdateTokenLastUsed(token.Id);
        }

        private async Task<NotificationToken?> GetValidNotificationToken(int userId)
        {
            var token = await _tokenRepository.GetAsQueryable()
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync();
            return token != null && !string.IsNullOrEmpty(token.Token) ? token : null;
        }

        private async Task UpdateTokenLastUsed(int tokenId)
        {
            var token = await _tokenRepository.GetAsQueryable()
                .FirstOrDefaultAsync(t => t.Id == tokenId);
            if (token != null)
            {
                token.LastSeen = DateTime.UtcNow;
                await _tokenRepository.UpdateAsync(token);
            }
        }

        private async Task<(Vehicle vehicle, User owner, NotificationToken token)> GetValidatedVehicleOwnerAndToken(int vehicleId)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId) ??
                throw new InvalidOperationException("Vehículo no encontrado");

            var owner = await _userRepository.GetAsQueryable()
                .Include(u => u.NotificationTokens)
                .FirstOrDefaultAsync(u => u.Id == vehicle.OwnerId) ??
                throw new InvalidOperationException("Dueño del vehículo no encontrado");

            var token = owner.NotificationTokens
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefault(t => !string.IsNullOrEmpty(t.Token)) ??
                throw new InvalidOperationException("Usuario no tiene token de notificación válido");

            return (vehicle, owner, token);
        }

        private async Task<User> GetUserWithValidation(int userId)
        {
            return await _userRepository.GetByIdAsync(userId) ??
                throw new InvalidOperationException("Usuario no encontrado");
        }

        private class NotificationRequest
        {
            public int UserId { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public string Type { get; set; }
            public Dictionary<string, string>? Data { get; set; }
        }
        #endregion
    }
}
