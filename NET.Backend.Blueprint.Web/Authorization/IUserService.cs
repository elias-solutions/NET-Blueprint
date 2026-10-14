namespace NET.Backend.Blueprint.Web.Authorization;

public interface IUserService
{
    User? GetCurrentUser();
}
