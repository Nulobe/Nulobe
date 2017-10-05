using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.Validation
{
    public class ModelErrorDictionary : Dictionary<string, IEnumerable<string>>
    {
        public IEnumerable<string> NonMemberErrors { get; private set; }

        public bool HasErrors => NonMemberErrors.Any() || this.SelectMany(kvp => kvp.Value).Any();

        public ModelErrorDictionary(string error)
            : this(new Dictionary<string, IEnumerable<string>>(), new string[] { error }) { }

        public ModelErrorDictionary(string error, string member)
            : this(new Dictionary<string, IEnumerable<string>>() { { member, new string[] { error } } }) { }

        public ModelErrorDictionary(
            Dictionary<string, IEnumerable<string>> errorsByMember,
            IEnumerable<string> modelErrors = null) : base(errorsByMember)
        {
            NonMemberErrors = modelErrors ?? Enumerable.Empty<string>();
        }

        public ModelErrorDictionary()
        {
            NonMemberErrors = Enumerable.Empty<string>();
        }

        public bool MemberHasErrors(string memberName)
        {
            return !TryGetValue(memberName, out IEnumerable<string> errors) || !errors.Any();
        }

        public void Add(string error, string memberName = null) => Add(error, memberName, false);

        public void Add(string error, int index) => Add(error, index.ToString(), true);

        public void Add(ModelErrorDictionary other, string memberName = null) => Add(other, memberName, false);

        public void Add(ModelErrorDictionary other, int index) => Add(other, index.ToString(), true);


        #region Helpers

        private void Add(string error, string memberName, bool isIndex)
        {
            memberName = ResolveMemberName(memberName, isIndex);

            if (string.IsNullOrEmpty(memberName))
            {
                NonMemberErrors = NonMemberErrors.Concat(new string[] { error });
            }
            else
            {
                TryGetValue(memberName, out IEnumerable<string> memberErrors);
                this[memberName] = (memberErrors ?? Enumerable.Empty<string>()).Concat(new string[] { error });
            }
        }

        private void Add(ModelErrorDictionary other, string memberName, bool isIndex)
        {
            memberName = ResolveMemberName(memberName, isIndex);

            if (other.NonMemberErrors.Any())
            {
                if (string.IsNullOrEmpty(memberName))
                {
                    NonMemberErrors = NonMemberErrors.Concat(other.NonMemberErrors);
                }
                else
                {
                    if (!TryGetValue(memberName, out IEnumerable<string> memberErrors))
                    {
                        memberErrors = Enumerable.Empty<string>();
                    }
                    this[memberName] = memberErrors.Concat(other.NonMemberErrors);
                }
            }

            if (other.SelectMany(kvp => kvp.Value).Any())
            {
                foreach (var kvp in other)
                {
                    if (string.IsNullOrEmpty(memberName))
                    {
                        this[kvp.Key] = kvp.Value;
                    }
                    else
                    {
                        var memberPath = memberName;
                        if (!IsMemberPathPartIndex(kvp.Key.Split('.').First()))
                        {
                            memberPath += ".";
                        }
                        memberPath += kvp.Key;

                        this[memberPath] = kvp.Value;
                    }
                }
            }
        }

        private string ResolveMemberName(string memberName, bool isIndex) => !string.IsNullOrEmpty(memberName) && isIndex ? "[" + memberName + "]" : memberName;

        private bool IsMemberPathPartIndex(string memberPathPart) => memberPathPart.StartsWith("[") && memberPathPart.EndsWith("]");

        #endregion
    }
}
