using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper
{
    public static class MapperExtensions
    {
        public static TDestination MapWithServices<TSource, TDestination>(
            this IMapper mapper,
            TSource source,
            IServiceProvider serviceProvider)
        {
            return mapper.Map<TSource, TDestination>(
                source,
                opts => opts.AddServices(serviceProvider));
        }
    }
}
