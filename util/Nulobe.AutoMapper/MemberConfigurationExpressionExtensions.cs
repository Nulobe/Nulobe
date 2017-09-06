using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper
{
    public static class MemberConfigurationExpressionExtensions
    {
        public static void ResolveUsingServices<TSource, TDestination, TMember, TService>(
            this IMemberConfigurationExpression<TSource, TDestination, TMember> expression,
            Func<TService, TSource, TMember> resolver)
        {
            expression.ResolveUsing((source, destination, member, resolutionContext) =>
            {
                var service = resolutionContext.GetRequiredService<TService>();
                var result = resolver(service, source);
                return result;
            });
        }
    }
}
