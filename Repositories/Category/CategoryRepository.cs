using DBModels;
using Microsoft.Data.Sqlite;

namespace Repositories.Category;

public class CategoryRepository : ICategoryRepository
{
    private readonly SqliteConnection _connection;
    public CategoryRepository(SqliteConnection connection)
    {
        _connection = connection;
    }
    private static CategoryDBModel MapCustomerCard(SqliteDataReader reader) => new(
        reader.GetInt64(reader.GetOrdinal("category_number")),
        reader.GetString(reader.GetOrdinal("category_name"))
    );

    public CategoryDBModel? GetCategory(long id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM Category WHERE category_number = @id";
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReader();
        return reader.Read() ? MapCustomerCard(reader) : null;
    }

    public IEnumerable<CategoryDBModel> GetCategories(bool sortByName = true)
    {
        using var command = _connection.CreateCommand();
        var query = "SELECT * FROM Category";
        if (sortByName) query += "ORDER BY category_name";
        command.CommandText = query;
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapCustomerCard(reader);
    }
}