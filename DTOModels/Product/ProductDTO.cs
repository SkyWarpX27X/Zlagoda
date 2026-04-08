namespace DTOModels;

public class ProductDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Characteristics { get; set; }
    public string Manufacturer { get; set; }

    public ProductDTO(long id, string name, string category, string characteristics, string manufacturer)
    {
        Id = id;
        Name = name;
        Category = category;
        Characteristics = characteristics;
        Manufacturer = manufacturer;
    }
    
    public ProductDTO(){}
}