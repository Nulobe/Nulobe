using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Framework
{
    public class QuizletOptions : IAuthOptions
    {
        public string Domain { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}
