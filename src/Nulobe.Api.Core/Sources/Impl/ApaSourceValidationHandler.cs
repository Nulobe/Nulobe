using Newtonsoft.Json.Linq;
using Nulobe.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources.Impl
{
    //public class ApaSourceValidationHandler : ISourceValidationHandler
    //{
    //    private const int MinimumYear = 1900;

    //    public SourceType Type => SourceType.Apa;

    //    public Task<SourceValidationResult> IsValidAsync(dynamic sourceOld)
    //    {
    //        JObject source = sourceOld;

    //        var errors = new ModelErrorDictionary();

    //        void apaSourceTypeInvalid(string message) => errors.Add(SourceFields.ApaType, message);

    //        ApaSourceType apaSourceType = ApaSourceType.Unknown;
    //        var apaSourceTypeToken = source.SelectToken(SourceFields.ApaType);
    //        if (apaSourceTypeToken == null)
    //        {
    //            apaSourceTypeInvalid("APA source type is required");
    //        }
    //        else if (apaSourceTypeToken is JValue apaSourceTypeValue)
    //        {
    //            if (apaSourceTypeValue.Type == JTokenType.Integer)
    //            {
    //                apaSourceType = (ApaSourceType)apaSourceTypeValue.ToObject<int>();
    //                if (!Enum.IsDefined(typeof(ApaSourceType), apaSourceType) || apaSourceType == ApaSourceType.Unknown)
    //                {
    //                    apaSourceTypeInvalid("APA source type value is invalid");
    //                }
    //            }
    //            else
    //            {
    //                apaSourceTypeInvalid("APA source type must be an integer");
    //            }
    //        }

    //        errors.Add(ValidateAuthors(source.SelectToken(SourceFields.Apa.Authors)), "Authors");
    //        errors.Add(ValidateDate(source.SelectToken(SourceFields.Apa.Date)), "Date");
    //        errors.Add(ValidateTitle(source.SelectToken(SourceFields.Apa.Title)), "Title");
    //        errors.Add(ValidateEdition(source.SelectToken(SourceFields.Apa.Edition)), "Edition");
    //        errors.Add(ValidatePages(source.SelectToken(SourceFields.Apa.Pages)), "Pages");
    //        errors.Add(ValidateDoi(source.SelectToken(SourceFields.Apa.Doi)), "Doi");
    //        errors.Add(ValidateOrganisation(source.SelectToken(SourceFields.Apa.Organisation)), "Organisation");

    //        return Task.FromResult(errors.Any() ? SourceValidationResult.Invalid(errors) : SourceValidationResult.Valid());
    //    }

    //    private ModelErrorDictionary ValidateAuthors(JToken authors)
    //    {
    //        var errors = new ModelErrorDictionary();

    //        if (authors is JArray authorsArray)
    //        {
    //            for (var i = 0; i < authorsArray.Count(); i++)
    //            {
    //                var author = authorsArray[i] as JValue;
    //                if (author == null || author.Type != JTokenType.String || string.IsNullOrEmpty(author.ToObject<string>()))
    //                {
    //                    errors.Add("Author must not a non-empty string", i);
    //                }
    //            }
    //        }
    //        else if (authors != null)
    //        {
    //            errors.Add("Expected authors to be a JSON array");
    //        }

    //        return errors;
    //    }

    //    private ModelErrorDictionary ValidateDate(JToken date)
    //    {
    //        var errors = new ModelErrorDictionary();

    //        if (date is JObject dateJObject)
    //        {
    //            var (year, yearErrors) = GetValidInteger(dateJObject.SelectToken("year"), 1900, DateTime.UtcNow.Year);
    //            errors.Add(yearErrors, "Year");

    //            if (!yearErrors.HasErrors)
    //            {
    //                var (month, monthErrors) = GetValidInteger(dateJObject.SelectToken("month"), 1, 12);
    //                errors.Add(monthErrors, "Month");

    //                if (!monthErrors.HasErrors)
    //                {
    //                    var daysInMonth = DateTime.DaysInMonth(year, month);
    //                    var (day, dayErrors) = GetValidInteger(dateJObject.SelectToken("day"), 1, daysInMonth);
    //                    errors.Add(dayErrors, "Day");
    //                }
    //            }
    //        }
    //        else if (date != null)
    //        {
    //            errors.Add("Expected value to be a JSON object");
    //        }

    //        return errors;
    //    }

    //    private ModelErrorDictionary ValidateTitle(JToken jToken)
    //    {
    //        return new ModelErrorDictionary();
    //    }

    //    private ModelErrorDictionary ValidateEdition(JToken jToken)
    //    {
    //        return new ModelErrorDictionary();
    //    }

    //    private ModelErrorDictionary ValidatePages(JToken jToken)
    //    {
    //        return new ModelErrorDictionary();
    //    }

    //    private ModelErrorDictionary ValidateDoi(JToken jToken)
    //    {
    //        return new ModelErrorDictionary();
    //    }

    //    private ModelErrorDictionary ValidateOrganisation(JToken jToken)
    //    {
    //        return new ModelErrorDictionary();
    //    }

    //    private (int value, ModelErrorDictionary errors) GetValidInteger(JToken value, int minimumValue, int maximumValue)
    //    {
    //        var result = default(int);
    //        var errors = new ModelErrorDictionary();

    //        if (value is JValue jValue)
    //        {
    //            if (jValue.Type == JTokenType.Integer)
    //            {
    //                var intValue = jValue.ToObject<int>();
    //                if (intValue >= minimumValue && intValue <= maximumValue)
    //                {
    //                    result = intValue;
    //                }
    //                else
    //                {
    //                    errors.Add($"Expected value to be an interger, between {minimumValue} and {maximumValue}");
    //                }
    //            }
    //            else
    //            {
    //                errors.Add("Expected value to be an integer");
    //            }
    //        }
    //        else if (value != null)
    //        {
    //            errors.Add("Expected value to be a JSON object");
    //        }

    //        return (result, errors);
    //    }
    //}
}
