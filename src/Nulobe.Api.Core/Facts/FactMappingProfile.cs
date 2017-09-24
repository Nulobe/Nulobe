using AutoMapper;
using Microsoft.Extensions.Options;
using Nulobe.Api.Core.Sources;
using Nulobe.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Facts
{
    public class FactMappingProfile : Profile
    {
        public FactMappingProfile()
        {
            CreateMap<FactCreate, FactData>()
                //.ForMember(dest => dest.Sources, opts => opts.ResolveUsing(src => src.Sources.Select(s => (IDictionary<string, object>)s)));
                .ForMember(
                    dest => dest.Sources,
                    opts => opts.ResolveUsing(src => src.Sources.Select(s => s.ToObject<IDictionary<string, object>>())));


            CreateMap<FactData, Fact>()
                .EnsureServices()
                .ForMember(
                    dest => dest.Sources,
                    opts => opts.ResolveUsing(src => src.Sources.Select(s =>
                    {
                        if (!s.ContainsKey("type"))
                        {
                            s.Add("type", SourceType.Legacy);
                        }
                        return s;
                    })))
                .BeforeMap((factData, fact) =>
                {
                    fact.ReadOnlyTags = Enumerable.Empty<string>();
                })
                .AfterMap((factData, fact, context) =>
                {
                    var countryOptions = context.GetRequiredService<IOptions<CountryOptions>>().Value;
                    if (!string.IsNullOrEmpty(factData.Country))
                    {
                        var country = countryOptions[factData.Country];
                        
                        fact.Tags = fact.Tags.Concat(country.Tag);
                        fact.ReadOnlyTags = fact.ReadOnlyTags.Concat(country.Tag);

                        fact.TitleLocalized = $"{fact.Title} ({country.ShortDisplayName ?? country.DisplayName})";
                    }
                    else
                    {
                        fact.TitleLocalized = fact.Title;
                    }

                    //foreach (var source in fact.Sources)
                    //{
                    //    if (!((IDictionary<string, object>)source).ContainsKey("type"))
                    //    {
                    //        source.type = SourceType.Legacy;
                    //    }
                    //}
                });
        }
    }
}
