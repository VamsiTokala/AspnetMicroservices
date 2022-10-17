using Catalog.API.Entities;
using Catalog.API.Repositories;
using DnsClient.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

/*After developing the controller, the most important part is registring objects into the asp.ner dependecny injection tool
 * here, controller class - we use repository object and repository object uses ICatalogContext. that means we have to register
 * these objects into the asp.net dependecy injection
 * Go to startup.cs - ConfigureServices
 * add -
 * services.AddScoped<ICatalogContext,CatalogContext >()
 * services.AddScoped<IProductRepository,ProductRepository >()
 * basically thee lines of code, when asp.net code sees ICatalogContext, it will dynamically create a CatalogContext using
 * builtin dependency injection
 * 
 * Finallylater - update swagger packer
 * Update-Package -ProjectName Catalog.api
 * */

namespace Catalog.API.Controllers
{
    //presentation layer

    /* we have to inject iproduct repository into the controller class for exposing our apis to the outside world
     * for catalog micro service.  We need conver controller to API controler and there are some attirbautes
     *  [ApiController] uses microsoft aspnetcore mvc namespace
     */
    [ApiController]

    /*v1/[controller] havingcontroller in [], it is going to get the catalog controller and remove controller and give only catalog
    *output is "api/v1/Catalog
     */

    [Route("api/v1/[controller]")]

    //it should inherit from Controllerbase in order to use api realted objects inside of our methods
    public class CatalogController : ControllerBase
    {
        //inject our repository
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        //after defining, generate constuctor and inject these
        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        //now we have arepository and local object we can use in these members in our api methods

        [HttpGet]
        /*in order to make the method resiliance we have to go ActionResult
        **learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-6.0
        * public async Task<IEnumerable<Product>> GetProducts() -->Task<ActionResult<IEnumerable<Product>>>
        * 
        */

        /*below attribute definition we have to restricted that getproducts api method will be returned with OK response
        , so when you call this line, you call this method ok, the first get method is ready for*/ 
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetProducts();
            return Ok(products);

        }
        // three attributes

        [HttpGet("{id:length(24)}", Name = "GetProduct")] // we are expecting id as parameter. 24 is bison id restrition in mongo db
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _repository.GetProduct(id);
            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }
            return Ok(product);
        }
        /*changing the route defintion
         * [action] is dynamic parameter, action name as method name i.e GetProductByCategory
         * {category} is a parameter which we are proving cetagory namefrom th querystring of the uri
         * first uri is api/v1/Catalog 
         * url for this will be  GetProductByCategory/categoryname
         */
        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await _repository.GetProductByCategory(category);
            return Ok(products);
        }

        [HttpPost]//using post
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _repository.CreateProduct(product);
            //CreatedAtRoute  comes from controlerbase class, routename is GetProduct, calling nextapi methosd
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _repository.UpdateProduct(product));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _repository.DeleteProduct(id));
        }

    }
}
