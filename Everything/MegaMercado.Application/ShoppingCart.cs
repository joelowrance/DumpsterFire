
namespace MegaMercado.Application;



public class ShoppingCart
{
    public Guid UserId { get; set; }

    private Dictionary<int, int> _items = new();

    public void AddProductToCart(int productId, int quantity)
    {
        if (!_items.ContainsKey(productId))
        {
            _items[productId] += quantity;
        }
        else
        {
            _items.Add(productId, quantity);
        }
    }

    public void RemoveProduct(int productId)
    {
        _items.Remove(productId);
    }
    
    
    public void SetQuantity(int productId, int quantity)
    {
        if (!_items.ContainsKey(productId)) return;
        _items[productId] = quantity;

        if (_items[productId] <= 0)
        {
            RemoveProduct(productId);
        }
    }

    public int ItemCount => _items.Count;

    public IEnumerable<KeyValuePair<int, int>> BasketSummary =>
        _items.Select(x => new KeyValuePair<int, int>(x.Key, x.Value));
}


