using Application.Schemas.Requests;
using Application.Schemas.Responses;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Schemas.Profiles
{
    public class TowerProfile : Profile
    {
        public TowerProfile()
        {
            CreateMap<Tower, TowerForUserResponseDTO>();
            
            CreateMap<TowerForCreateDTO, Tower>();

            CreateMap<TowerForUpdateDTO, Tower>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
