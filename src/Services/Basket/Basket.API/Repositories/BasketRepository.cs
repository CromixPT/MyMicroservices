using System.Text.Json;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _cache;
    public BasketRepository(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<ShoppingCart> GetBasket(string userName)
    {
        var basket = await _cache.GetStringAsync(userName);
        if (String.IsNullOrEmpty(basket))
            return null;

        return JsonSerializer.Deserialize<ShoppingCart>(basket);
    }

    public async Task<ShoppingCart> UpdateBasketGetCart(ShoppingCart basketCart)
    {
        var value = JsonSerializer.Serialize(basketCart);
        await _cache.SetStringAsync(basketCart.Username, value);

        return await GetBasket(basketCart.Username);
    }

    public async Task DeleteBasketGetCart(string userName)
    {
        await _cache.RemoveAsync(userName);
    }
}
