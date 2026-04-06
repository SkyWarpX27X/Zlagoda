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
    
    public IEnumerable<SaleDBModel> GetSales()
    {
        using var command = _connection.CreateCommand(); 
        command.CommandText = "SELECT * FROM Sale";
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapSale(reader);
    }
}