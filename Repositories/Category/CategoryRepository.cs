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
        if (sortByName) query += " ORDER BY category_name";
        command.CommandText = query;
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapCustomerCard(reader);
    }

    public void AddCategory(CategoryDBModel category)
    {
        // If needed, I can return the newly generated Id to update the DB model, but for now I shall leave it like this.
        // Please write to me if that is indeed necessary.
        using var command = _connection.CreateCommand();
        command.CommandText = "INSERT INTO Category (category_name) VALUES (@category_name)";
        command.Parameters.AddWithValue("@category_name", category.Name);
        command.ExecuteNonQuery();
    }

    public void UpdateCategory(CategoryDBModel category)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "UPDATE Category SET category_name = @category_name WHERE category_number = @category_number";
        command.Parameters.AddWithValue("@category_name", category.Name);
        command.Parameters.AddWithValue("@category_number", category.Id);
        command.ExecuteNonQuery();
    }

    public void DeleteCategory(long id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM Category WHERE category_number = @category_number";
        command.Parameters.AddWithValue("@category_number", id);
        command.ExecuteNonQuery();
    }
}