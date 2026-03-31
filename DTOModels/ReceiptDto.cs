namespace DTOModels;

public class ReceiptDto
{
    public string Id { get; set; }
    public string EmployeeName { get; set; }
    public string? CustomerName { get; set; }
    public DateTime PrintDate { get; set; }
    public decimal TotalSum { get; set; }
    public decimal Tax { get; set; }

    public ReceiptDto(string id, string employeeName, string? customerName, DateTime printDate, decimal totalSum, decimal tax)
    {
        Id = id;
        EmployeeName = employeeName;
        CustomerName = customerName;
        PrintDate = printDate;
        TotalSum = totalSum;
        Tax = tax;
    }
}