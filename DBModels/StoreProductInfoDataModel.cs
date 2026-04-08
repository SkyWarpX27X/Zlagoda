namespace DBModels;

public class StoreProductInfoDataModel(string upc, string Name, string? upcProm, long productId, decimal sellingPrice, 
    int quantity, bool promotional)
{
    public string UPC { get; set; } = upc;
    public string Name { get; set; } = Name;
    public string? UPCProm { get; set; } = upcProm;
    public long ProductId { get; set; } = productId;
    public decimal SellingPrice { get; set; } = sellingPrice;
    public int Quantity { get; set; } = quantity;
    public bool Promotional { get; set; } = promotional;
}