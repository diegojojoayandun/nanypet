using AutoMapper;
using NanyPet.Api.Models;
using NanyPet.Api.Models.Dto.Login;
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


            CreateMap<User, UserDto>().ReverseMap();
            //CreateMap<User, UserCreateDto>().ReverseMap();
            // CreateMap<Owner, OwnerUpdateDto>().ReverseMap();
        }
    }
}
