using DBModels;
using Microsoft.Data.Sqlite;

namespace Repositories;

public class OstapchukQueries : IOstapchukQueries
{
    private readonly SqliteConnection _connection;

    public OstapchukQueries(SqliteConnection connection)
    {
        _connection = connection;
    }

    public IEnumerable<(long, string, int)> GetProductCountByCategories(bool prom)
    {
        List<(long, string, int)> result = new List<(long, string, int)>();
        using (var  command = _connection.CreateCommand())
        {
            command.CommandText = """
                                  SELECT Category.category_number, category_name, COUNT(*) AS product_count
                                  FROM Product JOIN Category ON Product.category_number = Category.category_number
                                               JOIN Store_Product ON Store_Product.id_product = Product.id_product
                                  WHERE promotional_product = @prom AND products_number > 0
                                  GROUP BY Category.category_number, category_name
                                  """;
            command.Parameters.AddWithValue("@prom", prom);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(
                        (reader.GetInt64(reader.GetOrdinal("category_number")),
                         reader.GetString(reader.GetOrdinal("category_name")),
                         reader.GetInt32(reader.GetOrdinal("product_count"))));
                }
            }
        }
        return result;
    }
    
    public IEnumerable<(long, string, string, string?, decimal)> GetExperiencedCashiers()
    {
        List<(long, string, string, string?, decimal)> result = new List<(long, string, string, string?, decimal)>();
        using (var  command = _connection.CreateCommand())
        {
            command.CommandText = """
                                  SELECT id_employee, empl_surname, empl_name, empl_patronymic, salary
                                  FROM Employee
                                  WHERE NOT EXISTS(
                                      SELECT *
                                      FROM Customer_Card
                                      WHERE NOT EXISTS(
                                          SELECT *
                                          FROM Receipt
                                          WHERE Receipt.id_employee = Employee.id_employee AND Receipt.card_number = Customer_Card.card_number
                                      )
                                  )
                                  """;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(
                        (reader.GetInt64(reader.GetOrdinal("id_employee")), 
                         reader.GetString(reader.GetOrdinal("empl_surname")),
                         reader.GetString(reader.GetOrdinal("empl_name")),
                         reader.IsDBNull(reader.GetOrdinal("empl_patronymic")) ? null : reader.GetString(reader.GetOrdinal("empl_patronymic")),
                         reader.GetDecimal(reader.GetOrdinal("salary"))));
                }
            }
        }
        return result;
    }
}