namespace DTOModels;

public class ReceiptCreateDTO
{
    public long EmployeeId { get; set; }
    public string CustomerCardId { get; set; }
    public DateTime PrintDate { get; set; }
    public List<SaleCreateDTO> Sales { get; set; }

    public ReceiptCreateDTO(string customerCardId, DateTime printDate, List<SaleCreateDTO> sales)
    {
        CustomerCardId = customerCardId;
        PrintDate = printDate;
        Sales = sales;
    }
}