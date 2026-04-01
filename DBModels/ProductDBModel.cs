namespace DBModels;

public class ProductDBModel
{
    // Leaving handling the no Id case until I work on creation
    public long Id { get; set; }
    public long CategoryId { get; set; }
    public string Name { get; set; }
    public string Characteristics  { get; set; }
    public string Manufacturer { get; set; }

    public ProductDBModel(long id, long categoryId, string name, string characteristics, string manufacturer)
    {
        Id = id;
        CategoryId = categoryId;
        Name = name;
        Characteristics = characteristics;
        Manufacturer = manufacturer;
    }
}