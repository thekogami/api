
// Classe de repositório de produtos
public static class ProductRepository
{
    public static List<Product> Products { get; set; } = Products = new List<Product>();

    // Método de inicialização do repositório com base na configuração
    public static void Init(IConfiguration configuration)
    {
        var products = configuration.GetSection("Products").Get<List<Product>>();
        Products = products;
    }

    public static void Add(Product product)
    {
        Products.Add(product);
    }

    // Método para obter um produto por código
    public static Product GetBy(string code)
    {
        return Products.FirstOrDefault(p => p.Code == code);
    }

    public static void Remove(Product product)
    {
        Products.Remove(product);
    }
}
