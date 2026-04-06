namespace DBModels;

public class StoreProductDBModel(string upc, string? upcProm, long productId, decimal sellingPrice, int quantity, 
    bool promotional)
{
    public string UPC { get; set; } = upc;
    public string? UPCProm { get; set; } = upcProm;
    public long ProductId { get; set; } = productId;
    public decimal SellingPrice { get; set; } = sellingPrice;
    public int Quantity { get; set; } = quantity;
    public bool Promotional { get; set; } = promotional;
}