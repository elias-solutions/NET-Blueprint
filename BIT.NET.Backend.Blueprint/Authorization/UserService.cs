namespace BIT.NET.Backend.Blueprint.Authorization
{
    public class UserService : IUserService
    {
        public User? GetCurrentUser()
        {
            var role = Roles.Standard;
            var guid = Guid.Parse("E3D3D338-2053-4CE9-877D-DE4563FE321C");
            return new User(guid, role, $"{role}@{role}.ch", new [] { role });
        }
    }
}
