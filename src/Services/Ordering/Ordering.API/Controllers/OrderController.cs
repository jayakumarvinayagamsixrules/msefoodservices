using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator?? throw new ArgumentNullException(nameof(mediator));
        }

        //public async Task<ActionResult<>> GetOrdersByUserName(string userName)
        //{

        //}
    }
}
