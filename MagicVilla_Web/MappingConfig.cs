using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web
{
    public class MappingConfig: Profile
    {
        public MappingConfig() 
        { 
            //Para mapear el objeto y poder hacer conversiones de uno a otro
            CreateMap<VillaDto, VillaCreateDto>().ReverseMap();
            CreateMap<VillaDto, VillaUpdateDto>().ReverseMap();

            CreateMap<NumeroVillaDto, NumeroVillaCreateDto>().ReverseMap();
            CreateMap<NumeroVillaDto, NumeroVillaUpdateDto>().ReverseMap();
        }
    }
}
