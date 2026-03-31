using System.Security.Cryptography;
using DBModels;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Hosting;

namespace Storage;

public class SQLiteStorageContext : IStorageContext
{
    private readonly string _databaseFilePath;
    private SqliteConnection? _connection;

    public SQLiteStorageContext(string databaseFilePath)
    {
        _databaseFilePath = databaseFilePath;
    }

    private void Init()
    {
        if (_connection is not null) return;
        bool isFirstLaunch = !File.Exists(_databaseFilePath);
        if (isFirstLaunch) CreateDatabase();
        else
        {
            _connection = new SqliteConnection($"DataSource={_databaseFilePath}");
            _connection.Open();
        }
    }

    private void CreateDatabase()
    {
        File.Create(_databaseFilePath).Dispose();
        _connection = new SqliteConnection($"DataSource={_databaseFilePath}");
        _connection.Open();
        using var command = _connection.CreateCommand();
        // Check in the future: set limitations for date (since DateTime doesn't exist) and maybe add limits for foreign keys?
        command.CommandText = """
                              CREATE TABLE IF NOT EXISTS Employee (
                                  id_employee TEXT PRIMARY KEY CHECK(length(id_employee) <= 10),
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
                                  zip_code TEXT NOT NULL CHECK(length(zip_code) <= 9)
                              );
                              """; 
        command.ExecuteNonQuery();
        command.CommandText = """
                              CREATE TABLE IF NOT EXISTS User_Account (
                                  id_user INT PRIMARY KEY,
                                  id_employee TEXT NOT NULL,
                                  user_name TEXT NOT NULL CHECK(length(user_name) <= 20),
                                  user_password TEXT NOT NULL CHECK(length(user_password) <= 20),
                                  FOREIGN KEY (id_employee) 
                                    REFERENCES Employee (id_employee)
                                    ON UPDATE CASCADE
                                    ON DELETE NO ACTION
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
                                card_number INT PRIMARY KEY CHECK(length(card_number) <= 13),
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
                                receipt_number TEXT PRIMARY KEY CHECK(length(receipt_number) <= 10),
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

    public UserDBModel? GetUser(int userId)
    {
        Init();
        using var command = _connection!.CreateCommand();
        command.CommandText = "SELECT * FROM User_Account WHERE id_user = @id";
        command.Parameters.AddWithValue("@id", userId);
        using var reader = command.ExecuteReader();
        if (!reader.Read()) return null;
        var employeeId = reader.GetString(1);
        var username = reader.GetString(2);
        var password = reader.GetString(3);
        return new UserDBModel(userId, employeeId, username, password);
    }
    
}