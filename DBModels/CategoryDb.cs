namespace DBModels;

public class CategoryDb
{
    public int Id { get; set; }
    public string Name { get; set; }

    public CategoryDb(int id, string name)
    {
        Id = id;
        Name = name;
    }
}