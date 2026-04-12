using DTOModels;

namespace Services.Receipt;

public interface IReceiptService
{
    IEnumerable<ReceiptDTO> GetReceipts((DateTime start, DateTime end)? dates = null);
    IEnumerable<ReceiptDTO> GetReceiptsByCashier(long cashierId, (DateTime start, DateTime end)? dates = null);
    ReceiptDTO GetReceipt(long id);

    decimal GetReceiptsTotalSum((DateTime start, DateTime end)? dates = null);
    decimal GetReceiptsTotalSumByCashier(long cashierId, (DateTime start, DateTime end)? dates = null);
    
    void AddReceipt(ReceiptCreateDTO receipt);
    void DeleteReceipt(long id);
}