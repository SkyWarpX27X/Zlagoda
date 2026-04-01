namespace DTOModels;

public class EmployeeAuthDto
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    
    public EmployeeAuthDto(long id, string username, string password, string role)
    {
        Id = id;
        Username = username;
        Password = password;
        Role = role;
    }
}