using DTOModels;
using Zlagoda.Test;

namespace Zlagoda.ViewModels;

public class ReceiptsVM
{
    //TODO switch to real receipt service
    //private readonly IReceiptService _receiptService;

    public IEnumerable<ReceiptDTO> Receipts { get; private set; }
    
    public IEnumerable<CustomerDTO> Customers { get; private set; }
    public IEnumerable<StoreProductDTO> StoreProducts { get; private set; }

    public string SelectedEmployee { get; set; } = "";
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    
    public bool IsCreating { get; private set; }
    public ReceiptCreateDTO? NewReceipt { get; private set; }

    public ReceiptsVM()
    {
        //_receiptService = receiptService;
        Receipts = new List<ReceiptDTO>();
        Customers = new List<CustomerDTO>();
        StoreProducts = new List<StoreProductDTO>();
    }

    public IEnumerable<string> Employees => 
        Receipts?.Select(r => r.EmployeeName).Distinct().OrderBy(e => e) ?? Enumerable.Empty<string>();

    
    //TODO: Move filter to service layer after connecting it
    public IEnumerable<ReceiptDTO> FilteredReceipts
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
        
        // TODO: Load from real services
        Customers = FakeCustomers.GetCustomers();
        StoreProducts = FakeStoreProducts.GetProducts();
    }

    public void ClearFilters()
    {
        SelectedEmployee = "";
        FromDate = null;
        ToDate = null;
    }

    public void ShowCreateNew()
    {
        NewReceipt = new ReceiptCreateDTO( "", DateTime.Now, new List<SaleDTO>());
        IsCreating = true;
    }

    public void SaveNewReceipt(ReceiptCreateDTO receipt)
    {
        IsCreating = false;
        NewReceipt = null;
        // TODO: save receipt via service
        LoadReceipts();
    }

    public void CancelCreate()
    {
        IsCreating = false;
        NewReceipt = null;
    }

    public void DeleteReceipt(string id)
    {
        // TODO: delete receipt via service
        LoadReceipts();
    }
}
