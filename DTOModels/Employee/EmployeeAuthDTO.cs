namespace DTOModels;

public class EmployeeAuthDTO
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    
    public EmployeeAuthDTO(long id, string username, string password, string role)
    {
        Id = id;
        Username = username;
        Password = password;
        Role = role;
    }
}