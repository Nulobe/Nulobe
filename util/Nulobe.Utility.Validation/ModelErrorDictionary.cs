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

        public ModelErrorDictionary(string member, string error)
            : this(new Dictionary<string, IEnumerable<string>>() { { member, new string[] { error } } }) { }

        public ModelErrorDictionary(
            Dictionary<string, IEnumerable<string>> errorsByMember,
            IEnumerable<string> modelErrors = null) : base(errorsByMember)
        {
            Errors = errorsByMember.SelectMany(kvp => kvp.Value);

            if (modelErrors != null)
            {
                Errors = Errors.Concat(modelErrors);
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
