using DBModels;

namespace Repositories.Sale;

public interface ISaleRepository
{
    SaleDBModel? GetSale(string upc, long receiptId);
    IEnumerable<SaleDBModel> GetSales();
    void AddSale(SaleDBModel sale);
    
}