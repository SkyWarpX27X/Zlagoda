using DBModels;

namespace Repositories.Sale;

public interface ISaleRepository
{
    SaleDBModel? GetSale(string upc, long receiptId);
    IEnumerable<SaleDBModel> GetSales(long? receiptId = null);
    void AddSale(SaleDBModel sale);
    
}