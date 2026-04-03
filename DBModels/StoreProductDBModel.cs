namespace DBModels;

public class StoreProductDBModel
{
    public string UPC { get; set; }
    public string? UPCProm { get; set; }
    public long ProductId { get; set; }
    public decimal SellingPrice { get; set; }
    public int Quantity { get; set; }
    public bool Promotional { get; set; }

    public StoreProductDBModel(string upc, string? upcProm, long productId, decimal sellingPrice, int quantity,
        bool promotional)
    {
        UPC = upc;
        UPCProm = upcProm;
        ProductId = productId;
        SellingPrice = sellingPrice;
        Quantity = quantity;
        Promotional = promotional;
    }
}