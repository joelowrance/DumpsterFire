using System.Text.Json;
using MegaMercado.Application.Common;
using MegaMercado.Domain.ShoppingCart;
using StackExchange.Redis;

namespace MegaMercado.Infrastructure.Persistence.Redis;

public class RedisConnection
{
    private ConnectionMultiplexer Multiplexer { get; }

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

    public Cart GetCart(string emailAddress)
    {
        var cart = _redisConnection.LoadObject<Cart>(GetCartKey(emailAddress));
        return cart ?? new Cart { Id = emailAddress };
    }
    
    public Cart AddItemToCart(string emailAddress, LineItem lineItem)
    {
        var cart = GetCart(emailAddress);

        var existingItem = cart.Items.FirstOrDefault(x => x.ProductId == lineItem.ProductId);
        
        if (existingItem is not null)
        {
            existingItem.Quantity += lineItem.Quantity;
        }
        else
        {
            cart.Items.Add(lineItem);
        }

        _redisConnection.SaveObject(GetCartKey(emailAddress), cart);

        return cart;
    }

    public Cart RemoveItemFromCart(string emailAddress, int productid)
    {
        var cart = GetCart(emailAddress);

        var items = cart.Items.FirstOrDefault(x => x.ProductId == productid);
        
        if (items is not null)
        {
            cart.Items.Remove(items);
        }
        
        _redisConnection.SaveObject(GetCartKey(emailAddress), cart);

        return cart;
    }
    
    private string GetCartKey(string emailAddress)
    {
        return $"cart:{emailAddress}";
    }
}