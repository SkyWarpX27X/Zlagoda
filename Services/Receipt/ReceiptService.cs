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
    
    public IEnumerable<ReceiptDTO> GetReceipts((DateOnly start, DateOnly end)? dates = null)
    {
        (string start, string end)? argumentDates = null;
        if (dates is not null)
            argumentDates = new(dates.Value.start.ToShortDateString(), dates.Value.end.ToShortDateString());
        foreach (var receipt in _receiptRepository.GetReceipts(argumentDates))
        {
            yield return ReceiptDbToDtoModel(receipt);
        }
    }

    public IEnumerable<ReceiptDTO> GetReceiptsByCashier(long cashierId, (DateOnly start, DateOnly end)? dates = null)
    {
        (string start, string end)? argumentDates = null;
        if (dates is not null)
            argumentDates = new(dates.Value.start.ToShortDateString(), dates.Value.end.ToShortDateString());
        foreach (var receipt in _receiptRepository.GetReceiptsByCashier(cashierId, argumentDates))
        {
            yield return ReceiptDbToDtoModel(receipt);
        }
    }

    public ReceiptDTO GetReceipt(long id)
    {
        var receipt = _receiptRepository.GetReceipt(id);
        if (receipt is null) throw new InvalidDataException($"Receipt {id} does not exist");
        
        return ReceiptDbToDtoModel(receipt);
    }

    public decimal GetReceiptsTotalSum((DateOnly start, DateOnly end) dates)
    {
        (string, string) argumentDates = new(dates.start.ToShortDateString(), dates.end.ToShortDateString());
        return _receiptRepository.GetSumTotal(argumentDates);
    }

    public decimal GetReceiptsTotalSumByCashier(long cashierId, (DateOnly start, DateOnly end) dates)
    {
        (string, string) argumentDates = new(dates.start.ToShortDateString(), dates.end.ToShortDateString());
        return _receiptRepository.GetSumByCashier(cashierId, argumentDates);
    }

    public void AddReceipt(ReceiptCreateDTO receipt)
    {
        ValidateReceipt(receipt);
        decimal total = 0;
        foreach (var sale in receipt.Sales)
        {
            ValidateSale(sale);
            total += sale.Price * sale.Quantity;
        }
        
        long id = _receiptRepository.AddReceipt(new(
            receipt.EmployeeId,
            receipt.CustomerCardId,
            $"{receipt.PrintDate.ToShortDateString()} {receipt.PrintDate.ToShortTimeString()}",
            total,
            total * 0.2m
            ));
        foreach (var sale in receipt.Sales)
        {
            _saleRepository.AddSale(new(
                sale.ProductUPC,
                id,
                sale.Quantity,
                sale.Price
                ));
        }
    }

    public void DeleteReceipt(long id)
    {
        _receiptRepository.DeleteReceipt(id);
    }

    private ReceiptDTO ReceiptDbToDtoModel(ReceiptDBModel receipt)
    {
        var employee = _employeeRepository.GetEmployee(receipt.EmployeeId);
        if (employee is null) throw new InvalidDataException($"Employee {receipt.EmployeeId} does not exist");
        
        string? customerName = null;
        if (!string.IsNullOrEmpty(receipt.CardNumber))
        {
            var customer = _customerCardRepository.GetCustomer(receipt.CardNumber);
            if (customer is null) throw new InvalidDataException($"Customer {receipt.CardNumber} does not exist");
            customerName = customer.Name;
        }
        
        List<SaleDTO> sales = new();
        foreach (var sale in _saleRepository.GetSales(receipt.Id))
        {
            var storeProduct = _storeProductRepository.GetStoreProduct(sale.UPC);
            if (storeProduct is null) throw new InvalidDataException($"Store product {sale.UPC} doesn't exist");
            var product = _productRepository.GetProduct(storeProduct.ProductId);
            if (product is null) throw new InvalidDataException($"Product {storeProduct.ProductId} doesn't exist");
            sales.Add(new(sale.ReceiptId, product.Name, sale.SellingPrice, sale.ProductQuantity));
        }

        return new(
            receipt.Id,
            employee.Name,
            customerName,
            DateTime.Parse(receipt.PrintDate),
            receipt.TotalSum,
            receipt.Vat,
            sales);
    }

    private void ValidateReceipt(ReceiptCreateDTO receipt)
    {
        if (_customerCardRepository.GetCustomer(receipt.CustomerCardId) is null)
            throw new InvalidDataException($"Customer card {receipt.CustomerCardId} does not exist");
    }

    private void ValidateSale(SaleCreateDTO sale)
    {
        if (string.IsNullOrEmpty(sale.ProductUPC))
            throw new InvalidDataException("UPC is required");
        if (_storeProductRepository.GetStoreProduct(sale.ProductUPC) is null)
            throw new InvalidDataException($"Product {sale.ProductUPC} doesn't exist");
        if (sale.Price < 0) throw new InvalidDataException("Price can't be negative");
        if (sale.Quantity < 0) throw new InvalidDataException("Quantity can't be negative");
    }
}