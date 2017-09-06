using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper
{
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDestination> EnsureServices<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression)
        {
            return mappingExpression.BeforeMap((src, dest, resolutionContext) =>
            {
                var serviceProvider = resolutionContext.GetServices();
                if (serviceProvider == null)
                {
                    throw new Exception("ServiceProvider is required for this mapping");
                }
            });
        }
    }
}
