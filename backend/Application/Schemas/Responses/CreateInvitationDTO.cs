using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Schemas.Responses
{
    public class CreateInvitationDto
    {
        public string Email { get; set; }
        public int RoleId { get; set; }
        public int? ApartmentId { get; set; }
        public int? ExpiryInHours { get; set; } 
    }
}
