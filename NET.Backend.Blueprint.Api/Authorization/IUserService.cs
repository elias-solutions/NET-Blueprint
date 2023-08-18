namespace NET.Backend.Blueprint.Api.Authorization;

public interface IUserService
{
    User? GetCurrentUser();
}