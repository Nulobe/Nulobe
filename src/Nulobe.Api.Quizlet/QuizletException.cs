using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Quizlet
{
    public class QuizletException : Exception
    {
        public QuizletError Error { get; private set; }

        public QuizletException(QuizletError error) : base(error.ErrorDescription)
        {
            Error = error;
        }
    }
}
