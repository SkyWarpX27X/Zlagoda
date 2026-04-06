namespace DBModels;

public class ReceiptDBModel
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public string? CardNumber { get; set; }
    public string PrintDate { get; set; }
    public decimal TotalSum { get; set; }
    public decimal Vat { get; set; }

    public ReceiptDBModel(long id, long employeeId, string? cardNumber, string printDate, decimal totalSum,
        decimal vat)
    {
        Id = id;
        EmployeeId = employeeId;
        CardNumber = cardNumber;
        PrintDate = printDate;
        TotalSum = totalSum;
        Vat = vat;
    }
    
}