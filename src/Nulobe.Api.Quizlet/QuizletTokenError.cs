using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Quizlet
{
    public class QuizletTokenError
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_title")]
        public string ErrorTitle { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}
