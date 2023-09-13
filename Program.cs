using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); // Criando um construtor de aplicação web
builder.Services.AddDbContext<ApplicationDbContext>(); // Adicionando o serviço de contexto do banco de dados

var app = builder.Build();  // Construindo a aplicação
var configuration = app.Configuration; // Obtendo a configuração da aplicação
ProductRepository.Init(configuration); // Inicializando o repositório de produtos com base na configuração

// Rota para criar um novo produto usando um método POST
app.MapPost("/products", (Product product) => {
    ProductRepository.Add(product);
    return Results.Created($"/products/{product.Code}", product.Code);
});

//api.app.com/user/{code}  // Rota para obter um produto por código usando um método GET
app.MapGet("/products/{code}", ([FromRoute] string code) => {
    var product = ProductRepository.GetBy(code);
    if(product != null)
        return Results.Ok(product);
    return Results.NotFound();
});

// Rota para atualizar um produto usando um método PUT
app.MapPut("/products", (Product product) => {
    var productSaved = ProductRepository.GetBy(product.Code);
    productSaved.Name = product.Name;
    return Results.Ok();
});

// Rota para excluir um produto por código usando um método DELETE
app.MapDelete("/products/{code}", ([FromRoute] string code) => {
    var productSaved = ProductRepository.GetBy(code);
    ProductRepository.Remove(productSaved);
    return Results.Ok();
});

// Verificando se a aplicação está no ambiente de preparação (staging)
if(app.Environment.IsStaging())
    app.MapGet("/configuration/database", (IConfiguration configuration) => {
        return Results.Ok($"{configuration["database:connection"]}/{configuration["database:port"]}");
    });

// if(app.Environment.IsDevelopment())
//     app.MapGet("/configuration/database", (IConfiguration configuration) => {
//         return Results.Ok($"{configuration["database:connection"]}/{configuration["database:port"]}");
//     });

// if(app.Environment.IsProduction())
//     app.MapGet("/configuration/database", (IConfiguration configuration) => {
//         return Results.Ok($"{configuration["database:connection"]}/{configuration["database:port"]}");
//     });

app.Run();

// Classe de repositório de produtos
public static class ProductRepository {
    public static List<Product> Products { get; set; } = Products = new List<Product>();

    // Método de inicialização do repositório com base na configuração
    public static void Init(IConfiguration configuration) {
        var products = configuration.GetSection("Products").Get<List<Product>>();
        Products = products;
    }

    public static void Add(Product product) {
        Products.Add(product);
    }

    // Método para obter um produto por código
    public static Product GetBy(string code) {
        return Products.FirstOrDefault(p => p.Code == code);
    }

    public static void Remove(Product product) {
        Products.Remove(product);
    }
}

public class Category {
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Tag {
    public int Id { get; set; }
    public string Name { get; set; }
}

// Classe de modelo de produto
public class Product {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public List<Tag> Tags { get; set; }
}

// Classe de contexto de banco de dados
public class ApplicationDbContext : DbContext {

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) 
    {
        builder.Entity<Product>()
            .Property(p => p.Description).HasMaxLength(500).IsRequired(false);
        builder.Entity<Product>()
            .Property(p => p.Name).HasMaxLength(120).IsRequired();
        builder.Entity<Product>()
            .Property(p => p.Code).HasMaxLength(20).IsRequired();
    }

    // Configurando o contexto do banco de dados
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=localhost;Database=Products;User Id=sa;Password=@Sql2023;MultipleActiveResultSets=true;Encrypt=YES;TrustServerCertificate=YES");
}