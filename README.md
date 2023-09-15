# api

O objetivo deste programa é criar uma aplicação web usando o framework ASP.NET Core e o Entity Framework Core para realizar operações CRUD (Create, Read, Update, Delete) em produtos, bem como fornecer informações sobre a configuração do banco de dados quando a aplicação está em um ambiente de preparação (staging).

Aqui está uma explicação das principais funcionalidades deste programa:

Configuração do Banco de Dados:

O programa configura a conexão com o banco de dados SQL Server com base nas informações fornecidas na configuração da aplicação.
Rotas para Manipulação de Produtos:

Este programa define várias rotas HTTP para manipular produtos:
`POST /products`: Cria um novo produto com base nos dados fornecidos na solicitação `POST`. Isso inclui a criação de uma nova categoria (se não existir) e tags associadas ao produto.
`GET /products/{id}`: Recupera um produto existente por seu ID e retorna os detalhes do produto.
`PUT /products/{id}`: Atualiza um produto existente com base nos dados fornecidos na solicitação PUT.
`DELETE /products/{id}`: Exclui um produto com base no ID fornecido na solicitação DELETE.

Recuperação de Configuração de Banco de Dados:

Quando a aplicação está em um ambiente de preparação (staging), ele fornece informações sobre a configuração do banco de dados, como a conexão e a porta, quando a rota GET /configuration/database é acessada.

Gestão de Categoria e Tags:

A criação de um novo produto envolve a verificação da existência da categoria do produto no banco de dados. Se não existir, a categoria será criada. O mesmo ocorre com as tags associadas ao produto.

Controle de Ambiente:

O programa verifica o ambiente em que está sendo executado (desenvolvimento, preparação, produção) usando `app.Environment.IsStaging()` e define a rota para recuperar informações de configuração do banco de dados apenas no ambiente de preparação.
Este programa é uma aplicação de exemplo que demonstra como criar uma API web para gerenciar produtos com ASP.NET Core e Entity Framework Core, além de como acessar informações de configuração dependendo do ambiente em que a aplicação está sendo executada. É uma base sólida para construir uma aplicação completa de gerenciamento de produtos.
