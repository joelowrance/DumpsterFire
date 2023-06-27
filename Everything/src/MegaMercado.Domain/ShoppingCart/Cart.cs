namespace MegaMercado.Domain.ShoppingCart;

public class Cart
{
    public required string Id { get; set;} = string.Empty!;
    public List<LineItem> Items { get; set; } = new();
}

public record LineItem(int ProductId, decimal Msrp, decimal Price)
{
    public int Quantity { get; set; } 
}
