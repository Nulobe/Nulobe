using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.System.Collections.Generic
{
    public static class ListHelpers
    {
        public static List<TResult> Seed<TResult>(int count) where TResult : class, new()
        {
            return Seed(count, i => Activator.CreateInstance<TResult>());
        }

        public static List<TResult> SeedWithValue<TResult>(int count, TResult value)
        {
            return Seed(count, i => value);
        }

        public static List<TResult> Seed<TResult>(int count, Func<int, TResult> valueFunc)
        {
            var result = new List<TResult>();
            for (var i = 0; i < count; i++)
            {
                result.Add(valueFunc(i));
            }
            return result;
        }

        public static List<int> Range(int start, int end)
        {
            return Seed(end - start + 1, i => i + start);
        }
    }
}
