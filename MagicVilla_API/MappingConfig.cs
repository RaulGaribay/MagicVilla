﻿using AutoMapper;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;

namespace MagicVilla_API
{
    public class MappingConfig: Profile
    {
        public MappingConfig() 
        { 
            //Para mapear el objeto y poder hacer conversiones de uno a otro
            CreateMap<Villa, VillaDto>().ReverseMap();
            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();

            CreateMap<NumeroVilla, NumeroVillaDto>().ReverseMap();
            CreateMap<NumeroVilla, NumeroVillaCreateDto>().ReverseMap();
            CreateMap<NumeroVilla, NumeroVillaUpdateDto>().ReverseMap();

            CreateMap<Usuario, RegistroRequestDTO>().ReverseMap();
            CreateMap<UsuarioAplicacion, UsuarioDto>().ReverseMap();
        }
    }
}
