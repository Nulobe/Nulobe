﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nulobe.Utility.Validation;

namespace Nulobe.Api.Core.Sources.Impl
{
    public class LegacySourceHandler : BaseSourceHandler
    {
        public LegacySourceHandler() : base(SourceType.Legacy)
        {
        }

        public override void PreValidate(JObject source, ModelErrorDictionary errors)
        {
            string url = null;
            var urlToken = source.SelectToken(SourceFields.Url);
            if (urlToken == null)
            {
                errors.AddRequired(SourceFields.Url);
            }
            else if (urlToken is JValue urlValue)
            {
                if (urlToken.Type == JTokenType.String)
                {
                    url = urlToken.ToString();

                    if (!ValidationUtility.IsValidUri(url))
                    {
                        errors.AddUriExpected(SourceFields.Url);
                    }
                }
                else
                {
                    errors.AddUriExpected(SourceFields.Url);
                }
            }
        }
    }
}
