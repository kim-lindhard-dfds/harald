using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Harald.WebApi.Controllers
{
    [Route("api/v1/messages")]
    [ApiController]
    public class DispatchController : ControllerBase
    {
        [HttpPost]
        public IActionResult DispatchMessage(DispatchMessageInput input)
        {
            return Accepted();
        }
    }

    public class DispatchMessageInput
    {
        [Required]

        public Guid? CapabilityId { get; set; }

        [Required]
        public string Message { get; set; }
    }
}