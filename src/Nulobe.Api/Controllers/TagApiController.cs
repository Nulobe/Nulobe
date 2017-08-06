using Microsoft.AspNetCore.Mvc;
using Nulobe.Api.Core;
using Nulobe.Api.Core.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nulobe.Api.Controllers
{
    [Route("tags")]
    public class TagApiController : Controller
    {
        private readonly ITagQueryService _tagQueryService;

        public TagApiController(
            ITagQueryService tagQueryService)
        {
            _tagQueryService = tagQueryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Tag[]), 200)]
        public async Task<IActionResult> List([FromQuery] TagQuery query)
            => Ok(await _tagQueryService.QueryTagsAsync(query));
    }
}
