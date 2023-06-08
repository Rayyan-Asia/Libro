using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using AutoMapper;
using Domain;

namespace Application.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile() {
            CreateMap<Book, BookDto>();
        }
    }
}
