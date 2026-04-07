namespace DTOModels;

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Characteristics { get; set; }
    public string Manufacturer { get; set; }

    public ProductDTO(int id, string name, string category, string characteristics, string manufacturer)
    {
        Id = id;
        Name = name;
        Category = category;
        Characteristics = characteristics;
        Manufacturer = manufacturer;
    }
    
    public ProductDTO(){}
}