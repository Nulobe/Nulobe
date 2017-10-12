using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.Validation
{
    public static class ModelErrorDictionaryExtensions
    {
        public static void AddRequired(this ModelErrorDictionary modelErrorDictionary, string memberName)
            => modelErrorDictionary.Add($"{memberName} is required", memberName);

        public static void AddOutOfRange(this ModelErrorDictionary modelErrorDictionary, string memberName)
            => modelErrorDictionary.Add($"{memberName} was out of range of the allowed values", memberName);

        public static void AddStringExpected(this ModelErrorDictionary modelErrorDictionary, string memberName)
            => modelErrorDictionary.AddTypeExpected(memberName, "string");

        public static void AddIntegerExpected(this ModelErrorDictionary modelErrorDictionary, string memberName)
            => modelErrorDictionary.AddTypeExpected(memberName, "integer");

        public static void AddUriExpected(this ModelErrorDictionary modelErrorDictionary, string memberName)
            => modelErrorDictionary.AddTypeExpected(memberName, "uri");

        public static void AddTypeExpected(this ModelErrorDictionary modelErrorDictionary, string memberName, string typeDescriptor)
            => modelErrorDictionary.Add($"Expected {memberName} to be of type ${typeDescriptor}", memberName);
    }
}
