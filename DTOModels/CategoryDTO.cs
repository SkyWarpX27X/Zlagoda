namespace DTOModels;

public class CategoryDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    
    public CategoryDTO(long id, string name)
    {
        Id = id;
        Name = name;
    }
}