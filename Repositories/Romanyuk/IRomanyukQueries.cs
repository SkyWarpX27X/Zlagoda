namespace Repositories.Romanyuk;

public interface IRomanyukQueries
{
    IEnumerable<(long cardNumber, string customerSurname, string customerName, string? customerPatronymic,
        string customerPhone, long categoryNumber, string categoryName)> GetCustomersWhoBoughtWholeCategory();

    IEnumerable<(long categoryNumber, string categoryName, decimal avgPrice)> GetAvgPriceOfCategory(
        long min = 0, long max = 9999);
}