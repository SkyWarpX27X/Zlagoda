using DBModels;

namespace Repositories.Receipt;

public interface IReceiptRepository
{
    ReceiptDBModel? GetReceipt(long id);
    IEnumerable<ReceiptDBModel> GetReceipts();
}