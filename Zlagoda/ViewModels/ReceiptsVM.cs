using DTOModels;
using Zlagoda.Test;

namespace Zlagoda.ViewModels;

public class ReceiptsVM
{
    //TODO switch to real receipt service
    //private readonly IReceiptService _receiptService;

    public IEnumerable<ReceiptDto> Receipts { get; set; }

    public string SelectedEmployee { get; set; } = "";
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    public ReceiptsVM()
    {
        //_receiptService = receiptService;
        Receipts = new List<ReceiptDto>();
    }

    public IEnumerable<string> Employees => 
        Receipts?.Select(r => r.EmployeeName).Distinct().OrderBy(e => e) ?? Enumerable.Empty<string>();

    
    //TODO: Move filter to service layer after connecting it
    public IEnumerable<ReceiptDto> FilteredReceipts
    {
        get
        { 
            var query = Receipts.AsQueryable();

            if (!string.IsNullOrEmpty(SelectedEmployee))
            {
                query = query.Where(r => r.EmployeeName == SelectedEmployee);
            }

            if (FromDate.HasValue)
            {
                query = query.Where(r => r.PrintDate.Date >= FromDate.Value.Date);
            }

            if (ToDate.HasValue)
            {
                query = query.Where(r => r.PrintDate.Date <= ToDate.Value.Date);
            }

            return query.OrderByDescending(r => r.PrintDate);
        }
    }

    public void LoadReceipts()
    {
        // Receipts = _receiptService.GetReceipts();
        Receipts = FakeReceipts.GetReceipts();
    }

    public void ClearFilters()
    {
        SelectedEmployee = "";
        FromDate = null;
        ToDate = null;
    }

    public void CreateReceipt()
    {
        // TODO: Implement create
    }

    public void DeleteReceipt(string id)
    {
        // TODO: Implement delete
    }
}