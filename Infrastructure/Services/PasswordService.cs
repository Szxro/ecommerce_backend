using Application.Common.Interfaces;
using Infrastructure.Options.Hash;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Infrastructure.Services;

public class PasswordService : IPasswordService
{
    //Using the IOptions Pattern
    private readonly HashOptions _hash;

    public PasswordService(IOptions<HashOptions> options)
    {
        _hash = options.Value;
    }

    public bool CompareUserHash(string password, string hash, byte[] salt)
    {
        //Recreating the hash with the password gave
        byte[] hashCompare = Rfc2898DeriveBytes
                          .Pbkdf2(password,
                                      salt,
                                      _hash.Iterations,
                                      HashAlgorithmName.SHA512,
                                      _hash.KeySize);

        //Comparing the hash recreated with the hash saved in the DB
        return hashCompare.SequenceEqual(Convert.FromHexString(hash));
    }

    public string GenerateUserHashAndSalt(string password, out byte[] salt)
    {
        // Generating a salt base in a count
        salt = RandomNumberGenerator.GetBytes(_hash.KeySize);


        //Generating a hash base in differents params
        byte[] userHash = Rfc2898DeriveBytes
                        .Pbkdf2(password,
                                    salt,
                                    _hash.Iterations,
                                    HashAlgorithmName.SHA512,
                                    _hash.KeySize);


        //Converting from byte[] to string
        return Convert.ToHexString(userHash);
    }
}
