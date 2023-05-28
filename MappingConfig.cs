using AutoMapper;
using NanyPet.Models;
using NanyPet.Models.Dto.Herder;
using NanyPet.Models.Dto.Owner;

namespace NanyPet
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Herder, HerderDto>().ReverseMap();
            CreateMap<Herder, HerderCreateDto>().ReverseMap();
            CreateMap<Herder, HerderUpdateDto>().ReverseMap();

            CreateMap<Owner, OwnerDto>().ReverseMap();
            CreateMap<Owner, OwnerCreateDto>().ReverseMap();
            CreateMap<Owner, OwnerUpdateDto>().ReverseMap();
        }
    }
}
