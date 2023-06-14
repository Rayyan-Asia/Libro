﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Entities.Profiles.Queries
{
    public class ViewProfileQuery : IRequest<ProfileDto>
    {
        public int PatronId { get; set; }
    }
}