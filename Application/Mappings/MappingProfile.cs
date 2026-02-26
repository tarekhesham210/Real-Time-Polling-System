using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Option, OptionResponseDTO>()

                .ForMember(dest => dest.VoteCount,
                           opt => opt.MapFrom(src => src.Votes.Count))

                .ForMember(dest => dest.OptionId,
                                opt => opt.MapFrom(src => src.Id));



            CreateMap<Poll, PollResponseDTO>()

                .ForMember(dest => dest.TotalVotes,
                           opt => opt.MapFrom(src => src.Options.Sum(o => o.Votes.Count)))

                .ForMember(dest => dest.CreatorId,
                           opt => opt.MapFrom(src => src.CreatedById))

                .ForMember(dest => dest.CreatorName,
                           opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))

                .ForMember(dest => dest.Options,
                           opt => opt.MapFrom(src => src.Options));
            CreateMap<CreatePollDTO, Poll>()
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options));

            CreateMap<string, Option>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src));


        }
    }
}
