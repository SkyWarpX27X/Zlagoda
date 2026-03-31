namespace DTOModels;

public class CustomerDto
{
    public string CardId { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public int Percent { get; set; }

    public CustomerDto(string cardId, string fullName, string phone, string address, int percent)
    {
        CardId = cardId;
        FullName = fullName;
        Phone = phone;
        Address = address;
        Percent = percent;
    }
}