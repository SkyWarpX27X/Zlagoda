namespace DBModels;

public class EmployeeDBModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }

    public EmployeeDBModel(int id, string username, string password, string role)
    {
        Id = id;
        Username = username;
        Password = password;
        Role = role;
    }
}