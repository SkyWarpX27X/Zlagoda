using DBModels;

namespace Repositories.Receipt;

public interface IReceiptRepository
{
    ReceiptDBModel? GetReceipt(long id);
    IEnumerable<ReceiptDBModel> GetReceipts();
    void AddReceipt(ReceiptDBModel receipt);
    void DeleteReceipt(ReceiptDBModel receipt);
}