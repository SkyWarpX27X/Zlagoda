using Repositories.Filiushkin;

namespace Zlagoda.ViewModels;

public class EmployeeSalesByCategoryVM
{
    private readonly IFiliushkinQueries _repository;
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public IEnumerable<IFiliushkinQueries.EmployeeSalesByCategoryRow> EmployeeSalesByCategory
        => _repository.EmployeeSalesByCategory(StartDate, EndDate);
    
    public EmployeeSalesByCategoryVM(IFiliushkinQueries repository)
    {
        _repository = repository;
        StartDate = DateTime.Now;
        EndDate = DateTime.Now;
    }
}