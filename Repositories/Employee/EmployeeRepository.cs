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
        reader.IsDBNull(reader.GetOrdinal("empl_patronymic")) ? null : reader.GetString(reader.GetOrdinal("empl_patronymic")),
        reader.GetString(reader.GetOrdinal("empl_role")),
        reader.GetDecimal(reader.GetOrdinal("salary")),
        reader.GetString(reader.GetOrdinal("date_of_birth")),
        reader.GetString(reader.GetOrdinal("date_of_start")),
        reader.GetString(reader.GetOrdinal("phone_number")),
        reader.GetString(reader.GetOrdinal("city")),
        reader.GetString(reader.GetOrdinal("street")),
        reader.GetString(reader.GetOrdinal("zip_code")),
        reader.GetString(reader.GetOrdinal("user_name")),
        reader.GetString(reader.GetOrdinal("user_password"))
    );
    public EmployeeDBModel? GetEmployee(long id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM Employee WHERE id_employee = @id";
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReader();
        return reader.Read() ? MapEmployee(reader) : null;
    }

    public EmployeeDBModel? GetEmployee(string username)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM Employee WHERE user_name = @username";
        command.Parameters.AddWithValue("@username", username);
        using var reader = command.ExecuteReader();
        return reader.Read() ? MapEmployee(reader) : null;
    }

    public IEnumerable<EmployeeDBModel> GetEmployees(bool sortBySurname = true, bool cashiersOnly = false)
    {
        using var command = _connection.CreateCommand();
        var query = "SELECT * FROM Employee";
        if (cashiersOnly) query += " WHERE empl_role = 'Касир'";
        if (sortBySurname) query += " ORDER BY empl_surname";
        command.CommandText = query;
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapEmployee(reader);
    }

    public IEnumerable<EmployeeContactInfoDataModel> GetEmployeeContactInfo(string surnameQuery)
    {
        var namePattern = surnameQuery + "%";
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              SELECT empl_surname, phone_number, city, street, zip_code 
                              FROM Employee 
                              WHERE LOWER(empl_surname) LIKE LOWER(@namePattern)
                              """;
        command.Parameters.AddWithValue("@namePattern", namePattern);
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return new EmployeeContactInfoDataModel(
                reader.GetString(reader.GetOrdinal("empl_surname")),
                reader.GetString(reader.GetOrdinal("phone_number")),
                reader.GetString(reader.GetOrdinal("city")),
                reader.GetString(reader.GetOrdinal("street")),
                reader.GetString(reader.GetOrdinal("zip_code")));
    }
    
    public void AddEmployee(EmployeeDBModel employee)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              INSERT INTO Employee (empl_surname, empl_name, empl_patronymic, empl_role, salary, 
                                                    date_of_birth, date_of_start, phone_number, city, street, zip_code, 
                                                    user_name, user_password) 
                              VALUES (@empl_surname, @empl_name, @empl_patronymic, @empl_role, @salary, 
                              @date_of_birth, @date_of_start, @phone_number, @city, @street, @zip_code, @user_name, @user_password);
                              """;
        command.Parameters.AddWithValue("@empl_surname", employee.Surname);
        command.Parameters.AddWithValue("@empl_name", employee.Name);
        command.Parameters.AddWithValue("@empl_patronymic", employee.Patronymic is null ? DBNull.Value : employee.Patronymic);
        command.Parameters.AddWithValue("@empl_role", employee.Role);
        command.Parameters.AddWithValue("@salary", employee.Salary);
        command.Parameters.AddWithValue("@date_of_birth", employee.DateOfBirth);
        command.Parameters.AddWithValue("@date_of_start", employee.DateOfStart);
        command.Parameters.AddWithValue("@phone_number", employee.PhoneNumber);
        command.Parameters.AddWithValue("@city", employee.City);
        command.Parameters.AddWithValue("@street", employee.Street);
        command.Parameters.AddWithValue("@zip_code", employee.ZipCode);
        command.Parameters.AddWithValue("@user_name", employee.Username);
        command.Parameters.AddWithValue("@user_password", employee.Password);
        try
        {
            command.ExecuteNonQuery();
        }
        catch (SqliteException ex) when (ex.SqliteExtendedErrorCode == 2067)
        {
            throw new InvalidOperationException($"An employee with username \"{employee.Username}\" already exists.");
        }
    }

    public void UpdateEmployee(EmployeeDBModel employee)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              UPDATE Employee
                              SET empl_surname = @empl_surname,
                                  empl_name = @empl_name,
                                  empl_patronymic = @empl_patronymic,
                                  empl_role = @empl_role,
                                  salary = @salary,
                                  date_of_birth = @date_of_birth,
                                  date_of_start = @date_of_start,
                                  phone_number = @phone_number,
                                  city = @city,
                                  street = @street,
                                  zip_code = @zip_code,
                                  user_name = @user_name,
                                  user_password = @user_password
                              WHERE id_employee = @id_employee;
                              """;
        command.Parameters.AddWithValue("@empl_surname", employee.Surname);
        command.Parameters.AddWithValue("@empl_name", employee.Name);
        command.Parameters.AddWithValue("@empl_patronymic", employee.Patronymic is null ? DBNull.Value : employee.Patronymic);
        command.Parameters.AddWithValue("@empl_role", employee.Role);
        command.Parameters.AddWithValue("@salary", employee.Salary);
        command.Parameters.AddWithValue("@date_of_birth", employee.DateOfBirth);
        command.Parameters.AddWithValue("@date_of_start", employee.DateOfStart);
        command.Parameters.AddWithValue("@phone_number", employee.PhoneNumber);
        command.Parameters.AddWithValue("@city", employee.City);
        command.Parameters.AddWithValue("@street", employee.Street);
        command.Parameters.AddWithValue("@zip_code", employee.ZipCode);
        command.Parameters.AddWithValue("@user_name", employee.Username);
        command.Parameters.AddWithValue("@user_password", employee.Password);
        command.Parameters.AddWithValue("@id_employee", employee.Id);
        try
        {
            command.ExecuteNonQuery();
        }
        catch (SqliteException ex) when (ex.SqliteExtendedErrorCode == 2067)
        {
            throw new InvalidOperationException($"An employee with username \"{employee.Username}\" already exists.");
        }
    }

    public void DeleteEmployee(EmployeeDBModel employee)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM Employee WHERE id_employee = @id_employee";
        command.Parameters.AddWithValue("@id_employee", employee.Id);
        command.ExecuteNonQuery();
    }
    
}