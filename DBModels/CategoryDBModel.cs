namespace DBModels;

public class CategoryDBModel
{
    public long Id { get; set; }
    public string Name { get; set; }

    public CategoryDBModel(long id, string name)
    {
        Id = id;
        Name = name;
    }

    public CategoryDBModel(string name) : this(0, name) {}
}