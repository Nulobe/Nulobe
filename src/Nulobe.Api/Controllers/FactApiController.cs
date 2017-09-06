using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nulobe.Api.Core;
using Nulobe.Api.Core.Facts;
using Microsoft.Extensions.Primitives;

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
        {
            var result = await _factQueryService.QueryFactsAsync(query);
            Response.Headers.Add("X-Total-Count", result.Count.ToString());
            return Ok(result.Facts);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Fact), 200)]
        public async Task<IActionResult> Get(string id)
            => Ok(await _factService.GetFactAsync(id));

        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(typeof(Fact), 201)]
        public async Task<IActionResult> Create([FromBody] FactCreate create, [FromQuery(Name = "dryRun")] bool? dryRunNullable = null)
        {
            var dryRun = dryRunNullable ?? false;
            var result = await _factService.CreateFactAsync(create, dryRunNullable ?? false);
            if (dryRun)
            {
                return Ok(result);
            }
            else
            {
                return CreatedAtAction(
                    nameof(Get),
                    new { id = result.Id },
                    result);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Fact), 200)]
        public async Task<IActionResult> Update(string id, [FromBody] FactCreate create)
            => Ok(await _factService.UpdateFactAsync(id, create));

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
