using Application.Common.Interfaces;

namespace Infrastructure.Services;

public class DateService : IDateService
{
    public DateTime NowUTC => DateTime.UtcNow;
}
