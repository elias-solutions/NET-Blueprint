namespace BIT.NET.Backend.Blueprint.Authorization;

public interface IUserService
{
    User? GetCurrentUser();
}