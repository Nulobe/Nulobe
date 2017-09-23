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
            IsValid = true
        };

        public static SourceValidationResult Invalid(string member, string error) => new SourceValidationResult()
        {
            IsValid = false,
            ModelErrors = new ModelErrorDictionary(member, error)
        };

        public static SourceValidationResult Invalid(ModelErrorDictionary modelErrors) => new SourceValidationResult()
        {
            IsValid = false,
            ModelErrors = modelErrors
        };
    }
}
