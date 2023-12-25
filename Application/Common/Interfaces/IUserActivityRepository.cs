using Domain;

namespace Application.Common.Interfaces;

public interface IUserActivityRepository
{
    Task<bool> IsUserAlreadyRegisterByUsernameAsync(string username);

    Task InsertUserActivity(User user, DateTime loggedIn, CancellationToken cancellationToken = default);

    Task<int?> UpdateUserActivityOfflinePropertyByUsername(string username, bool userState);

    Task<int?> UpdateUserLoggedDateInByUsername(string username, DateTime dateTime);
}
