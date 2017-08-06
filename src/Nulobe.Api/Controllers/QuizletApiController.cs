using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nulobe.Api.Core;
using Nulobe.Api.Core.Facts;
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
        private readonly IQuizletTokenService _quizletTokenClient;
        private readonly IQuizletSetService _quizletSetService;

        public QuizletApiController(
            IQuizletTokenService quizletTokenClient,
            IQuizletSetService quizletSetService)
        {
            _quizletTokenClient = quizletTokenClient;
            _quizletSetService = quizletSetService;
        }

        [HttpPost("token")]
        [ProducesResponseType(typeof(QuizletTokenResponse), 200)]
        public async Task<IActionResult> Token([FromBody] QuizletTokenRequest request)
            => Ok(await _quizletTokenClient.GetTokenAsync(request));

        [Route("sets")]
        [ProducesResponseType(typeof(QuizletSet), 201)]
        public async Task<IActionResult> CreateSetAsync([FromBody] FactQuery query)
        {
            var set = await _quizletSetService.CreateSetAsync(query);
            return new CreatedResult(set.Url, set);
        }
    }
}
