using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Schemas.Requests
{
    public class TowerFilterParams : PaginationParams
    {
        public string? Name { get; set; }
    }
}