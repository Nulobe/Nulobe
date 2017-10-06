using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources
{
    public static class SourceFields
    {
        public static string Type = "Type";

        public static string Url = "Url";

        public static string Description = "Description";

        public static string FactId = "FactId";

        public static string ApaType = "ApaType";

        public static class Apa
        {
            public const string Authors = "Authors";

            public const string Date = "Date";

            public const string Title = "Title";

            public const string Edition = "Edition";

            public const string Pages = "Pages";

            public const string Doi = "Doi";

            public const string Organisation = "Organisation";
        }
    }
}
