using DTOModels;

namespace Services.Receipt;

public interface IReceiptService
{
    IEnumerable<ReceiptDTO> GetReceipts((DateOnly start, DateOnly end)? dates = null);
    IEnumerable<ReceiptDTO> GetReceiptsByCashier(long cashierId, (DateOnly start, DateOnly end)? dates = null);
    ReceiptDTO GetReceipt(long id);

    decimal GetReceiptsTotalSum((DateOnly start, DateOnly end) dates);
    decimal GetReceiptsTotalSumByCashier(long cashierId, (DateOnly start, DateOnly end) dates);
    
    void AddReceipt(ReceiptCreateDTO receipt);
    void DeleteReceipt(long id);
}