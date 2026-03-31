using DBModels;

namespace Storage;

public interface IStorageContext
{
    UserDBModel? GetUser(int userId);
}