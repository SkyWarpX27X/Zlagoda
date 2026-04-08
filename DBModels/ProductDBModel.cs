namespace DBModels;

public class ProductDBModel(long id, long categoryId, string name, string characteristics, string manufacturer)
{
    public long Id { get; set; } = id;
    public long CategoryId { get; set; } = categoryId;
    public string Name { get; set; } = name;
    public string Characteristics  { get; set; } = characteristics;
    public string Manufacturer { get; set; } = manufacturer;

    public ProductDBModel(long categoryId, string name, string characteristics, string manufacturer) : this(0,
        categoryId, name, characteristics, manufacturer) {}
}