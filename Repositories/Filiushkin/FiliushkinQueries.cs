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
                              SELECT Employee.empl_surname, Employee.empl_name, Category.category_name, Receipt.print_date, SUM(Sale.selling_price * Sale.product_number) AS sum_total
                              FROM Receipt
                                  JOIN Employee ON Employee.id_employee = Receipt.id_employee
                                  JOIN Sale ON Sale.receipt_number = Receipt.receipt_number
                                  JOIN Store_Product ON Sale.UPC = Store_Product.UPC
                                  JOIN Product ON Product.id_product = Store_Product.id_product
                                  JOIN Category ON Category.category_number = Product.category_number
                                  WHERE Receipt.print_date BETWEEN @start_date AND @end_date
                                  GROUP BY Receipt.print_date, Category.category_number, Category.category_name,
                                           Employee.id_employee, Employee.empl_surname, Employee.empl_name
                                  ORDER BY Receipt.print_date, sum_total DESC
                              """;
        command.Parameters.AddWithValue("@start_date", start.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@end_date", end.ToString("yyyy-MM-dd HH:mm:ss"));
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            yield return new(
                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                DateTime.Parse(reader.GetString(3)),
                reader.GetDecimal(4)
            );
        }
    }

    public IEnumerable<IFiliushkinQueries.EmployeesSoldAllProductsRow> EmployeesSoldAllProducts()
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              SELECT Employee.empl_surname, Employee.empl_name
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
                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2)
            );
        }
    }
}