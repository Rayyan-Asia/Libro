using Application.DTOs;
using AutoMapper;
using Domain;

namespace Application.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>();
        }

    }
}
