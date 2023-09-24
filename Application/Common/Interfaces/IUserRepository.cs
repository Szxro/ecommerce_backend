using Domain;

namespace Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserById(int id);

    Task<User?> GetUserByUsername(string username);

    void Add(User newUser);

    void ChangeTrackerToUnchanged(User user);
}
