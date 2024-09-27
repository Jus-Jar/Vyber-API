using System;
using Microsoft.AspNetCore.Mvc;

namespace vyber_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Health : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok(new { status = "The Web Service Wukkin!!!!!" });
        }
    }
}
