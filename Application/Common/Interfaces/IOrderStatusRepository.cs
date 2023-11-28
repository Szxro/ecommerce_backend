namespace Application.Common.Interfaces;

public interface IOrderStatusRepository 
{
    Task AddDefaultOrderStatusAsync();

    bool CheckHaveAnyData();
}
