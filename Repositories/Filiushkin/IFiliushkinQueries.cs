namespace Repositories.Filiushkin;

public interface IFiliushkinQueries
{
    public readonly record struct EmployeeSalesByCategoryRow(
        string Surname,
        string Name,
        string? Patronymic,
        string Category,
        decimal Total);

    public readonly record struct EmployeesSoldAllProductsRow(
        string Surname,
        string Name,
        string? Patronymic
    );
    
    IEnumerable<EmployeeSalesByCategoryRow> EmployeeSalesByCategory(DateTime start, DateTime end);
    IEnumerable<EmployeesSoldAllProductsRow> EmployeesSoldAllProducts();
}