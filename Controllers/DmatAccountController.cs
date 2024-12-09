using Microsoft.AspNetCore.Mvc;
using DmatAccountApi.Models;

namespace DmatAccountApi.Controllers
{
    [ApiController]
    [Route("api/dmat")]
    public class DmatAccountController : ControllerBase
    {
        [HttpPost("create")]
        public IActionResult CreateDmatAccount([FromBody] DmatAccountRequest request)
        {
            return Ok(new
            {
                RequestTime = System.DateTime.Now,
                Status = "Success",
                Message = "DMAT Account created successfully."
            });
        }
    }
}
