namespace DTOModels;

public class ReceiptCreateDTO
{
    public string CustomerCardId { get; set; }
    public DateTime PrintDate { get; set; }
    public List<SaleDTO> Sales { get; set; }

    public ReceiptCreateDTO(string customerCardId, DateTime printDate, List<SaleDTO> sales)
    {
        CustomerCardId = customerCardId;
        PrintDate = printDate;
        Sales = sales;
    }
}