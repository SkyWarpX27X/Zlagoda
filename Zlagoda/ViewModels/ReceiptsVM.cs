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
    public long? SearchReceiptId { get; set; }
    
    public bool IsCreating { get; private set; }
    public ReceiptCreateDTO? NewReceipt { get; private set; }

    public ReceiptsVM(IEmployeeService employeeService, IReceiptService receiptService, IStoreProductService storeProductService, ICustomerService customerService)
    {
        _receiptService = receiptService;
        _employeeService = employeeService;
        _storeProductService = storeProductService;
        _customerService = customerService;
        SelectedEmployee = -1;
    }
    public IEnumerable<ReceiptDTO> FilterReceipts()
    {
        if (SearchReceiptId.HasValue)
        {
            try
            {
                var receipt = _receiptService.GetReceipt(SearchReceiptId.Value);
                if (receipt != null)
                {
                    return new List<ReceiptDTO> { receipt };
                }
            }
            catch (Exception e)
            {
                return new List<ReceiptDTO>();
            }
        }

        if (SelectedEmployee != -1)
        {
            if (FromDate != null && ToDate != null)
            {
                return _receiptService.GetReceiptsByCashier(SelectedEmployee, (FromDate.Value, ToDate.Value));
            }
            return _receiptService.GetReceiptsByCashier(SelectedEmployee);
        }
        if (FromDate != null && ToDate != null)
        {
            return _receiptService.GetReceipts((FromDate.Value, ToDate.Value));
        }
        return _receiptService.GetReceipts();
    }

    public EmployeeDTO GetEmployee(string username)
    {
        return  _employeeService.GetEmployee(username);
    }

    public decimal TotalSum
    {
        get
        {
            if (SelectedEmployee != -1)
            {
                if (FromDate != null && ToDate != null)
                {
                    return _receiptService.GetReceiptsTotalSumByCashier(SelectedEmployee, (FromDate.Value, ToDate.Value));
                }
                return _receiptService.GetReceiptsTotalSumByCashier(SelectedEmployee);
            }
            if (FromDate != null && ToDate != null)
            {
                return _receiptService.GetReceiptsTotalSum((FromDate.Value, ToDate.Value));
            }
            return _receiptService.GetReceiptsTotalSum();
        }
    }
    
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
        SearchReceiptId = null;
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