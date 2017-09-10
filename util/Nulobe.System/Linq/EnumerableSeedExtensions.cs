using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class EnumerableSeedExtensions
    {
        public static IEnumerable<T> Seed<T>(
            int count,
            Func<T, T> accumulatorFunc,
            T initial)
        {
            return Seed(count, (prev, index) => accumulatorFunc(prev), initial);
        }

        public static IEnumerable<T> Seed<T>(
            int count,
            Func<T, int, T> accumulatorFunc,
            T initial)
        {
            yield return initial;
            var current = initial;
            for (int i = 0; i < count - 1; i++)
            {
                current = accumulatorFunc(current, i);
                yield return current;
            }
        }
    }
}
