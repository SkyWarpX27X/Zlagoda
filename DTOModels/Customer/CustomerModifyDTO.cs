namespace DTOModels;

public class CustomerModifyDTO
{
    public string? CardId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    public string Phone { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? ZipCode { get; set; }
    public int Percent { get; set; }

    public CustomerModifyDTO(string? cardId, string firstName, string lastName, string? patronymic, string phone, string? city, string? street, string? zipCode, int percent)
    {
        CardId = cardId;
        FirstName = firstName;
        LastName = lastName;
        Patronymic = patronymic;
        Phone = phone;
        City = city;
        Street = street;
        ZipCode = zipCode;
        Percent = percent;
    }
    
    public CustomerModifyDTO(){}
}