namespace MegaMercado.Domain;

public class DomainEntity 
{
    public int Id { get; set; }
}

public class Customer : DomainEntity
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
}

public class ShoppingCart : DomainEntity
{
    public int CustomerId { get; set; }
    public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.Now;
    
    //make this readonly so the cart is the aggregate
    public List<LineItem> LineItems { get; set; } = new List<LineItem>();
}


//what happens if the price of an item changes
public class LineItem
{
    public int ProductId { get; set; }
    public DateTimeOffset DateCreated { get; set; }
}

public class Product : DomainEntity
{
    public string Sku { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal? Msrp     { get; set; }
    public decimal? ListPrice { get; set; }
}