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
                .BeforeMap((factData, fact) =>
                {
                    fact.ReadOnlyTags = Enumerable.Empty<string>();
                })
                .AfterMap((factData, fact, context) =>
                {
                    var countryOptions = context.GetRequiredService<IOptions<CountryOptions>>().Value;
                    if (!string.IsNullOrEmpty(factData.Country))
                    {
                        var countryTag = countryOptions[factData.Country].Tag;
                        fact.Tags = fact.Tags.Concat(countryTag);
                        fact.ReadOnlyTags = fact.ReadOnlyTags.Concat(countryTag);
                    }
                });
        }
    }
}
