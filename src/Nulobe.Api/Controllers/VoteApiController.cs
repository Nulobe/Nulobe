using Microsoft.AspNetCore.Mvc;
using Nulobe.Api.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nulobe.Api.Controllers
{
    [Route("votes")]
    public class VoteApiController : Controller
    {
        private IVoteEventService _voteService;

        public VoteApiController(
            IVoteEventService voteService)
        {
            _voteService = voteService;
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(Event), 200)]
        public async Task<IActionResult> Create([FromBody] VoteCreate create)
        {
            var vote = await _voteService.CreateEventAsync(create);
            return Ok(vote);
        }
    }
}
