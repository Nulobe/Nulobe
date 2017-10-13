using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources.Impl
{
    public class SourceFieldsDictionary : ISourceFieldDictionary
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
            },
            {
                SourceType.Doi,
                new string[]
                {
                    SourceFields.Doi,
                    SourceFields.NotesMarkdown
                }
            },
            {
                SourceType.Apa,
                new string[]
                {
                    SourceFields.ApaType,
                    SourceFields.Citation,
                    SourceFields.NotesMarkdown
                }
            }
        }
        .ToReadOnlyDictionary();

        IEnumerable<string> ISourceFieldDictionary.this[SourceType key]
            => WithSourceTypeField(_sourceFieldDictionary[key]);

        IDictionary<SourceType, IEnumerable<string>> ISourceFieldDictionary.GetDictionary()
            => _sourceFieldDictionary.ToDictionary(kvp => kvp.Key, kvp => WithSourceTypeField(kvp.Value));


        #region Helpers

        private IEnumerable<string> WithSourceTypeField(IEnumerable<string> fields)
            => new string[] { SourceFields.Type }.Concat(fields);

        #endregion
    }
}
