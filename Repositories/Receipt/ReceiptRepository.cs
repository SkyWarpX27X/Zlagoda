using System.Globalization;
using DBModels;
using Microsoft.Data.Sqlite;

namespace Repositories.Receipt;

public class ReceiptRepository : IReceiptRepository
{
    private readonly SqliteConnection _connection;
    public ReceiptRepository(SqliteConnection connection)
    {
        _connection = connection;
    }
    private static ReceiptDBModel MapReceipt(SqliteDataReader reader) => new(
        reader.GetInt64(reader.GetOrdinal("receipt_number")),
        reader.GetInt64(reader.GetOrdinal("id_employee")),
         reader.IsDBNull(reader.GetOrdinal("card_number")) ? null : reader.GetString(reader.GetOrdinal("card_number")),
        reader.GetString(reader.GetOrdinal("print_date")),
        reader.GetDecimal(reader.GetOrdinal("sum_total")),
        reader.GetDecimal(reader.GetOrdinal("vat"))
    );

    public ReceiptDBModel? GetReceipt(long id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM Receipt WHERE receipt_number = @id";
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReader();
        return reader.Read() ? MapReceipt(reader) : null;
    }
    
    public IEnumerable<ReceiptDBModel> GetReceipts()
    {
        using var command = _connection.CreateCommand(); 
        command.CommandText = "SELECT * FROM Receipt";
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapReceipt(reader);
    }
}