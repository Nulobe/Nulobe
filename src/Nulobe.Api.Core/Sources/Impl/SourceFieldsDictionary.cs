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
        private readonly IReadOnlyDictionary<SourceType, IEnumerable<string>> _sourceFieldDictionary = new Dictionary<SourceType, IEnumerable<string>>()
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

        private readonly IReadOnlyDictionary<ApaSourceType, IEnumerable<string>> _apaSourceFieldDictionary = new Dictionary<ApaSourceType, IEnumerable<string>>()
        {
            {
                ApaSourceType.JournalArticle,
                new string[]
                {
                    SourceFields.ApaType,
                    SourceFields.Apa.Authors,
                    SourceFields.Apa.Organisation,
                    SourceFields.Apa.Date,
                    SourceFields.Apa.Title,
                    SourceFields.Apa.Journal,
                    SourceFields.Apa.Pages,
                    SourceFields.Apa.Doi,
                    SourceFields.Url,
                }
            },
            {
                ApaSourceType.Book,
                new string[]
                {
                    SourceFields.ApaType,
                    SourceFields.Apa.Authors,
                    SourceFields.Apa.Organisation,
                    SourceFields.Apa.Date,
                    SourceFields.Apa.Title,
                    SourceFields.Apa.Edition,
                    SourceFields.Apa.Pages,
                    SourceFields.Url
                }
            },
            {
                ApaSourceType.Webpage,
                new string[]
                {
                    SourceFields.ApaType,
                    SourceFields.Apa.Authors,
                    SourceFields.Apa.Organisation,
                    SourceFields.Apa.Date,
                    SourceFields.Apa.Title,
                    SourceFields.Url
                }
            },
            {
                ApaSourceType.Newspaper,
                new string[]
                {

                }
            },
            {
                ApaSourceType.Magazine,
                new string[]
                {

                }
            },
            
        }
        .ToReadOnlyDictionary();

        IEnumerable<string> IApaSourceFieldDictionary.this[ApaSourceType key] => WithApaSourceTypeFields(_apaSourceFieldDictionary[key]);

        IEnumerable<string> ISourceFieldDictionary.this[SourceType key]
        {
            get
            {
                if (key == SourceType.Apa)
                {
                    throw new InvalidOperationException($"Use {nameof(IApaSourceFieldDictionary)} to lookup APA sources");
                }

                return WithSourceTypeField(_sourceFieldDictionary[key]);
            }
        }

        IDictionary<SourceType, IEnumerable<string>> ISourceFieldDictionary.GetDictionary()
            => _sourceFieldDictionary.ToDictionary(kvp => kvp.Key, kvp => WithSourceTypeField(kvp.Value));

        IDictionary<ApaSourceType, IEnumerable<string>> IApaSourceFieldDictionary.GetDictionary()
            => _apaSourceFieldDictionary.ToDictionary(kvp => kvp.Key, kvp => WithApaSourceTypeFields(kvp.Value));


        #region Helpers

        private IEnumerable<string> WithApaSourceTypeFields(IEnumerable<string> fields)
            => WithSourceTypeField(new string[] { SourceFields.ApaType }.Concat(fields));

        private IEnumerable<string> WithSourceTypeField(IEnumerable<string> fields)
            => new string[] { SourceFields.Type }.Concat(fields);

        #endregion
    }
}
