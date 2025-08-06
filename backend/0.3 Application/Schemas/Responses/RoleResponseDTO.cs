using Domain.Common.Enum;

namespace Application.Schemas.Responses
{
    public class RoleResponseDto
    {
        public int Id { get; set; }
        public UserRoleEnum Type { get; set; }
    }
}
