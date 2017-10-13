using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nulobe.Utility.Validation;

namespace Nulobe.Api.Core.Sources.Impl
{
    public class ApaSourceHandler : BaseSourceHandler
    {
        public ApaSourceHandler() : base(SourceType.Apa) { }

        public override void PreValidate(JObject source, ModelErrorDictionary errors)
        {
            var apaSourceType = GetValidApaSourceType(source, errors);
            if (errors.HasErrors)
            {
                return;
            }


        }


        #region Helpers

        private ApaSourceType GetValidApaSourceType(JObject source, ModelErrorDictionary errors)
        {
            var apaSourceType = ApaSourceType.Unknown;

            var apaSourceTypeToken = source.SelectToken(SourceFields.ApaType);
            if (apaSourceTypeToken == null)
            {
                errors.AddRequired(SourceFields.ApaType);
            }
            else if (apaSourceTypeToken is JValue jValue)
            {
                if (jValue.Type == JTokenType.Integer)
                {
                    var candidateApaSourceType = (ApaSourceType)jValue.ToObject<int>();
                    if (!Enum.IsDefined(typeof(ApaSourceType), candidateApaSourceType) || candidateApaSourceType == ApaSourceType.Unknown)
                    {
                        errors.AddOutOfRange(SourceFields.ApaType);
                    }
                    else
                    {
                        apaSourceType = candidateApaSourceType;
                    }
                }
                else
                {
                    errors.AddIntegerExpected(SourceFields.ApaType);
                }
            }

            return apaSourceType;
        }

        #endregion
    }
}
