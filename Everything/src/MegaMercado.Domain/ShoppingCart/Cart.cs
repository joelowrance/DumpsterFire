namespace MegaMercado.Domain.ShoppingCart;

public class Cart
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public List<LineItem> Items { get; set; } = new();
}

public record LineItem(int ProductId, decimal Msrp, decimal Price)
{
    public int Quantity { get; set; } 
}
