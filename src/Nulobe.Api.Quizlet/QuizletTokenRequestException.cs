using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Quizlet
{
    public class QuizletTokenRequestException : Exception
    {
        public QuizletTokenError Error { get; private set; }

        public QuizletTokenRequestException(QuizletTokenError error) : base(error.ErrorDescription)
        {
            Error = error;
        }
    }
}
