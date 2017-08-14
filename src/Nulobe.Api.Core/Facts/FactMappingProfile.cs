using AutoMapper;
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
            CreateMap<FactCreate, Fact>();
        }
    }
}
