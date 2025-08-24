using AutoMapper;
using Op.Prueba.Application.DTOs;
using Op.Prueba.Domain.Entities;

namespace OP.Prueba.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Owner, OwnerDto>();
        }
    }
}