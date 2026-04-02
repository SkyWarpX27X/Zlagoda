using DTOModels;

namespace Zlagoda.Test;

static class FakeReceipts
{
    static List<ReceiptDto> _receipts;

    static FakeReceipts()
    {
        var sale1 = new SaleDto("1", "Кілограм щура", 100, 2);
        var sale2 = new SaleDto("2", "Газета 'Кишинівський Підмічатель'", 30, 1);
        var sale3 = new SaleDto("1", "Газета 'Кишинівський Підмічатель'", 25, 3);

        _receipts = new List<ReceiptDto>()
        {
            new ReceiptDto("1", "Бівіс", "Африкан Свиридович", new DateTime(2026, 4, 1, 12, 0, 0, 0), 275, 12.5m,new List<SaleDto>() { sale1, sale3 }),
            new ReceiptDto("2", "Баттхед", null, new DateTime(2026, 4, 1, 23, 0, 0, 0), 30, 1, new List<SaleDto>(){sale2})
        };
    }

    public static IEnumerable<ReceiptDto> GetReceipts()
    {
        return _receipts;
    }
}