using Newtonsoft.Json.Linq;
using Nulobe.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources.SourceValidationHandlers
{
    public class ApaSourceValidationHandler : ISourceValidationHandler
    {
        private const int MinimumYear = 1900;

        public SourceType Type => SourceType.Apa;

        public IEnumerable<string> Fields => new string[] { "apaType", "authors" };

        public Task<SourceValidationResult> IsValidAsync(dynamic source)
        {
            var errors = new ModelErrorDictionary();

            void apaSourceTypeInvalid(string message) => errors.Add("ApaType", message);

            ApaSourceType apaSourceType = ApaSourceType.Unknown;
            var apaSourceTypeInt = (int?)source.apaType;
            if (apaSourceTypeInt.HasValue)
            {
                apaSourceType = (ApaSourceType)apaSourceTypeInt;
                if (!Enum.IsDefined(typeof(ApaSourceType), apaSourceType) || apaSourceType == ApaSourceType.Unknown)
                {
                    apaSourceTypeInvalid("APA source type value is invalid");
                }
            }
            else
            {
                apaSourceTypeInvalid("APA source type is required");
            }

            errors.Add(ValidateAuthors(source.authors), "Authors");
            errors.Add(ValidateDate(source.date), "Date");

            return Task.FromResult(errors.Any() ? SourceValidationResult.Invalid(errors) : SourceValidationResult.Valid());
        }

        private ModelErrorDictionary ValidateAuthors(dynamic authors)
        {
            var errors = new ModelErrorDictionary();

            if (authors is JArray)
            {
                var authorsArray = (JArray)authors;
                for (var i = 0; i < authorsArray.Count(); i++)
                {
                    var author = authorsArray[i] as JValue;
                    if (author == null || string.IsNullOrEmpty(author.ToObject<string>()))
                    {
                        errors.Add("Author must not a non-empty string", $"[{i}]");
                    }
                }
            }
            else if (authors != null)
            {
                errors.Add("Expected authors to be a JSON array");
            }

            return errors;
        }

        private ModelErrorDictionary ValidateDate(dynamic date)
        {
            var errors = new ModelErrorDictionary();

            if (date is JObject)
            {
                var dateJObject = (JObject)date;

                var (year, yearErrors) = GetValidInteger(dateJObject.SelectToken("year"), 1900, DateTime.UtcNow.Year);
                errors.Add(yearErrors, "Year");

                if  (!yearErrors.HasErrors)
                {
                    var (month, monthErrors) = GetValidInteger(dateJObject.SelectToken("month"), 1, 12);
                    errors.Add(monthErrors, "Month");

                    if (!monthErrors.HasErrors)
                    {
                        var daysInMonth = DateTime.DaysInMonth(year, month);
                        var (day, dayErrors) = GetValidInteger(dateJObject.SelectToken("day"), 1, daysInMonth);
                        errors.Add(dayErrors);
                    }
                }
            }
            else if (date != null)
            {
                errors.Add("Expected value to be a JSON object");
            }

            return errors;
        }

        private (int value, ModelErrorDictionary errors) GetValidInteger(dynamic value, int minimumValue, int maximumValue)
        {
            var result = default(int);
            var errors = new ModelErrorDictionary();

            if (value is JValue)
            {
                var jValue = (JValue)value;
                if (jValue.Type == JTokenType.Integer)
                {
                    var intValue = jValue.ToObject<int>();
                    if (intValue >= minimumValue && intValue <= maximumValue)
                    {
                        result = intValue;
                    }
                    else
                    {
                        errors.Add($"Expected value to be an interger, between {minimumValue} and {maximumValue}");
                    }
                }
                else
                {
                    errors.Add("Expected value to be an integer");
                }
            }
            else if (value != null)
            {
                errors.Add("Expected value to be a JSON object");
            }

            return (result, errors);
        }
    }
}
