﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Schemas.Responses
{
    public class ApartmentInfoDTO
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public TowerForUserResponseDTO? Tower { get; set; }
    }
}
