namespace DBModels;

public class ReceiptDBModel(long id, long employeeId, string? cardNumber, string printDate, decimal totalSum, decimal vat)
{
    public long Id { get; set; } = id;
    public long EmployeeId { get; set; } = employeeId;
    public string? CardNumber { get; set; } = cardNumber;
    public string PrintDate { get; set; } = printDate;
    public decimal TotalSum { get; set; } = totalSum;
    public decimal Vat { get; set; } = vat;
}