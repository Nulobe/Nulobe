using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nulobe.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nulobe.Api.Controllers
{
    [Route("quizlet")]
    public class QuizletApiController : Controller
    {
        private readonly QuizletOptions _options;

        public QuizletApiController(
            IOptions<QuizletOptions> options)
        {
            _options = options.Value;
        }


    }
}
