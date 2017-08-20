using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.Query
{
    public class QueryFormatException : Exception
    {
        public QueryFormatException()
        {
        }

        public QueryFormatException(string message) : base(message)
        {
        }

        public QueryFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected QueryFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
