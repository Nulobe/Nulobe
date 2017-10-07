using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources
{
    public static class SourceFields
    {
        public static string Type = "type";

        public static string Title = "title";

        public static string Url = "url";

        public static string Description = "description";

        public static string FactId = "factId";

        public static string ApaType = "apaType";

        public static class Apa
        {
            public const string Authors = "authors";

            public const string Date = "date";

            public const string Title = "title";

            public const string Edition = "edition";

            public const string Pages = "pages";

            public const string Doi = "doi";

            public const string Organisation = "organisation";
        }
    }
}
