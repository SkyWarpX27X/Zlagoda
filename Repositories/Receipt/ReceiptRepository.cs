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

    public void AddReceipt(ReceiptDBModel receipt)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              INSERT INTO Receipt (id_employee, card_number, print_date, sum_total, vat) 
                              VALUES (@id_employee, @card_number, @print_date, @sum_total, @vat)
                              """;
        command.Parameters.AddWithValue("@id_employee", receipt.EmployeeId);
        command.Parameters.AddWithValue("@card_number", receipt.CardNumber);
        command.Parameters.AddWithValue("@print_date", receipt.PrintDate);
        command.Parameters.AddWithValue("@sum_total", receipt.TotalSum);
        command.Parameters.AddWithValue("@vat", receipt.Vat);
        command.ExecuteNonQuery();
    }

    public void DeleteReceipt(ReceiptDBModel receipt)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM Receipt WHERE receipt_number = @receipt_number;";
        command.Parameters.AddWithValue("@receipt_number", receipt.Id);
        command.ExecuteNonQuery();
    }

}