using Application.Common.Interfaces;
using Domain;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserActivityRepository : GenericRepository<UserActivity>, IUserActivityRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public UserActivityRepository(AppDbContext context,
                                  IUnitOfWork unitOfWork) : base(context)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> IsUserAlreadyRegisterByUsernameAsync(string username)
    {
        return await CheckPropertyExistAsync(user => user.User.Username == username, new List<string>() { "User" });
    }

    public async Task InsertUserActivity(User user, DateTime loggedIn, CancellationToken cancellationToken = default)
    {
        UserActivity userActivity = new()
        {
            User = user,
            LoggedDate = loggedIn,
        };

        ChangeEntityContextTracker(userActivity.User, EntityState.Unchanged);

        Add(userActivity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<int?> UpdateUserActivityOfflinePropertyByUsername(string username, bool userState)
    {
        return await UpdateByAsync(x => x.User.Username == username ,x => x.SetProperty(x => x.IsOffline,userState), 
                                                new List<string>() {"User"});
    }

    public async Task<int?> UpdateUserLoggedDateInByUsername(string username,DateTime dateTime)
    {
        return await UpdateByAsync(x => x.User.Username == username, x => x.SetProperty(x => x.LoggedDate, dateTime),
                                  new List<string>() { "User" });
    }
}
