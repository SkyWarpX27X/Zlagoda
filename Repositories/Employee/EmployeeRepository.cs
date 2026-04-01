using DBModels;
using Microsoft.Data.Sqlite;

namespace Repositories.Employee;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly SqliteConnection _connection;
    public EmployeeRepository(SqliteConnection connection)
    {
        _connection = connection;
    }
    private static EmployeeDBModel MapEmployee(SqliteDataReader reader) => new(
        reader.GetInt64(reader.GetOrdinal("id_employee")),
        reader.GetString(reader.GetOrdinal("empl_surname")),
        reader.GetString(reader.GetOrdinal("empl_name")),
        reader.GetString(reader.GetOrdinal("empl_patronymic")),
        reader.GetString(reader.GetOrdinal("empl_role")),
        reader.GetDecimal(reader.GetOrdinal("salary")),
        reader.GetString(reader.GetOrdinal("date_of_birth")),
        reader.GetString(reader.GetOrdinal("date_of_start")),
        reader.GetString(reader.GetOrdinal("phone_number")),
        reader.GetString(reader.GetOrdinal("city")),
        reader.GetString(reader.GetOrdinal("street")),
        reader.GetString(reader.GetOrdinal("zip_code")),
        reader.GetString(reader.GetOrdinal("user_name")),
        reader.GetString(reader.GetOrdinal("password"))
    );
    public EmployeeDBModel? GetEmployee(long id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM Employee WHERE id_employee = @id";
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReader();
        return reader.Read() ? MapEmployee(reader) : null;
    }

    public IEnumerable<EmployeeDBModel> GetEmployees(bool sortBySurname = true, bool cashiersOnly = false)
    {
        using var command = _connection.CreateCommand();
        var query = "SELECT * FROM Employee";
        if (cashiersOnly) query += " WHERE empl_role = 'Cashier'";
        if (sortBySurname) query += " ORDER BY empl_surname";
        command.CommandText = query;
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapEmployee(reader);
    }
    
}