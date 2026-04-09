using DTOModels;

namespace Zlagoda.Test;

static class FakeReceipts
{
    static List<ReceiptDTO> _receipts;

    static FakeReceipts()
    {
        var sale1 = new SaleDTO(1, "Кілограм щура", 100, 2);
        var sale2 = new SaleDTO(2, "Газета 'Кишинівський Підмічатель'", 30, 1);
        var sale3 = new SaleDTO(1, "Газета 'Кишинівський Підмічатель'", 25, 3);

        _receipts = new List<ReceiptDTO>()
        {
            new ReceiptDTO(1, "Бівіс", "Африкан Свиридович", new DateTime(2026, 4, 1, 12, 0, 0, 0), 275, 12.5m,new List<SaleDTO>() { sale1, sale3 }),
            new ReceiptDTO(2, "Баттхед", null, new DateTime(2026, 4, 1, 23, 0, 0, 0), 30, 1, new List<SaleDTO>(){sale2})
        };
    }

    public static IEnumerable<ReceiptDTO> GetReceipts()
    {
        return _receipts;
    }
}