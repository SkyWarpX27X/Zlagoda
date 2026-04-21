using Microsoft.Data.Sqlite;

namespace Repositories.Filiushkin;

public class FiliushkinQueries : IFiliushkinQueries
{
    private readonly SqliteConnection _connection;

    public FiliushkinQueries(SqliteConnection connection)
    {
        _connection = connection;
    }
    
    public IEnumerable<IFiliushkinQueries.EmployeeSalesByCategoryRow> EmployeeSalesByCategory(DateTime start, DateTime end)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              SELECT Employee.empl_surname, Employee.empl_name, Employee.empl_patronymic, Category.category_name, SUM(Sale.selling_price * Sale.product_number) AS sum_total
                              FROM Receipt
                                       JOIN Employee ON Employee.id_employee = Receipt.id_employee
                                       JOIN Sale ON Sale.receipt_number = Receipt.receipt_number
                                       JOIN Store_Product ON Sale.UPC = Store_Product.UPC
                                       JOIN Product ON Product.id_product = Store_Product.id_product
                                       JOIN Category ON Category.category_number = Product.category_number
                              WHERE Receipt.print_date BETWEEN @start_date AND @end_date
                              GROUP BY Category.category_number, Category.category_name,
                                       Employee.id_employee, Employee.empl_surname, Employee.empl_name, Employee.empl_patronymic
                              ORDER BY sum_total DESC
                              """;
        command.Parameters.AddWithValue("@start_date", start.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@end_date", end.ToString("yyyy-MM-dd HH:mm:ss"));
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            yield return new(
                reader.GetString(reader.GetOrdinal("empl_surname")),
                reader.GetString(reader.GetOrdinal("empl_name")),
                reader.GetString(reader.GetOrdinal("empl_patronymic")),
                reader.GetString(reader.GetOrdinal("category_name")),
                reader.GetDecimal(reader.GetOrdinal("sum_total"))
            );
        }
    }

    public IEnumerable<IFiliushkinQueries.EmployeesSoldAllProductsRow> EmployeesSoldAllProducts()
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              SELECT Employee.empl_surname, Employee.empl_name, Employee.empl_patronymic
                              FROM Employee
                              WHERE NOT EXISTS(
                                  SELECT Product.id_product
                                  FROM Product
                                  WHERE NOT EXISTS(
                                      SELECT Sale.UPC
                                      FROM Sale
                                      JOIN Receipt ON Receipt.receipt_number = Sale.receipt_number
                                      JOIN Store_Product ON Sale.UPC = Store_Product.UPC
                                      WHERE Store_Product.id_product = Product.id_product
                                          AND Receipt.id_employee = Employee.id_employee
                                  )
                              )
                              """;
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            yield return new(
                reader.GetString(reader.GetOrdinal("empl_surname")),
                reader.GetString(reader.GetOrdinal("empl_name")),
                reader.GetString(reader.GetOrdinal("empl_patronymic"))
            );
        }
    }
}