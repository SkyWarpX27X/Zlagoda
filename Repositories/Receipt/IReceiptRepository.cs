using DBModels;

namespace Repositories.Receipt;

public interface IReceiptRepository
{
    ReceiptDBModel? GetReceipt(long id);
    IEnumerable<ReceiptDBModel> GetReceipts((string StartDate, string EndDate)? dates = null);
    IEnumerable<ReceiptDBModel> GetReceiptsByCashier(long employeeId, (string StartDate, string EndDate)? dates = null);
    decimal GetSumTotal((string StartDate, string EndDate) dates);
    decimal GetSumByCashier(long employeeId, (string StartDate, string EndDate) dates);
    void AddReceipt(ReceiptDBModel receipt);
    void DeleteReceipt(long id);
}