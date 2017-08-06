using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Quizlet
{
    public class QuizletSet
    {
        public int Id { get; set; }

        public Uri Url { get; set; }

        public IEnumerable<QuizletTerm> Terms { get; set; }
    }
}
