using Basket.API.Entities;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userName);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);//fatching basekt info from shoppingcart object
        Task DeleteBasket(string userName); //user name will be key of our key value structure in dictionary database
    }
}
