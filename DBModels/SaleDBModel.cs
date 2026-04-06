namespace DBModels;

public class SaleDBModel
{
    public string UPC { get; set; }
    public long ReceiptId { get; set; }
    public int ProductQuantity { get; set; }
    public decimal SellingPrice { get; set; }

    public SaleDBModel(string upc, long receiptId, int productQuantity, decimal sellingPrice)
    {
        UPC = upc;
        ReceiptId = receiptId;
        ProductQuantity = productQuantity;
        SellingPrice = sellingPrice;
    }
}