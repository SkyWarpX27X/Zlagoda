using DBModels;
using Microsoft.Data.Sqlite;

namespace Repositories.Sale;

public class SaleRepository : ISaleRepository
{
    private readonly SqliteConnection _connection;
    public SaleRepository(SqliteConnection connection)
    {
        _connection = connection;
    }
    private static SaleDBModel MapSale(SqliteDataReader reader) => new(
        reader.GetString(reader.GetOrdinal("UPC")),
        reader.GetInt64(reader.GetOrdinal("receipt_number")),
        reader.GetInt32(reader.GetOrdinal("product_number")),
        reader.GetDecimal(reader.GetOrdinal("selling_price"))
    );

    public SaleDBModel? GetSale(string upc, long receiptId)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM Sale WHERE UPC = @upc AND receipt_number = @receiptId";
        command.Parameters.AddWithValue("@upc", upc);
        command.Parameters.AddWithValue("@receiptId", receiptId);
        using var reader = command.ExecuteReader();
        return reader.Read() ? MapSale(reader) : null;
    }
    
    public IEnumerable<SaleDBModel> GetSales(long? receiptId = null)
    {
        using var command = _connection.CreateCommand(); 
        var query = "SELECT * FROM Sale";
        if (receiptId != null)
            query += " WHERE receipt_number = @receipt_number;";
        command.CommandText = query;
        if (receiptId != null)
            command.Parameters.AddWithValue("@receipt_number", receiptId);
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapSale(reader);
    }

    public void AddSale(SaleDBModel sale)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              INSERT INTO Sale (UPC, receipt_number, product_number, selling_price) 
                              VALUES (@UPC,  @receipt_number, @product_number, @selling_price)
                              """;
        command.Parameters.AddWithValue("@UPC", sale.UPC);
        command.Parameters.AddWithValue("@receipt_number", sale.ReceiptId);
        command.Parameters.AddWithValue("@product_number", sale.ProductQuantity);
        command.Parameters.AddWithValue("@selling_price", sale.SellingPrice);
        command.ExecuteNonQuery();
    }
}