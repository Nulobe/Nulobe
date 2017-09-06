using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static (IEnumerable<T> matches, IEnumerable<T> nonMatches) Fork<T>(
            this IEnumerable<T> source,
            Func<T, bool> pred)
        {
            var groupedByMatching = source.ToLookup(pred);
            return (groupedByMatching[true], groupedByMatching[false]);
        }

        public static int AnyIndex<T>(
            this IEnumerable<T> source,
            Func<T, bool> pred)
        {
            var indexedSource = source.Select((i, ix) => new
            {
                Item = i,
                Index = ix
            });

            foreach (var item in indexedSource)
            {
                if (pred(item.Item))
                {
                    return item.Index;
                }
            }

            return -1;
        }

        public static IEnumerable<T> Concat<T>(
            this IEnumerable<T> source,
            T item)
        {
            return source.Concat(new T[] { item });
        }
    }
}
