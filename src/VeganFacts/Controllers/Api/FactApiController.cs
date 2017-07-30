using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeganFacts.Services;

namespace VeganFacts.Controllers.Api
{
    [Route("api/facts")]
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
        public async Task<IActionResult> List([FromQuery] FactQuery query)
            => Ok(await _factQueryService.QueryFactsAsync(query));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
            => Ok(await _factService.GetFactAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Fact fact)
        {
            var result = await _factService.CreateFactAsync(fact);
            return CreatedAtAction(
                nameof(Get),
                new { id = result.Id },
                result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Fact fact)
            => Ok(await _factService.UpdateFactAsync(id, fact));
    }
}
