namespace iParking.Infrastructure.Security
{
    public class SecurityHash: ISecurityHash
    {
        public string GenerateHash(string password)
        {
            using var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(password, Array.Empty<byte>(), 10000, System.Security.Cryptography.HashAlgorithmName.SHA256);
            byte[] hashBytes = pbkdf2.GetBytes(32);
            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyHash(string password, string storedHash)
        {
            using var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(
                password,
                Array.Empty<byte>(),
                100000, 
                System.Security.Cryptography.HashAlgorithmName.SHA256 
            );

            byte[] hashBytes = pbkdf2.GetBytes(32);
            return Convert.ToBase64String(hashBytes) == storedHash;
        }
    }
}
