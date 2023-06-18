using Application.DTOs;
using AutoMapper;
using Domain;

namespace Application.Profiles
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<Genre, IdDto>();
            CreateMap<IdDto, Genre>();
        }
    }
}
