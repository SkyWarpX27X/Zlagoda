using DBModels;
using Microsoft.Data.Sqlite;

namespace Repositories.CustomerCard;

public class CustomerCardRepository : ICustomerCardRepository
{
    private SqliteConnection _connection;

    public CustomerCardRepository(SqliteConnection connection)
    {
        _connection = connection;
    }
    private static CustomerCardDBModel MapCustomer(SqliteDataReader reader) => new(
        reader.GetString(reader.GetOrdinal("card_number")),
        reader.GetString(reader.GetOrdinal("cust_surname")),
        reader.GetString(reader.GetOrdinal("cust_name")),
        reader.IsDBNull(reader.GetOrdinal("cust_patronymic")) ? null : reader.GetString(reader.GetOrdinal("cust_patronymic")),
        reader.GetString(reader.GetOrdinal("phone_number")),
        reader.IsDBNull(reader.GetOrdinal("city")) ? null : reader.GetString(reader.GetOrdinal("city")),
        reader.IsDBNull(reader.GetOrdinal("street")) ? null : reader.GetString(reader.GetOrdinal("street")),
        reader.IsDBNull(reader.GetOrdinal("zip_code")) ? null : reader.GetString(reader.GetOrdinal("zip_code")),
        reader.GetInt32(reader.GetOrdinal("percent"))
    );

    public CustomerCardDBModel? GetCustomer(string number)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM Customer_Card WHERE card_number = @number";
        command.Parameters.AddWithValue("@number", number);
        using var reader = command.ExecuteReader();
        return reader.Read() ? MapCustomer(reader) : null;
    }

    public IEnumerable<CustomerCardDBModel> GetCustomers(bool sortByName = true, int percent = -1)
    {
        using var command = _connection.CreateCommand();
        var query = "SELECT * FROM Customer_Card";
        if (percent > -1) query += " WHERE percent == @percent";
        if (sortByName) query += " ORDER BY cust_surname";
        command.CommandText = query;
        command.Parameters.AddWithValue("@percent", percent);
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapCustomer(reader);
    }

    public IEnumerable<CustomerCardDBModel> GetCustomersBySurname(string surnameQuery)
    {
        var namePattern = surnameQuery + "%";
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM Customer_Card WHERE LOWER(cust_surname) LIKE LOWER(@namePattern)";
        command.Parameters.AddWithValue("@namePattern", namePattern);
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapCustomer(reader);
    }
    public void AddCustomerCard(CustomerCardDBModel card)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              INSERT INTO Customer_Card (card_number, cust_surname, cust_name, cust_patronymic,
                                                         phone_number, city, street, zip_code, percent) 
                              VALUES (@card_number, @cust_surname, @cust_name, @cust_patronymic, @phone_number,
                                      @city, @street, @zip_code, @percent);
                              """;
        command.Parameters.AddWithValue("@card_number", card.Number);
        command.Parameters.AddWithValue("@cust_surname", card.Surname);
        command.Parameters.AddWithValue("@cust_name", card.Name);
        command.Parameters.AddWithValue("@cust_patronymic", card.Patronymic is null ? DBNull.Value : card.Patronymic);
        command.Parameters.AddWithValue("@phone_number", card.PhoneNumber);
        command.Parameters.AddWithValue("@city", card.City is null ? DBNull.Value : card.City);
        command.Parameters.AddWithValue("@street", card.Street is null ? DBNull.Value : card.Street);
        command.Parameters.AddWithValue("@zip_code", card.ZipCode is  null ? DBNull.Value : card.ZipCode);
        command.Parameters.AddWithValue("@percent", card.Percent);
        try
        {
            command.ExecuteNonQuery();
        }
        catch (SqliteException ex) when (ex.SqliteExtendedErrorCode == 1555)
        {
            throw new InvalidOperationException($"A card with number {card.Number} already exists.");
        }
    }

    public void UpdateCustomerCard(CustomerCardDBModel card)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              UPDATE Customer_Card 
                              SET cust_surname = @cust_surname,
                                  cust_name = @cust_name,
                                  cust_patronymic = @cust_patronymic,
                                  phone_number = @phone_number,
                                  city = @city,
                                  street = @street,
                                  zip_code = @zip_code,
                                  percent = @percent
                              WHERE card_number = @card_number;
                              """;
        command.Parameters.AddWithValue("@card_number", card.Number);
        command.Parameters.AddWithValue("@cust_surname", card.Surname);
        command.Parameters.AddWithValue("@cust_name", card.Name);
        command.Parameters.AddWithValue("@cust_patronymic", card.Patronymic is null ? DBNull.Value : card.Patronymic);
        command.Parameters.AddWithValue("@phone_number", card.PhoneNumber);
        command.Parameters.AddWithValue("@city", card.City is null ? DBNull.Value : card.City);
        command.Parameters.AddWithValue("@street", card.Street is null ? DBNull.Value : card.Street);
        command.Parameters.AddWithValue("@zip_code", card.ZipCode is null ? DBNull.Value : card.ZipCode);
        command.Parameters.AddWithValue("@percent", card.Percent);
        try
        {
            command.ExecuteNonQuery();
        }
        catch (SqliteException ex) when (ex.SqliteExtendedErrorCode == 1555)
        {
            throw new InvalidOperationException($"A card with number {card.Number} already exists.");
        }
    }

    public void DeleteCustomerCard(string number)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM Customer_Card WHERE card_number = @card_number";
        command.Parameters.AddWithValue("@card_number", number);
        command.ExecuteNonQuery();
    }
}