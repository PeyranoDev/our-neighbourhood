using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Schemas.Responses
{
    internal class VehicleForResposneDTO
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public bool IsParked { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
    }
}
