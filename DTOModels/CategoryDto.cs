namespace DTOModels;

public class CategoryDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    
    public CategoryDto(long id, string name)
    {
        Id = id;
        Name = name;
    }
}