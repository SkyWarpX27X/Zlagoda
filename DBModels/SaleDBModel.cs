namespace DBModels;

public class SaleDBModel(string upc, long receiptId, int productQuantity, decimal sellingPrice)
{
    public string UPC { get; set; } = upc;
    public long ReceiptId { get; set; } = receiptId;
    public int ProductQuantity { get; set; } = productQuantity;
    public decimal SellingPrice { get; set; } = sellingPrice;
}