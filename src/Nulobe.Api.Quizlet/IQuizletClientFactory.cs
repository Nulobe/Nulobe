using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Quizlet
{
    public interface IQuizletClientFactory
    {
        QuizletClient Create();
    }
}
