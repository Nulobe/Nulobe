using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nulobe.Api.Quizlet;
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
        private readonly IQuizletTokenClient _quizletTokenClient;

        public QuizletApiController(
            IQuizletTokenClient quizletTokenClient)
        {
            _quizletTokenClient = quizletTokenClient;
        }

        [HttpPost("token")]
        [ProducesResponseType(typeof(QuizletTokenResponse), 200)]
        public async Task<IActionResult> Token(QuizletTokenRequest request)
            => Ok(await _quizletTokenClient.GetTokenAsync(request));

        [Route("sets")]
        public Task<IActionResult> CreateSet()
        {
            throw new NotImplementedException();
        }
    }
}
