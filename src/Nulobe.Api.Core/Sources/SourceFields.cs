using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources
{
    public static class SourceFields
    {
        public const string Type = "type";

        public const string Title = "title";

        public const string Url = "url";

        public const string Description = "description";

        public const string FactId = "factId";

        public const string FactTitle = "factTitle";

        public const string ApaType = "apaType";

        public const string NotesMarkdown = "notesMarkdown";

        public const string CitationFromDoi = "citationFromDoi";



        public static class Apa
        {
            public const string Authors = "authors";

            public const string Date = "date";

            public const string Title = "title";

            public const string Edition = "edition";

            public const string Pages = "pages";

            public const string Doi = "doi";

            public const string Organisation = "organisation";

            public const string Journal = "journal";

            public const string JournalTitle = Journal + ".title";

            public const string JournalVolume = Journal + ".volume";

            public const string JournalIssue = Journal + ".issue";
        }
    }
}
