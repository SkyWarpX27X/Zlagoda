using DBModels;
using DTOModels;
using Repositories.CustomerCard;
using Repositories.Employee;
using Repositories.Product;
using Repositories.Receipt;
using Repositories.Sale;
using Repositories.StoreProduct;

namespace Services.Receipt;

public class ReceiptService : IReceiptService
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICustomerCardRepository _customerCardRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStoreProductRepository _storeProductRepository;

    public ReceiptService(
        IReceiptRepository receiptRepository,
        IEmployeeRepository employeeRepository,
        ICustomerCardRepository customerCardRepository,
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        IStoreProductRepository storeProductRepository)
    {
        _receiptRepository = receiptRepository;
        _employeeRepository = employeeRepository;
        _customerCardRepository = customerCardRepository;
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _storeProductRepository = storeProductRepository;
    }
    
    public IEnumerable<ReceiptDTO> GetReceipts((DateTime start, DateTime end)? dates = null)
    {
        (string start, string end)? argumentDates = null;
        if (dates is not null)
            argumentDates = DateRangeToStrings(dates.Value.start, dates.Value.end);
        foreach (var receipt in _receiptRepository.GetReceipts(argumentDates))
        {
            yield return ReceiptDbToDtoModel(receipt);
        }
    }

    public IEnumerable<ReceiptDTO> GetReceiptsByCashier(long cashierId, (DateTime start, DateTime end)? dates = null)
    {
        (string start, string end)? argumentDates = null;
        if (dates is not null)
            argumentDates = DateRangeToStrings(dates.Value.start, dates.Value.end);
        foreach (var receipt in _receiptRepository.GetReceiptsByCashier(cashierId, argumentDates))
        {
            yield return ReceiptDbToDtoModel(receipt);
        }
    }

    public ReceiptDTO GetReceipt(long id)
    {
        var receipt = _receiptRepository.GetReceipt(id)
                      ?? throw new InvalidDataException("Receipt does not exist");
        
        return ReceiptDbToDtoModel(receipt);
    }

    public decimal GetReceiptsTotalSum((DateTime start, DateTime end)? dates)
    {
        (string start, string end)? argumentDates = null;
        if (dates is not null)
            argumentDates = DateRangeToStrings(dates.Value.start, dates.Value.end);
        return _receiptRepository.GetSumTotal(argumentDates);
    }

    public decimal GetReceiptsTotalSumByCashier(long cashierId, (DateTime start, DateTime end)? dates)
    {
        (string start, string end)? argumentDates = null;
        if (dates is not null)
            argumentDates = DateRangeToStrings(dates.Value.start, dates.Value.end);
        return _receiptRepository.GetSumByCashier(cashierId, argumentDates);
    }

    public void AddReceipt(ReceiptCreateDTO receipt)
    {
        Validation.ValidateReceiptCreate(receipt);
        if (string.IsNullOrEmpty(receipt.CustomerCardId))
            receipt.CustomerCardId = null;
        
        decimal total = 0;
        foreach (var sale in receipt.Sales)
            total += sale.Price * sale.Quantity;

        ReceiptDBModel receiptResult = new(
            receipt.EmployeeId,
            receipt.CustomerCardId,
            receipt.PrintDate.ToString("yyyy-MM-dd HH:mm:ss"),
            total,
            total * 0.2m);
        long id = _receiptRepository.AddReceipt(receiptResult);
        
        foreach (var sale in receipt.Sales)
        {
            var storeProduct = _storeProductRepository.GetStoreProduct(sale.ProductUPC)
                    ?? throw new InvalidDataException("Store product doesn't exist");
            
            SaleDBModel saleResult = new(sale.ProductUPC, id, sale.Quantity, sale.Price);
            if (saleResult.ProductQuantity > storeProduct.Quantity)
                saleResult.ProductQuantity = storeProduct.Quantity;
            _saleRepository.AddSale(saleResult);

            StoreProductDBModel storeProductResult = new(
                storeProduct.UPC,
                storeProduct.UPCProm,
                storeProduct.ProductId,
                storeProduct.SellingPrice,
                storeProduct.Quantity - saleResult.ProductQuantity,
                storeProduct.Promotional);
            _storeProductRepository.UpdateStoreProduct(storeProductResult);
        }
    }

    public void DeleteReceipt(long id)
    {
        _receiptRepository.DeleteReceipt(id);
    }

    private ReceiptDTO ReceiptDbToDtoModel(ReceiptDBModel receipt)
    {
        var employee = _employeeRepository.GetEmployee(receipt.EmployeeId)
                       ?? throw new InvalidDataException("Employee does not exist");
        
        string? customerName = null;
        if (!string.IsNullOrEmpty(receipt.CardNumber))
        {
            var customer = _customerCardRepository.GetCustomer(receipt.CardNumber)
                           ?? throw new InvalidDataException("Customer does not exist");
            customerName = $"{customer.Surname} {customer.Name} {customer.Patronymic}";
        }
        
        List<SaleDTO> sales = new();
        foreach (var sale in _saleRepository.GetSales(receipt.Id))
        {
            var storeProduct = _storeProductRepository.GetStoreProduct(sale.UPC)
                               ?? throw new InvalidDataException("Store product doesn't exist");
            var product = _productRepository.GetProduct(storeProduct.ProductId)
                          ?? throw new InvalidDataException("Product doesn't exist");
            sales.Add(new(sale.ReceiptId, product.Name, sale.SellingPrice, sale.ProductQuantity));
        }

        return new(
            receipt.Id,
            $"{employee.Surname} {employee.Name} {employee.Patronymic}",
            customerName,
            DateTime.Parse(receipt.PrintDate),
            receipt.TotalSum,
            receipt.Vat,
            sales);
    }

    private (string, string) DateRangeToStrings(DateTime start, DateTime end)
    {
        var a = start.ToString("yyyy-MM-dd HH:mm:ss");
        var b = end.ToString("yyyy-MM-dd HH:mm:ss");
        return (a, b);
    }
}