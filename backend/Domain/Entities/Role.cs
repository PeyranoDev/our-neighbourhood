using Domain.Common.Enum;

namespace Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public UserRoleEnum Type { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
