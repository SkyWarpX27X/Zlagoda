namespace DBModels;

public class UserDBModel
{
    public int? UserId { get; }
    public string EmployeeId { get; }
    public string Username { get; }
    // No hashing yet!
    public string Password { get; }
    
    public UserDBModel(int? userId, string employeeId, string username, string password)
    {
        UserId = userId;
        EmployeeId = employeeId;
        Username = username;
        Password = password;
    }
}
