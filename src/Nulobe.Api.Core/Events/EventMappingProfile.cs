using AutoMapper;
using Newtonsoft.Json;
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
            CreateMap<IEventCreate, Event>()
                .ForMember(dest => dest.RowKey, opts => opts.ResolveUsing(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.PartitionKey, opts => opts.ResolveUsing(src => src.FactId));

            CreateMap<FlagCreate, Event>()
                .IncludeBase<IEventCreate, Event>()
                .ForMember(dest => dest.EventType, opts => opts.UseValue("flag"))
                .ForMember(dest => dest.DataJson, opts => opts.ResolveUsing(src => JsonConvert.SerializeObject(new
                {
                    Desription = src.Description
                })));

            CreateMap<VoteCreate, Event>()
                .IncludeBase<IEventCreate, Event>()
                .ForMember(dest => dest.EventType, opts => opts.UseValue("like"));
        }
    }
}
