using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nulobe.Api.Core;
using Nulobe.Api.Core.Facts;

namespace Nulobe.Api.Controllers
{
    [Route("facts")]
    public class FactApiController : Controller
    {
        private readonly IFactService _factService;
        private readonly IFactQueryService _factQueryService;

        public FactApiController(
            IFactService factService,
            IFactQueryService factQueryService)
        {
            _factService = factService;
            _factQueryService = factQueryService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(Fact[]), 200)]
        public async Task<IActionResult> List([FromQuery] FactQuery query)
            => Ok(await _factQueryService.QueryFactsAsync(query));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Fact), 200)]
        public async Task<IActionResult> Get(string id)
            => Ok(await _factService.GetFactAsync(id));

        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(typeof(FactCreate), 201)]
        public async Task<IActionResult> Create([FromBody] FactCreate create)
        {
            var result = await _factService.CreateFactAsync(create);
            return CreatedAtAction(
                nameof(Get),
                new { id = result.Id },
                result);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Fact), 200)]
        public async Task<IActionResult> Update(string id, [FromBody] Fact fact)
            => Ok(await _factService.UpdateFactAsync(id, fact));

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string id)
        {
            await _factService.DeleteFactAsync(id);
            return NoContent();
        }
    }
}
