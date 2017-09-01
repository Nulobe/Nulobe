using Nulobe.Utility.Query;

namespace Nulobe.Api.Core.Facts
{
    public class FactQuery : IPageQuery, IFieldQuery<Fact>
    {
        public string Tags { get; set; }

        public string Slug { get; set; }

        public string Pattern { get; set; }

        public string Fields { get; set; }

        public string OrderBy { get; set; } = $"{nameof(Fact.Title)}";

        public string PageNumber { get; set; }

        public string PageSize { get; set; }
    }
}