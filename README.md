# API ASP.NET Core and Entity Framework Core

The purpose of this program is to create a web application using the ASP.NET Core framework and Entity Framework Core to perform CRUD (Create, Read, Update, Delete) operations on products, as well as to provide information about the database configuration when the application is in a staging environment.

## Features

### Database Configuration:

The program configures the connection to the SQL Server database based on the information provided in the application configuration.

### Routes for Product Manipulation:

This program defines various HTTP routes to manipulate products:
 - `POST /products`: Creates a new product based on the data provided in the `POST` request. This includes creating a new category (if it does not exist) and tags associated with the product.
 - `GET /products/{id}`: Retrieves an existing product by its ID and returns the product details.
 - `PUT /products/{id}`: Updates an existing product based on the data provided in the `PUT` request.
 - `DELETE /products/{id}`: Deletes a product based on the ID provided in the `DELETE` request.

### Database Configuration Retrieval:

When the application is in a staging environment, it provides information about the database configuration, such as the connection and port, when the `GET /configuration/database` route is accessed.

### Category and Tags Management:

Creating a new product involves checking the existence of the product category in the database. If it does not exist, the category will be created. The same applies to tags associated with the product.

### Environment Control:

The program checks the environment in which it is being executed (development, staging, production) using `app.Environment.IsStaging()` and defines the route to retrieve database configuration information only in the staging environment.

## Overview

This program is a sample application that demonstrates how to create a web API to manage products with ASP.NET Core and Entity Framework Core, as well as how to access configuration information depending on the environment in which the application is running. It serves as a solid foundation for building a complete product management application.
