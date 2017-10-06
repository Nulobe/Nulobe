using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nulobe.Api.Core.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nulobe.Api.Controllers
{
    /// <summary>
    /// Used to include types which are not explicitly included by other controllers.
    /// </summary>
    [Route("__dummy")]
    [Authorize]
    public class SwaggerGenerationDummyController : Controller
    {
        [ProducesResponseType(typeof(SourceType), 200)]
        [ProducesResponseType(typeof(ApaSourceType), 200)]
        public IActionResult DummyEndpoint()
            => NotFound();
    }
}
