# Product Web API with ASP.NET Core 8 and LINQ

This project is an example implementation of a Web API using ASP.NET Core 8 with LINQ as the Object-Relational Mapper (ORM). The use case is the management of product data through the `Product` table.

## Features

* **Product CRUD (Create, Read, Update, Delete):**
    * Add new products.
    * View a list of all products.
    * View product details by ID.
    * Modify product data.
    * Delete products.
* **LINQ for Data Access:**
    * Implementation of database operations using LINQ for ease and flexibility.
* **RESTful API Architecture:**
    * API design follows RESTful principles for ease of use and scalability.
* **In-Memory or SQL Server Database:**
    * The project can be configured to use an in-memory database for development or SQL Server for production.

## Technologies Used

* **ASP.NET Core 8:** Web framework for building APIs.
* **.NET 8:** Development platform.
* **Entity Framework Core:** ORM for database access.
* **LINQ (Language Integrated Query):** Integrated query language for data manipulation.
* **SQL Server (optional):** Relational database.
* **Swagger/OpenAPI:** API documentation.

## Setup

1.  **Install .NET 8 SDK:**
    * Download and install the .NET 8 SDK from the official Microsoft website.
2.  **Install Visual Studio or Visual Studio Code:**
    * Use Visual Studio or Visual Studio Code as your IDE.
3.  **Install SQL Server (optional):**
    * If you want to use SQL Server, install and configure SQL Server on your computer.

## Configuration

1.  **Database Configuration:**
    * Open the `appsettings.json` file.
    * Change the connection string according to your database configuration.
        * To use an in-memory database, leave the default configuration.
        * To use SQL Server, replace the connection string with a connection to your SQL Server database.
2.  **Database Migration:**
    * Open a terminal in the project directory.
    * Run the following commands to create migrations and update the database:
        * `dotnet ef migrations add InitialCreate`
        * `dotnet ef database update`

## How to Run the Application

1.  **Open the project in Visual Studio or Visual Studio Code.**
2.  **Run the application by pressing the "Run" button or using the `dotnet run` command in the terminal.**
3.  **Open a browser and access the API URL (e.g., `https://localhost:5001/swagger`) to view the Swagger documentation.**
4.  **Use Postman or a similar tool to test the API endpoints.**

## API Endpoints

* `GET /api/products`: Get a list of all products.
* `GET /api/products/{id}`: Get product details by ID.
* `POST /api/products`: Add a new product.
* `PUT /api/products/{id}`: Modify product data.
* `DELETE /api/products/{id}`: Delete a product.

## Usage Examples

* **Add Product:**

    ```json
    POST /api/products
    {
      "name": "Laptop XYZ",
      "description": "High-specification laptop",
      "price": 12000000
    }
    ```

* **Get Product List:**

    ```json
    GET /api/products
    ```

## Contribution

If you would like to contribute to this project, please fork the repository and submit a pull request.

## License

This project is licensed under the \[MIT/Apache 2.0/etc.] license.


`<tipe>/<deskripsi-singkat>`

`feature/add-payment-gateway`

`feature/add-user-authentication`

`db/add-orders-table`

`bugfix/fix-login-validation-error`

`refactor/simplify-payment-logic`

`experiment/test-new-auth-library`

# ERD API Produk

## Entitas

### Products

* **ProductID** (INT, Primary Key)
* **ProductName** (VARCHAR)
* **Description** (TEXT)
* **Price** (DECIMAL)
* **CategoryID** (INT, Foreign Key)
* **SupplierID** (INT, Foreign Key)

### Categories

* **CategoryID** (INT, Primary Key)
* **CategoryName** (VARCHAR)

### Inventory

* **InventoryID** (INT, Primary Key)
* **ProductID** (INT, Foreign Key)
* **StockQuantity** (INT)
* **LastStockUpdate** (DATETIME)

### Suppliers

* **SupplierID** (INT, Primary Key)
* **SupplierName** (VARCHAR)
* **ContactPerson** (VARCHAR)
* **ContactPhone** (VARCHAR)

### Orders

* **OrderID** (INT, Primary Key)
* **OrderDate** (DATETIME)
* **CustomerID** (INT, Foreign Key)
* **TotalAmount** (DECIMAL)

### OrderDetails

* **OrderDetailID** (INT, Primary Key)
* **OrderID** (INT, Foreign Key)
* **ProductID** (INT, Foreign Key)
* **Quantity** (INT)
* **UnitPrice** (DECIMAL)

## Relasi

* Products memiliki satu Category (1:1..*).
* Products memiliki satu Supplier (1:1).
* Inventory terhubung ke satu Product (1:1).
* Orders terdiri dari banyak OrderDetails (1:N).
* OrderDetails terhubung ke satu Order (N:1).
* OrderDetails terhubung ke satu Product (N:1).

## Diagram

```mermaid
erDiagram
    Products {
        guid ProductID PK
        varchar ProductName
        text Description
        decimal Price
        guid CategoryID FK
        guid SupplierID FK
    }

    Categories {
        guid CategoryID PK
        varchar CategoryName
    }

    Inventory {
        guid InventoryID PK
        guid ProductID FK
        int StockQuantity
        datetime LastStockUpdate
    }

    Suppliers {
        guid SupplierID PK
        varchar SupplierName
        varchar ContactPerson
        varchar ContactPhone
    }

    Customer {
        guid Id PK
        varchar Name
        varchar Email
        varchar Phone
        varchar Address
    }

    Orders {
        guid OrderID PK
        datetime OrderDate
        guid CustomerID FK
        decimal TotalAmount
    }

    OrderDetails {
        guid OrderDetailID PK
        guid OrderID FK
        guid ProductID FK
        int Quantity
        decimal UnitPrice
    }

    Products }|--|| Categories : memiliki
    Products }|--|| Suppliers : dari
    Inventory }|--|| Products : memiliki
    Orders ||--|{ OrderDetails : terdiri dari
    OrderDetails }|--|| Orders : ada di
    OrderDetails }|--|| Products : berisi
    Orders }|--|| Customer : ditempatkan oleh