using Repositories;

namespace Zlagoda.ViewModels;

public class ProductCountVM
{
    private readonly IOstapchukQueries _repository;
    
    public string IsPromotional { get; set; }
    public IEnumerable<(long CategoryId, string CategoryName, int ProductCount)> ProductCounts => _repository.GetProductCountByCategories(IsPromotional == "prom");

    public ProductCountVM(IOstapchukQueries ostapchukQueries)
    {
        _repository = ostapchukQueries;
        IsPromotional = "non-prom";
    }
}