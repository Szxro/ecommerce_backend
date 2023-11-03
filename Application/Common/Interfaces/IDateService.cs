namespace Application.Common.Interfaces;

public interface IDateService
{
    DateTime NowUTC { get; }

    DateTime TimeStampToUTCDate(long timeStamp);
}
