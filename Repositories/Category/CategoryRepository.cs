using DBModels;
using Microsoft.Data.Sqlite;

namespace Repositories.Category;

public class CategoryRepository : ICategoryRepository
{
    
    public IEnumerable<CategoryDBModel> GetCategories()
    {
        List<CategoryDBModel> categories = new List<CategoryDBModel>();
        using (var connection = new SqliteConnection("Data Source=F:\\Programs\\Zlagoda\\maindb.sqlite"))
        {
            string query = "SELECT * FROM Category";
            connection.Open();
            using (var command = new SqliteCommand(query, connection))
            {
                using (var  reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(1);
                        string name = reader.GetString(0);
                        categories.Add(new CategoryDBModel(id, name));
                    }
                }
            }
        }
        return categories;
    }
}