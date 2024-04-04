using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Checkins.DoCheckin;
using PassIn.Communication.Responses;

namespace PassIn.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckInController : ControllerBase
    {
        [HttpPost]
        [Route("{attendeeId}")]
       [ProducesResponseType(typeof(ResponseRegisteredJson), StatusCodes.Status201Created)]
       [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
       [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
        public IActionResult Checkin([FromRoute] Guid attendeeId)
        {
            var useCase = new DoAttendeeCheckinUseCase();

            var response = useCase.Execute(attendeeId);
            
            return Created();

        }
    }
}