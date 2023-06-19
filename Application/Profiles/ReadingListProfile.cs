using Application.DTOs;
using AutoMapper;
using Domain;

namespace Application.Profiles
{
    public class ReadingListProfile : Profile
    {
        public ReadingListProfile()
        {
            CreateMap<ReadingList, ReadingListDto>();
        }

    }
}
