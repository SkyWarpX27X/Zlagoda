namespace DTOModels;

public class SaleDTO
{
    public string ReceiptId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Sum => Price * Quantity;

    public SaleDTO(string receiptId, string productName, decimal price, int quantity)
    {
        ReceiptId = receiptId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
    }
}