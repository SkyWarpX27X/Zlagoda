namespace Repositories;

public interface IOstapchukQueries
{
    IEnumerable<(long, string, int)> GetProductCountByCategories(bool prom);
    IEnumerable<(long, string, string, string?, decimal)> GetExperiencedCashiers();
}