namespace Application.Common.Interfaces;

public interface IShippingMethodRepository
{
    Task AddDefaultShippingMethodsAsnyc();

    bool CheckHaveAnyData();
}
