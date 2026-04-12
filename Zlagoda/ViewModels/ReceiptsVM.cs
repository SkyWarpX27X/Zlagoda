using DTOModels;
using Services.Employee;
using Services.Customer;
using Services.ProductStore;
using Services.Receipt;

namespace Zlagoda.ViewModels;

public class ReceiptsVM
{
    private readonly IReceiptService _receiptService;
    private readonly IEmployeeService _employeeService;
    private readonly IStoreProductService _storeProductService;
    private readonly ICustomerService _customerService;

    public IEnumerable<ReceiptDTO> Receipts => FilterReceipts();
    
    public IEnumerable<CustomerDTO> Customers { get; private set; }
    public IEnumerable<StoreProductDTO> StoreProducts { get; private set; }
    public IEnumerable<EmployeeDTO> Employees { get; private set; }
    public string? ErrorMessage { get; private set; }
    
    public long SelectedEmployee { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    
    public bool IsCreating { get; private set; }
    public ReceiptCreateDTO? NewReceipt { get; private set; }

    public ReceiptsVM(IEmployeeService employeeService, IReceiptService receiptService, IStoreProductService storeProductService, ICustomerService customerService)
    {
        _receiptService = receiptService;
        _employeeService = employeeService;
        _storeProductService = storeProductService;
        _customerService = customerService;
    }
    public IEnumerable<ReceiptDTO> FilterReceipts()
    {
        //TODO uncomment when null value for dates will be ready
        if (SelectedEmployee != -1)
        {
            if (FromDate != null && ToDate != null)
            {
                TotalSum = _receiptService.GetReceiptsTotalSumByCashier(SelectedEmployee, (FromDate.Value, ToDate.Value));
                return _receiptService.GetReceiptsByCashier(SelectedEmployee, (FromDate.Value, ToDate.Value));
            }
            //TotalSum = _receiptService.GetReceiptsTotalSumByCashier(SelectedEmployee);
            return _receiptService.GetReceiptsByCashier(SelectedEmployee);
        }
        if (FromDate != null && ToDate != null)
        {
            TotalSum = _receiptService.GetReceiptsTotalSum((FromDate.Value, ToDate.Value));
            return _receiptService.GetReceipts((FromDate.Value, ToDate.Value));
        }
        //TotalSum = _receiptService.GetReceiptsTotalSum();
        return _receiptService.GetReceipts();
    }

    public EmployeeDTO GetEmployee(string username)
    {
        return  _employeeService.GetEmployee(username);
    }
    
    public decimal TotalSum { get; private set; }
    
    public void LoadData()
    {
        Customers = _customerService.GetCustomers();
        StoreProducts = _storeProductService.GetStoreProducts();
        Employees = _employeeService.GetEmployees(true);
    }

    public void ClearFilters()
    {
        FromDate = null;
        ToDate = null;
    }

    public void ShowCreateNew()
    {
        NewReceipt = new ReceiptCreateDTO( "", DateTime.Now, new List<SaleCreateDTO>());
        IsCreating = true;
        ErrorMessage = null;
    }
    
    public void ClearError()
    {
        ErrorMessage = null;
    }

    public void SaveNewReceipt(ReceiptCreateDTO receipt)
    {
        try
        {
            _receiptService.AddReceipt(receipt);
            IsCreating = false;
            NewReceipt = null;
            ErrorMessage = null;
        }
        catch (InvalidDataException e)
        {
            ErrorMessage = e.Message;
        }
    }

    public void CancelCreate()
    {
        IsCreating = false;
        NewReceipt = null;
        ErrorMessage = null;
    }

    public void DeleteReceipt(long id)
    {
        _receiptService.DeleteReceipt(id);
    }
}