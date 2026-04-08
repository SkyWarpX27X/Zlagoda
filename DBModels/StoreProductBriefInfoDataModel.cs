namespace DBModels;

public class StoreProductBriefInfoDataModel(string upc, decimal sellingPrice, int quantity, string name, string characteristics)
{
    public string UPC { get; set; } = upc;
    public decimal SellingPrice { get; set; } = sellingPrice;
    public int Quantity { get; set; } = quantity;
    public string Name { get; set; } = name;
    public string Characteristics { get; set; } = characteristics;
}