namespace iParking.Infrastructure.Security
{
    public interface ISecurityHash
    {
        string GenerateHash(string password);
        bool VerifyHash(string password, string storedHash);
    }
}
