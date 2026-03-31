using DBModels;
using Microsoft.Data.Sqlite;

namespace Repositories.Employee;

public class EmployeeRepository : IEmployeeRepository
{
    public IEnumerable<EmployeeDBModel> GetEmployees()
    {
        //Add other fields after test
        List<EmployeeDBModel> employees = new List<EmployeeDBModel>();
        using (var connection = new SqliteConnection("Data Source=C:\\Users\\Naz\\RiderProjects\\Zlagoda\\maindb.sqlite"))
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
                        employees.Add(new EmployeeDBModel(id, username, password, role));
                    }
                }
            }
        }
        return employees;
    }

    public EmployeeDBModel GetEmployee(int id)
    {
        return GetEmployees().First(e => e.Id == id);
    }
}