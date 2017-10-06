using Nulobe.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources
{
    public class SourceValidationResult
    {
        public bool IsValid { get; set; }

        public ModelErrorDictionary ModelErrors { get; set; }

        public static SourceValidationResult Valid() => new SourceValidationResult()
        {
            IsValid = true,
            ModelErrors = new ModelErrorDictionary()
        };

        public static SourceValidationResult Invalid(string error) => new SourceValidationResult()
        {
            IsValid = false,
            ModelErrors = new ModelErrorDictionary(error)
        };

        public static SourceValidationResult Invalid(string error, string member) => new SourceValidationResult()
        {
            IsValid = false,
            ModelErrors = new ModelErrorDictionary(error, member)
        };

        public static SourceValidationResult Invalid(ModelErrorDictionary modelErrors) => new SourceValidationResult()
        {
            IsValid = false,
            ModelErrors = modelErrors
        };
    }
}
