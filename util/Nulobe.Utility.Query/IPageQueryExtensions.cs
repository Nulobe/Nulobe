using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.Query
{
    public static class IPageQueryExtensions
    {
        private const int DefaultPageNumber = 1;
        private const int DefaultDefaultPageSize = 10;
        private const int DefaultMaxPageSize = 50;

        public static int GetPageNumber(
            this IPageQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.PageNumber))
            {
                return DefaultPageNumber;
            }
            else if (int.TryParse(query.PageNumber, out int parsedPageNumber))
            {
                if (parsedPageNumber < 1)
                {
                    throw new Exception("Page number was out of ranger");
                }
                return parsedPageNumber;
            }
            else
            {
                throw new Exception("Could not parse number from query string");
            }
        }

        public static int GetPageSize(
            this IPageQuery query,
            int maximumPageSize = DefaultMaxPageSize,
            int defaultPageSize = DefaultDefaultPageSize)
        {
            int requestedPageSize = defaultPageSize;
            var requestedPageSizeStr = query.PageSize;
            if (!string.IsNullOrWhiteSpace(requestedPageSizeStr))
            {
                int.TryParse(requestedPageSizeStr, out requestedPageSize);
            }

            if (requestedPageSize > maximumPageSize)
            {
                throw new QueryFormatException("Exceeded maximum page size");
            }

            return requestedPageSize;
        }
    }
}
