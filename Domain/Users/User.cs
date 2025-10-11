using SGO.Domain.Common;

namespace Domain.Users
{
    public sealed class User : Entity
    {
        public int Id { get; set; }
        public int Matricula { get; private set; }
        public string Username { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;
        public string Role { get; private set; } = default!;
    }
}
