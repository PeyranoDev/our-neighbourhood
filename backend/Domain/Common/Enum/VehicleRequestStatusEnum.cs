using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Enum
{
    public enum VehicleRequestStatusEnum
    {
        Pending,      
        InPreparation, 
        AlmostReady,
        Ready,         
        Cancelled,
        Completed      
    }
}
