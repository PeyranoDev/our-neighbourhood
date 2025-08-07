using Application.Schemas.Requests;
using Application.Schemas.Responses;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Presentation.Helpers;
using Presentation.Middlewares;
using Application.Services.Interfaces;
using System.Net;
using System.Threading.Tasks;

namespace Presentation.Functions
{
    public class NotificationFunctions
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<NotificationFunctions> _logger;

        public NotificationFunctions(ITokenService tokenService, ILogger<NotificationFunctions> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }

        [Function("RegisterPushToken")]
        [Authorize]
        public async Task<HttpResponseData> RegisterPushToken(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "notification/register")] HttpRequestData req,
            FunctionContext context)
        {
            var userId = context.GetUserId();
            _logger.LogInformation("Registering push token for user ID: {UserId}", userId);

            var dto = await req.ReadFromJsonAsync<NotificationTokenCreateDTO>();
            if (dto == null || string.IsNullOrEmpty(dto.Token))
            {
                return await req.CreateJsonResponse(HttpStatusCode.BadRequest, ApiResponse<object>.Fail("Token cannot be null or empty."));
            }

            var success = await _tokenService.AddNotificationTokenAsync(dto, userId);

            if (success)
            {
                return await req.CreateJsonResponse(HttpStatusCode.OK, ApiResponse<object>.Ok("Token registered successfully."));
            }

            return await req.CreateJsonResponse(HttpStatusCode.InternalServerError, ApiResponse<object>.Fail("Failed to register token."));
        }
    }
}
