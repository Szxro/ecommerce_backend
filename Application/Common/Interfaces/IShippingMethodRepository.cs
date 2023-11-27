namespace Application.Common.Interfaces;

public interface IShippingMethodRepository
{
    Task AddDefaultShippingMethods();

    bool CheckHaveAnyData();
}
