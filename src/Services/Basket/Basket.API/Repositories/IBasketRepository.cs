using Basket.API.Entities;

namespace Basket.API.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string userName);
    Task<ShoppingCart> UpdateBasketGetCart(ShoppingCart basketCart);    
    Task DeleteBasketGetCart(string userName);
}
