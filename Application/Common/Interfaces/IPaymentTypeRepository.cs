namespace Application.Common.Interfaces;

public interface IPaymentTypeRepository
{
    Task AddDefaultPaymentTypeAndProvider();
}
