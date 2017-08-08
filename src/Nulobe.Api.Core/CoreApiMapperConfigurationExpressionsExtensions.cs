using Nulobe.Api.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper
{
    public static class CoreApiMapperConfigurationExpressionsExtensions
    {
        public static void AddCoreApiMapperConfigurations(
            this IMapperConfigurationExpression conf)
        {
            conf.AddProfile<EventMappingProfile>();
        }
    }
}
