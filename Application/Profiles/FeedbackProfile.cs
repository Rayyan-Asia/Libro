using Application.DTOs;
using AutoMapper;
using Domain;

namespace Application.Profiles
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<Feedback, FeedbackDto>();
        }

    }
}
