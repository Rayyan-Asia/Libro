using Application.DTOs;
using AutoMapper;
using Domain;

namespace Application.Profiles
{
    public class ReturnProfile : Profile
    {
        public ReturnProfile()
        {
            CreateMap<BookReturn, BookReturnDto>();
        }
    }
}
