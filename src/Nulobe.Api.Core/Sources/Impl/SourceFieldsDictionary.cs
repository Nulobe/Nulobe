using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources.Impl
{
    public class SourceFieldsDictionary : ISourceFieldDictionary, IApaSourceFieldDictionary
    {
        private readonly IReadOnlyDictionary<SourceType, IEnumerable<string>> _apaSourceFieldDictionary = new Dictionary<SourceType, IEnumerable<string>>()
        {
            {
                SourceType.CitationNeeded,
                Enumerable.Empty<string>()
            },
            {
                SourceType.Nulobe,
                new string[]
                {
                    SourceFields.FactId
                }
            },
            {
                SourceType.Legacy,
                new string[]
                {
                    SourceFields.Description,
                    SourceFields.Url
                }
            }
        }
        .ToReadOnlyDictionary();

        private readonly IReadOnlyDictionary<ApaSourceType, IEnumerable<string>> _internalApaSourceFieldDictionary = new Dictionary<ApaSourceType, IEnumerable<string>>()
        {
            {
                ApaSourceType.JournalArticle,
                new string[]
                {
                    SourceFields.Apa.Authors,
                    SourceFields.Apa.Pages,
                    SourceFields.Apa.Organisation,
                    SourceFields.Apa.Doi,
                    SourceFields.Url,
                }
            },
            {
                ApaSourceType.Book,
                new string[]
                {
                    SourceFields.Apa.Authors,
                    SourceFields.Apa.Edition,
                    SourceFields.Apa.Pages,
                    SourceFields.Apa.Organisation,
                    SourceFields.Url
                }
            },
            {
                ApaSourceType.Webpage,
                new string[]
                {
                    SourceFields.Apa.Authors,
                    SourceFields.Apa.Organisation,
                    SourceFields.Url
                }
            }
        }
        .ToReadOnlyDictionary();

        public IEnumerable<string> this[ApaSourceType key] => WithSourceTypeField(_internalApaSourceFieldDictionary[key]);

        public IEnumerable<string> this[SourceType key]
        {
            get
            {
                if (key == SourceType.Apa)
                {
                    throw new InvalidOperationException($"Use {nameof(IApaSourceFieldDictionary)} to lookup APA sources");
                }

                return WithSourceTypeField(_apaSourceFieldDictionary[key]);
            }
        }

        private IEnumerable<string> WithApaSourceTypeFields(IEnumerable<string> fields)
            => WithSourceTypeField(new string[] { SourceFields.ApaType }.Concat(fields));

        private IEnumerable<string> WithSourceTypeField(IEnumerable<string> fields)
            => new string[] { SourceFields.Type }.Concat(fields);
    }
}
