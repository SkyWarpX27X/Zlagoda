using Microsoft.Data.Sqlite;

namespace Repositories.Romanyuk;

public class RomanyukQueries : IRomanyukQueries
{
    private readonly SqliteConnection _connection;
    public RomanyukQueries(SqliteConnection connection)
    {
        _connection = connection;
    }

    public IEnumerable<(long cardNumber, string customerSurname, string customerName, string? customerPatronymic, 
	    string customerPhone, long categoryNumber, string categoryName)> GetCustomersWhoBoughtWholeCategory()
    {
	    var res = new List<(long, string, string, string?, string, long, string)>();
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              SELECT card_number, cust_surname, cust_name, cust_patronymic, phone_number, category_number, category_name
                              FROM Customer_Card cc
                              JOIN Category c
                              ON NOT EXISTS (
                              		SELECT 1
                              		FROM Product p
                              		WHERE p.category_number = c.category_number
                              		AND NOT EXISTS (
                              				SELECT 1
                              				FROM Receipt r 
                              				JOIN Sale s on r.receipt_number = s.receipt_number
                              				JOIN Store_Product sp ON s.UPC = sp.UPC
                              				WHERE r.card_number = cc.card_number
                              				AND sp.id_product = p.id_product
                              				)
                              		)
                              AND EXISTS (
                                  SELECT 1
                                  FROM Product 
                                  WHERE Product.category_number = c.category_number
                                  );
                              """;
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
	        res.Add(
		        (reader.GetInt64(reader.GetOrdinal("card_number")),
		        reader.GetString(reader.GetOrdinal("cust_surname")),
		        reader.GetString(reader.GetOrdinal("cust_name")),
		        reader.IsDBNull(reader.GetOrdinal("cust_patronymic")) ? null : reader.GetString(reader.GetOrdinal("cust_patronymic")),
		        reader.GetString(reader.GetOrdinal("phone_number")),
		        reader.GetInt64(reader.GetOrdinal("category_number")),
		        reader.GetString(reader.GetOrdinal("category_name")))
	        );
        }
        return res;
    }

    public IEnumerable<(long categoryNumber, string categoryName, decimal avgPrice)> GetAvgPriceOfCategory(
	    long min = 0, long max = 9999)
    {
	    var res = new List<(long, string, decimal)>();
	    using var command = _connection.CreateCommand();
	    command.CommandText = """
	                          SELECT c.category_number, category_name, COALESCE(AVG(selling_price), 0) AS avg_price
	                          FROM Category c
	                          JOIN Product p ON p.category_number = c.category_number
	                          JOIN Store_Product sp ON sp.id_product = p.id_product 
	                          		AND sp.products_number > 0 
	                          		OR sp.promotional_product IS FALSE
	                          GROUP BY c.category_number, c.category_name
	                          HAVING avg_price >= @min AND avg_price <= @max
	                          ORDER BY avg_price DESC;
	                          """;
	    command.Parameters.AddWithValue("@min", min);
	    command.Parameters.AddWithValue("@max", max);
	    using var reader = command.ExecuteReader();
	    while (reader.Read())
	    {
		    res.Add(
			    (
				    reader.GetInt64(reader.GetOrdinal("category_number")),
				    reader.GetString(reader.GetOrdinal("category_name")),
				    reader.GetDecimal(reader.GetOrdinal("avg_price"))
			    )
		    );
	    }
	    return res;
    }
    
    
}