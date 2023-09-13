using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args); // Criando um construtor de aplicação web
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]); // Adicionando o serviço de contexto do banco de dados

var app = builder.Build();  // Construindo a aplicação
var configuration = app.Configuration; // Obtendo a configuração da aplicação
ProductRepository.Init(configuration); // Inicializando o repositório de produtos com base na configuração

// Rota para criar um novo produto usando um método POST
app.MapPost("/products", (ProductRequest productRequest, ApplicationDbContext context) =>
{
    var category = context.Categories.Where(c => c.Id == productRequest.CategoryId).First(); // Recupera a categoria do banco de dados com base no ID fornecido na solicitação
    // Cria um novo objeto de produto com base nos dados da solicitação
    var product = new Product {
        Code = productRequest.Code,
        Name = productRequest.Name,
        Description = productRequest.Description,
        Category = category
    };
    if(productRequest.Tags != null) // Verifica se a solicitação inclui tags
    {
        product.Tags = new List<Tag>(); // Inicializa uma lista vazia de tags para o produto
        foreach (var item in productRequest.Tags) // Itera sobre as tags fornecidas na solicitação e cria objetos de tag para cada uma
        {
            product.Tags.Add(new Tag{ Name = item });
        }
    }
    context.Products.Add(product); // add o produto ao contexto do BD
    context.SaveChanges();
    return Results.Created($"/products/{product.Id}", product.Id); // Retorna uma resposta indicando a criação bem-sucedida do produto, incluindo o URL do novo produto e seu ID
});

//api.app.com/user/{code}  // Rota para obter um produto por código usando um método GET
app.MapGet("/products/{code}", ([FromRoute] string code) =>
{
    var product = ProductRepository.GetBy(code);
    if (product != null)
        return Results.Ok(product);
    return Results.NotFound();
});

// Rota para atualizar um produto usando um método PUT
app.MapPut("/products", (Product product) =>
{
    var productSaved = ProductRepository.GetBy(product.Code);
    productSaved.Name = product.Name;
    return Results.Ok();
});

// Rota para excluir um produto por código usando um método DELETE
app.MapDelete("/products/{code}", ([FromRoute] string code) =>
{
    var productSaved = ProductRepository.GetBy(code);
    ProductRepository.Remove(productSaved);
    return Results.Ok();
});

// Verificando se a aplicação está no ambiente de preparação (staging)
if (app.Environment.IsStaging())
    app.MapGet("/configuration/database", (IConfiguration configuration) =>
    {
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
