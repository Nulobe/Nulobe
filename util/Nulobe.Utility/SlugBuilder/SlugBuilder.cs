using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.SlugBuilder
{
    public class SlugBuilder : ISlugBuilder
    {
        private readonly int maxLength = 80;

        private StringBuilder innerBuilder = new StringBuilder();

        public SlugBuilder(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public ISlugBuilder Add(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                this.innerBuilder.Append(" ");
                this.innerBuilder.Append(input);
            }
            return this;
        }

        public ISlugBuilder Add(string format, params object[] args)
        {
            if (!string.IsNullOrEmpty(format))
            {
                this.innerBuilder.Append(" ");
                this.innerBuilder.Append(string.Format(format, args));
            }
            return this;
        }

        public ISlugBuilder Add(int input)
        {
            this.innerBuilder.Append(" ");
            this.innerBuilder.Append(input);
            return this;
        }

        public ISlugBuilder Add(long input)
        {
            this.innerBuilder.Append(" ");
            this.innerBuilder.Append(input);
            return this;
        }

        public ISlugBuilder Add(Guid input)
        {
            this.innerBuilder.Append(" ");
            this.innerBuilder.Append(input);
            return this;
        }

        public override string ToString()
        {
            return Create(this.innerBuilder.ToString(), this.maxLength);
        }

        #region [ Helpers ]

        public static string Create(string input, int maxLength = 80)
        {
            if (input == null) return string.Empty;

            int len = input.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = input[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxLength) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }

        private static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåąā".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'æ')
            {
                return "ae";
            }
            else if (c == 'œ')
            {
                return "oe";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }

            return string.Empty;
        }

        #endregion
    }
}
