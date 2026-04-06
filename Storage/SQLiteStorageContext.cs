using System.Security.Cryptography;
using DBModels;

using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Hosting;
using Repositories.Category;
using Repositories.CustomerCard;
using Repositories.Employee;
using Repositories.Product;
using Repositories.Receipt;
using Repositories.Sale;
using Repositories.StoreProduct;

namespace Storage;

public class SQLiteStorageContext
{
    private readonly SqliteConnection _connection;
    public IEmployeeRepository Employees { get; private set; }
    public ICategoryRepository Categories { get; private set; }
    public ICustomerCardRepository CustomerCards { get; private set; }
    public IProductRepository Products { get; private set; }
    public IStoreProductRepository StoreProducts { get; private set; }
    public IReceiptRepository Receipts { get; private set; }
    public ISaleRepository Sales { get; private set; }

    public SQLiteStorageContext(string databaseFilePath)
    {
        var isFirstLaunch = !File.Exists(databaseFilePath);
        _connection = new SqliteConnection($"DataSource={databaseFilePath}");
        _connection.Open();
        Employees = new EmployeeRepository(_connection);
        Categories = new CategoryRepository(_connection);
        CustomerCards = new CustomerCardRepository(_connection);
        Products = new ProductRepository(_connection);
        StoreProducts = new StoreProductRepository(_connection);
        Receipts = new ReceiptRepository(_connection);
        Sales = new SaleRepository(_connection);
        if (isFirstLaunch) CreateDatabase();
    }
    
    private void CreateDatabase()
    {
        using var command = _connection.CreateCommand();
        // Check in the future: set limitations for date (since DateTime doesn't exist) and maybe add limits for foreign keys?
        command.CommandText = """
                              CREATE TABLE IF NOT EXISTS Employee (
                                  id_employee INT PRIMARY KEY,
                                  empl_surname TEXT NOT NULL CHECK(length(empl_surname) <= 50),
                                  empl_name TEXT NOT NULL CHECK(length(empl_name) <= 50),
                                  empl_patronymic TEXT NULL CHECK(length(empl_patronymic) <= 50),
                                  empl_role TEXT NOT NULL CHECK(length(empl_role) <= 10),
                                  salary DECIMAL(13,4) NOT NULL,
                                  date_of_birth TEXT NOT NULL,
                                  date_of_start TEXT NOT NULL,
                                  phone_number TEXT NOT NULL CHECK(length(phone_number) <= 13),
                                  city TEXT NOT NULL CHECK(length(city) <= 50),
                                  street TEXT NOT NULL CHECK(length(street) <= 50),
                                  zip_code TEXT NOT NULL CHECK(length(zip_code) <= 9),
                                  user_name TEXT NOT NULL CHECK(length(user_name) <= 20) UNIQUE,
                                  user_password TEXT NOT NULL CHECK(length(user_password) <= 20)                                
                              );
                              """; 
        command.ExecuteNonQuery();
        command.CommandText = """
                              CREATE TABLE IF NOT EXISTS Category (
                                  category_number INT PRIMARY KEY,
                                  category_name TEXT NOT NULL CHECK(length(category_name) <= 50)
                              );
                              """;
        command.ExecuteNonQuery();
        command.CommandText = """
                              CREATE TABLE IF NOT EXISTS Product (
                                id_product INT PRIMARY KEY,
                                category_number INT NOT NULL,
                                product_name TEXT NOT NULL CHECK(length(product_name) <= 50),
                                characteristics TEXT NOT NULL CHECK(length(characteristics) <= 50),
                                manufacturer TEXT NOT NULL CHECK(length(manufacturer) <= 50),
                                FOREIGN KEY (category_number)
                                    REFERENCES Category (category_number)
                                    ON UPDATE CASCADE
                                    ON DELETE NO ACTION
                              );
                              """;
        command.ExecuteNonQuery();
        command.CommandText = """
                              CREATE TABLE IF NOT EXISTS Store_Product (
                                  UPC TEXT PRIMARY KEY CHECK(length(UPC) <= 12),
                                  UPC_prom TEXT NOT NULL,
                                  id_product INT NOT NULL,
                                  selling_price DECIMAL(13,4) NOT NULL,
                                  products_number INT NOT NULL,
                                  promotional_product BOOL NOT NULL,
                                  FOREIGN KEY (UPC_prom)
                                    REFERENCES Store_Product (UPC)
                                    ON UPDATE CASCADE
                                    ON DELETE SET NULL,
                                  FOREIGN KEY (id_product)
                                    REFERENCES Product (id_product)
                                    ON UPDATE CASCADE
                                    ON DELETE NO ACTION
                              );
                              """;
        command.ExecuteNonQuery();
        command.CommandText = """
                              CREATE TABLE IF NOT EXISTS Customer_Card (
                                card_number TEXT PRIMARY KEY CHECK(length(card_number) <= 13),
                                cust_surname TEXT NOT NULL CHECK(length(cust_surname) <= 50),
                                cust_name TEXT NOT NULL CHECK(length(cust_name) <= 50),
                                cust_patronymic  TEXT NULL CHECK(length(cust_patronymic) <= 50),
                                phone_number TEXT NOT NULL CHECK(length(phone_number) <= 13),
                                city TEXT NULL CHECK(length(city) <= 50),
                                street TEXT NULL CHECK(length(street) <= 50),
                                zip_code TEXT NULL CHECK(length(zip_code) <= 9),
                                percent INT NOT NULL
                              );
                              """;
        command.CommandText = """
                              CREATE TABLE IF NOT EXISTS Receipt (
                                receipt_number INT PRIMARY KEY,
                                id_employee TEXT NOT NULL,
                                card_number TEXT NOT NULL,
                                print_date TEXT NOT NULL,
                                sum_total DECIMAL(13,4) NOT NULL,
                                vat DECIMAL(13,4) NOT NULL,
                                FOREIGN KEY (id_employee)
                                    REFERENCES Employee (id_employee)
                                    ON UPDATE CASCADE
                                    ON DELETE NO ACTION,
                                FOREIGN KEY (card_number)
                                    REFERENCES Customer_Card (card_number)
                                    ON UPDATE CASCADE
                                    ON DELETE NO ACTION
                              );
                              """;
        command.ExecuteNonQuery();
        command.CommandText = """
                              CREATE TABLE IF NOT EXISTS Sale (
                                UPC TEXT NOT NULL,
                                receipt_number TEXT NOT NULL,
                                product_number INT NOT NULL,
                                selling_price DECIMAL(13,4) NOT NULL,
                                FOREIGN KEY (UPC)
                                  REFERENCES Store_Product (UPC)
                                  ON UPDATE CASCADE
                                  ON DELETE NO ACTION,
                                FOREIGN KEY (receipt_number)
                                  REFERENCES Receipt (receipt_number)
                                  ON UPDATE CASCADE
                                  ON DELETE CASCADE,
                                PRIMARY KEY(UPC, receipt_number)
                              );
                              """;
        command.ExecuteNonQuery();
    }
}