using BC = BCrypt.Net.BCrypt;
namespace OnlineStore.Domain.Helpers
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            string passwordHash = BC.HashPassword(password);
            return passwordHash;
        }

        public static bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return BC.Verify(enteredPassword, storedPasswordHash);
        }
    }
}
