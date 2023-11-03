using Application.Common.Interfaces;

namespace Infrastructure.Services;

public class DateService : IDateService
{
    public DateTime NowUTC => DateTime.UtcNow;

    public DateTime TimeStampToUTCDate(long timeStamp)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timeStamp).ToUniversalTime();
    }
}
