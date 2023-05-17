// See https://aka.ms/new-console-template for more information

// uses https://www.kaggle.com/datasets
// https://www.kaggle.com/datasets/surajjha101/bigbasket-entire-product-list-28k-datapoints


using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using MegaMercado.DatasetImport.EF;

var config = new CsvConfiguration(CultureInfo.InvariantCulture)
{
    HasHeaderRecord = true
};
using var reader = new StreamReader(@".\BigBasket Products.csv");
using var csv = new CsvReader(reader, config);

var records = csv.GetRecords<Record>().ToList();

Console.WriteLine($"got {records.Count()}");

//int categoryId = 1;
//int productId = 1;
//int brandId = 1;

var categories = new List<Category>();
var brands = new List<Brand>();
var products = new List<Product>();

using var cx = new LearningContext();

foreach (var record in records)
{
    var parentCategory =
        categories.FirstOrDefault(x => x.Description.Equals(record.Category, StringComparison.OrdinalIgnoreCase));

    if (parentCategory == null)
    {
        parentCategory = new Category
        {
            Description = record.Category
        };
        categories.Add(parentCategory);
        cx.Categories.Add(parentCategory);
    }

    var subCategory =
        categories.FirstOrDefault(x => x.Description.Equals(record.SubCategory, StringComparison.OrdinalIgnoreCase));

    if (subCategory == null)
    {
        if (parentCategory == null) throw new Exception("Something is out of line");

        subCategory = new Category
        {
            Description = record.SubCategory
        };

        parentCategory.SubCategories.Add(subCategory);
        categories.Add(subCategory);
        cx.Categories.Add(subCategory);
    }

    var brand = brands.FirstOrDefault(x => x.Name.Equals(record.Brand, StringComparison.OrdinalIgnoreCase));

    if (brand == null)
    {
        brand = new Brand { Name = record.Brand };
        brands.Add(brand);
    }


    var product = new Product
    {
        Brand = brand,
        Categories = new List<Category> { parentCategory, subCategory },
        Description = record.Description,
        Msrp = record.MarketPrice,
        Price = record.SalePrice,
        Name = record.Product,
        Rating = string.IsNullOrEmpty(record.Rating) ? 0 : decimal.Parse(record.Rating),
        Type = record.Type
    };

    products.Add(product);
    cx.Products.Add(product);
}

cx.SaveChanges();


Console.ReadLine();


public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Product> Products { get; set; } = new();
}


public class Record
{
    [Index(0)] public int Index { get; set; }

    [Index(1)] public string Product { get; set; }

    [Index(2)] public string Category { get; set; }

    [Index(3)] public string SubCategory { get; set; }

    [Index(4)] public string Brand { get; set; }

    [Index(5)] public decimal SalePrice { get; set; }

    [Index(6)] public decimal MarketPrice { get; set; }

    [Index(7)] public string Type { get; set; }

    [Index(8)] public string Rating { get; set; }

    [Index(9)] public string Description { get; set; }
}

public class Category
{
    public int Id { get; set; }
    public int? ParentCategoryId { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<Category> SubCategories { get; set; } = new();
    public List<Product> Products { get; set; } = new();
    public Category ParentCategory { get; set; }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Category> Categories { get; set; } = new();

    public int BrandId { get; set; }
    public Brand Brand { get; set; }
    public decimal Price { get; set; }
    public decimal Msrp { get; set; }
    public string Type { get; set; }
    public decimal Rating { get; set; }
    public string Description { get; set; }
}