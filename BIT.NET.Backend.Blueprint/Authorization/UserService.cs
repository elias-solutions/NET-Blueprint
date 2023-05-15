namespace BIT.NET.Backend.Blueprint.Authorization
{
    public class UserService : IUserService
    {
        public User? GetCurrentUser()
        {
            var guid = Guid.Parse("E3D3D338-2053-4CE9-877D-DE4563FE321C");
            return new User(guid, Roles.Admin, $"{Roles.Admin}@{Roles.Admin}.ch", new []{Roles.Admin});
        }
    }
}
