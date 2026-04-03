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
    private static CustomerCardDBModel MapCategory(SqliteDataReader reader) => new(
        reader.GetString(reader.GetOrdinal("card_number")),
        reader.GetString(reader.GetOrdinal("cust_surname")),
        reader.GetString(reader.GetOrdinal("cust_name")),
        reader.GetString(reader.GetOrdinal("cust_patronymic")),
        reader.GetString(reader.GetOrdinal("phone_number")),
        reader.GetString(reader.GetOrdinal("city")),
        reader.GetString(reader.GetOrdinal("street")),
        reader.GetString(reader.GetOrdinal("zip_code")),
        reader.GetInt32(reader.GetOrdinal("percent"))
    );

    public CustomerCardDBModel? GetCategory(string number)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM Customer_Card WHERE card_number = @number";
        command.Parameters.AddWithValue("@number", number);
        using var reader = command.ExecuteReader();
        return reader.Read() ? MapCategory(reader) : null;
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
            yield return MapCategory(reader);
    }
}