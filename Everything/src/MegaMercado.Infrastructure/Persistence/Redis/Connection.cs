using System.Text.Json;
using MegaMercado.Application.Common;
using MegaMercado.Domain.ShoppingCart;
using StackExchange.Redis;

namespace MegaMercado.Infrastructure.Persistence.Redis;

public class RedisConnection
{
    public ConnectionMultiplexer Multiplexer { get; }

    public RedisConnection(string redisConnectionString)
    {
        Multiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
    }
    
    public bool HasKey(string key)
    {
        var db = Multiplexer.GetDatabase();
        return db.KeyExists(key);
    }

    
    public void SaveObject<T>(string key, T obj)
    {
        var db = Multiplexer.GetDatabase();
        db.StringSet(key, JsonSerializer.Serialize(obj));
    }

    
    public T? LoadObject<T>(string key)
    {
        var db = Multiplexer.GetDatabase();
        var json = db.StringGet(key);

        if (json.HasValue)
        {
            return JsonSerializer.Deserialize<T>(json.ToString());
        }

        return default;
    }
}

public class ShoppingCartService : IShoppingCartService
{
    private readonly RedisConnection _redisConnection;

    public ShoppingCartService(RedisConnection redisConnection)
    {
        _redisConnection = redisConnection;
    }

    public Cart GetCart(Guid userId)
    {
        var cart = _redisConnection.LoadObject<Cart>(GetCartKey(userId));
        return cart ?? new Cart() { Id = userId };
    }
    
    public Cart AddItemToCart(Guid userId, LineItem lineItem)
    {
        var cart = GetCart(userId);

        var existingItem = cart.Items.FirstOrDefault(x => x.ProductId == lineItem.ProductId);
        
        if (existingItem is not null)
        {
            existingItem.Quantity += lineItem.Quantity;
        }
        else
        {
            cart.Items.Add(lineItem);
        }

        _redisConnection.SaveObject(GetCartKey(userId), cart);

        return cart;
    }

    public Cart RemoveItemFromCart(Guid userId, int productid)
    {
        var cart = GetCart(userId);

        var items = cart.Items.FirstOrDefault(x => x.ProductId == productid);
        
        if (items is not null)
        {
            cart.Items.Remove(items);
        }
        
        _redisConnection.SaveObject(GetCartKey(userId), cart);

        return cart;
    }
    
    private string GetCartKey(Guid userId)
    {
        return $"cart:{userId}";
    }
}