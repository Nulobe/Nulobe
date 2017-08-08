using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Events
{
    public class EventMappingProfile : Profile
    {
        public EventMappingProfile()
        {
            CreateMap<FlagCreate, Event>()
                .ForMember(dest => dest.Type, opts => opts.UseValue(EventType.Flag))
                .AfterMap((src, dest) =>
                {
                    dest.Data = new
                    {
                        Desription = src.Description
                    };
                });

            CreateMap<VoteCreate, Event>()
                .ForMember(dest => dest.Type, opts => opts.UseValue(EventType.Like));
        }
    }
}
