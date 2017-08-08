using Microsoft.AspNetCore.Mvc;
using Nulobe.Api.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nulobe.Api.Controllers
{
    [Route("flags")]
    public class FlagApiController : Controller
    {
        private IFlagEventService _flagService;

        public FlagApiController(
            IFlagEventService flagService)
        {
            _flagService = flagService;
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(Event), 200)]
        public async Task<IActionResult> Create([FromBody] FlagCreate create)
        {
            var flag = await _flagService.CreateEventAsync(create);
            return Ok(flag);
        }
    }
}
