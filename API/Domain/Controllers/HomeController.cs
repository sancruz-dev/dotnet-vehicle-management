using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using minimal_api.Domain.ModelViews;

namespace minimal_api.Domain.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet("/")]
        [AllowAnonymous]
        public IActionResult GetHome()
        {
            return Ok(new Home());
        }
    }
}