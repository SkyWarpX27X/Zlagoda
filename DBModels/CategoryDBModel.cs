namespace DBModels;

public class CategoryDBModel(long id, string name)
{
    public long Id { get; set; } = id;
    public string Name { get; set; } = name;

    public CategoryDBModel(string name) : this(0, name) {}
}