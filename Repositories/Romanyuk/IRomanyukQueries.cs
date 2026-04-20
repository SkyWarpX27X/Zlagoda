namespace Repositories.Romanyuk;

public interface IRomanyukQueries
{
    IEnumerable<(long cardNumber, string customerSurname, string customerName, string? customerPatronymic,
        string customerPhone, long categoryNumber, string categoryName)> GetCustomersWhoBoughtWholeCategory();
}