using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.Validation
{
    public class ModelErrorDictionary : Dictionary<string, IEnumerable<string>>
    {
        public IEnumerable<string> Errors { get; private set; }

        public ModelErrorDictionary(
            IEnumerable<string> modelErrors = null,
            Dictionary<string, IEnumerable<string>> errorsByMember = null) : base(errorsByMember ?? new Dictionary<string, IEnumerable<string>>())
        {
            Errors = Enumerable.Empty<string>();

            if (modelErrors != null)
            {
                Errors = Errors.Concat(modelErrors);
            }

            if (errorsByMember != null)
            {
                Errors = Errors.Concat(errorsByMember.SelectMany(kvp => kvp.Value));
            }
        }

        public void Add(string error, string memberName = null)
        {
            Errors = Errors.Concat(new string[] { error });

            if (!string.IsNullOrEmpty(memberName))
            {
                TryGetValue(memberName, out IEnumerable<string> memberErrors);
                this[memberName] = (memberErrors ?? Enumerable.Empty<string>()).Concat(new string[] { error });
            }
        }

        public void Add(ModelErrorDictionary other, string memberPrefix = null)
        {
            Errors = Errors.Concat(other.Errors);
            foreach (var kvp in other)
            {
                if (string.IsNullOrEmpty(memberPrefix))
                {
                    this[memberPrefix] = kvp.Value;
                }
                else
                {
                    this[$"{memberPrefix}.{kvp.Key}"] = kvp.Value;
                }

            }
        }

        public bool IsMemberValid(string memberName)
        {
            return !TryGetValue(memberName, out IEnumerable<string> errors) || !errors.Any();
        }
    }
}
