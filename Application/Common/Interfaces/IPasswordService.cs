namespace Application.Common.Interfaces;

public interface IPasswordService
{
    string GenerateUserHashAndSalt(string password,out byte[] salt);

    bool CompareUserHash(string password, string hash, byte[] salt);
}
