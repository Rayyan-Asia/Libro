namespace Application.Services
{
    public interface IPasswordHasher
    {
        string ComputeHash(string password, string salt, int iteration = 4);
        string GenerateSalt();
        bool VerifyPassword(string password, string salt, string passwordToCompare, int iteration = 4);
    }
}