using AutoMapper;
using Microsoft.Extensions.Options;
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
            CreateMap<FactCreate, FactData>();

            CreateMap<FactData, Fact>()
                .EnsureServices()
                .ForMember(f => f.SlugHistory, opts => opts.Ignore())
                .AfterMap((factData, fact, context) =>
                {
                    var countryOptions = context.GetRequiredService<IOptions<CountryOptions>>().Value;
                    if (!string.IsNullOrEmpty(factData.Country))
                    {
                        fact.Tags = fact.Tags.Concat(countryOptions[factData.Country].Tag);
                    }
                });
        }
    }
}
