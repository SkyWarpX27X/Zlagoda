namespace DTOModels;

public class StoreProductModifyDTO
{
    public string Upc { get; set; }
    public string? UpcProm { get; set; }
    public long ProductId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool Promotional { get; set; }
    
    public StoreProductModifyDTO(string upc, long productId, decimal price, int quantity, string? upcProm = null, bool promotional = true)
    {
        Upc = upc;
        Price = price;
        Quantity = quantity;
        UpcProm = upcProm;
        Promotional = promotional;
    }
}