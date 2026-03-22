using DBModels;
using Microsoft.Data.Sqlite;

namespace Repositories.Employee;

public class EmployeeRepository : IEmployeeRepository
{
    public IEnumerable<EmployeeDb> GetEmployees()
    {
        //Add other fields after test
        List<EmployeeDb> employees = new List<EmployeeDb>();
        using (var connection = new SqliteConnection("Data Source=F:\\Programs\\Zlagoda\\maindb.sqlite"))
        {
            string query = "SELECT * FROM Employee";
            connection.Open();
            using (var  command = new SqliteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string username = reader.GetString(1);
                        string password = reader.GetString(2);
                        string role = reader.GetString(3);
                        employees.Add(new EmployeeDb(id, username, password, role));
                    }
                }
            }
        }
        return employees;
    }

    public EmployeeDb GetEmployee(int id)
    {
        return GetEmployees().First(e => e.Id == id);
    }
}