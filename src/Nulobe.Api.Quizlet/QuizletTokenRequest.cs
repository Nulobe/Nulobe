using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Quizlet
{
    public class QuizletTokenRequest
    {
        /// <summary>
        /// The authorization code that was provided by the quizlet auth service
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The client-defined redirect URI, which was used in retrieving the authorization code
        /// </summary>
        public Uri RedirectUri { get; set; } 
    }
}
