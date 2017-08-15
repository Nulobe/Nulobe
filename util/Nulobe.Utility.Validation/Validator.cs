using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemValidator = System.ComponentModel.DataAnnotations.Validator;

namespace Nulobe.Utility.Validation
{
    public static class Validator
    {


        public static (bool isValid, ModelErrorDictionary modelErrors) IsValid(
           object model,
           IServiceProvider serviceProvider = null,
           IDictionary<object, object> items = null)
        {
            var validationContext = new ValidationContext(model, serviceProvider, items);
            var validationResults = new List<ValidationResult>();

            var isValid = SystemValidator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);
            var (resultsWithMember, resultsWithoutMember) = validationResults.Fork(v => v.MemberNames.Count() > 0);

            return (isValid, new ModelErrorDictionary(
                resultsWithoutMember.Select(v => v.ErrorMessage),
                resultsWithMember
                    .SelectMany(v => v.MemberNames.Select(m => (MemberName: m, ErrorMessage: v.ErrorMessage)))
                    .ToLookup(t => t.MemberName)
                    .ToDictionary(g => g.Key, g => g.Select(t => t.ErrorMessage))));
        }

        public static void Validate(object model, IServiceProvider serviceProvider = null, IDictionary<object, object> items = null)
        {
            var (isValid, modelErrors) = IsValid(model, serviceProvider, items);
            if (!isValid)
            {
                throw new ClientModelValidationException(modelErrors);
            }
        }

        public static void ValidateNotNull(object model, string paramName)
        {
            if (model == null)
            {
                throw new ClientArgumentNullException(paramName);
            }
        }

        public static void ValidateStringNotNullOrEmpty(string str, string paramName)
        {
            if (str == null)
            {
                throw new ClientArgumentNullException(paramName);
            }
            else if (str == string.Empty)
            {
                throw new ClientArgumentException(paramName);
            }
        }
    }
}
