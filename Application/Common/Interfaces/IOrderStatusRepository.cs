namespace Application.Common.Interfaces;

public interface IOrderStatusRepository 
{
    Task AddDefaultOrderStatus();

    bool CheckHaveAnyData();
}
