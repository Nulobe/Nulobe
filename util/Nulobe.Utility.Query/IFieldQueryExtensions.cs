using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.Query
{
    public static class IFieldQueryExtensions
    {
        public static IEnumerable<string> GetFieldPropertyNames<T>(
            this IFieldQuery<T> query)
        {
            if (string.IsNullOrEmpty(query.Fields))
            {
                return Enumerable.Empty<string>();
            }

            return query.Fields
               .Split(',')
               .Select(f => f.Trim())
               .Select(f =>
               {
                   var propertyName = typeof(T)
                       .GetProperties()
                       .Where(p => p.Name.Equals(f, StringComparison.InvariantCultureIgnoreCase))
                       .Select(p => p.Name)
                       .FirstOrDefault();

                   if (string.IsNullOrEmpty(propertyName))
                   {
                       throw new QueryFormatException("Unable to locate property name");
                   }

                   return propertyName;
               });
        }
    }
}
