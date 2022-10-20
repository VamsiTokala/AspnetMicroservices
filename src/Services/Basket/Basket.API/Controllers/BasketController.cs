using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    /*v1/[controller] havingcontroller in [], it is going to get the catalog controller and remove controller and give only catalog
*output is "api/v1/Catalog
 */
    [ApiController]
    [Route("api/v1/[controller]")]



    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;

        public BasketController(IBasketRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {

        }

    }
}
