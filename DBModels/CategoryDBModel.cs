namespace DBModels;

public class CategoryDBModel
{
    public int Id { get; set; }
    public string Name { get; set; }

    public CategoryDBModel(int id, string name)
    {
        Id = id;
        Name = name;
    }
}