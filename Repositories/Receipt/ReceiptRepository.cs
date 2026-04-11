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
    
    public IEnumerable<ReceiptDBModel> GetReceipts((string StartDate, string EndDate)? dates = null)
    {
        using var command = _connection.CreateCommand(); 
        var query = "SELECT * FROM Receipt";
        if (dates != null)
            query += "WHERE print_date BETWEEN @startDate AND @endDate";
        command.CommandText = query;
        if (dates != null)
        {
            command.Parameters.AddWithValue("@startDate", dates?.StartDate);
            command.Parameters.AddWithValue("@endDate", dates?.EndDate);
        }
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapReceipt(reader);
    }

    public IEnumerable<ReceiptDBModel> GetReceiptsByCashier(long employeeId, (string StartDate, string EndDate)? dates = null)
    {
        using var command = _connection.CreateCommand();
        var query = "SELECT * FROM Receipt WHERE id_employee = @employeeId";
        if (dates != null)
            query += "AND print_date BETWEEN @startDate AND @endDate";
        command.CommandText = query;
        command.Parameters.AddWithValue("@employeeId", employeeId);
        if (dates != null)
        {
            command.Parameters.AddWithValue("@startDate", dates?.StartDate);
            command.Parameters.AddWithValue("@endDate", dates?.EndDate);
        }
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapReceipt(reader);
    }

    public decimal GetSumTotal((string StartDate, string EndDate) dates)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              SELECT COALESCE(SUM(sum_total), 0)
                              FROM Receipt 
                              WHERE print_date BETWEEN @startDate AND @endDate;
                              """;
        command.Parameters.AddWithValue("@startDate", dates.StartDate);
        command.Parameters.AddWithValue("@endDate", dates.EndDate);
        return Convert.ToDecimal(command.ExecuteScalar());
    }

    public decimal GetSumByCashier(long employeeId, (string StartDate, string EndDate) dates)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              SELECT COALESCE(SUM(sum_total), 0)
                              FROM Receipt 
                              WHERE id_employee = @employeeId
                              AND print_date BETWEEN @startDate AND @endDate;
                              """;
        command.Parameters.AddWithValue("@employeeId", employeeId);
        command.Parameters.AddWithValue("@startDate", dates.StartDate);
        command.Parameters.AddWithValue("@endDate", dates.EndDate);
        return Convert.ToDecimal(command.ExecuteScalar());
    }
    
    public long AddReceipt(ReceiptDBModel receipt)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              INSERT INTO Receipt (id_employee, card_number, print_date, sum_total, vat)
                              VALUES (@id_employee, @card_number, @print_date, @sum_total, @vat);
                              SELECT last_insert_rowid();
                              """;
        command.Parameters.AddWithValue("@id_employee", receipt.EmployeeId);
        command.Parameters.AddWithValue("@card_number", receipt.CardNumber is null ? DBNull.Value : receipt.CardNumber);
        command.Parameters.AddWithValue("@print_date", receipt.PrintDate);
        command.Parameters.AddWithValue("@sum_total", receipt.TotalSum);
        command.Parameters.AddWithValue("@vat", receipt.Vat);
        return Convert.ToInt64(command.ExecuteScalar());
    }

    public void DeleteReceipt(long id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM Receipt WHERE receipt_number = @receipt_number;";
        command.Parameters.AddWithValue("@receipt_number", id);
        command.ExecuteNonQuery();
    }

}