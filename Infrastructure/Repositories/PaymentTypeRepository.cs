using Application.Common.Interfaces;
using Domain;
using Infrastructure.Common;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class PaymentTypeRepository : GenericRepository<PaymentType>,IPaymentTypeRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public PaymentTypeRepository(AppDbContext context,IUnitOfWork unitOfWork) : base(context)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddDefaultPaymentTypeAndProvider()
    {
        ICollection<PaymentType> paymentTypes = new HashSet<PaymentType>()
        {
           new PaymentType(){
               TypeName = "Credit Card",
               Providers = new HashSet<Provider>()
               {
                   new Provider(){ProviderName = "MasterCard" },
                   new Provider(){ProviderName = "Visa" },
                   new Provider(){ProviderName = "Blue" },
                   new Provider(){ProviderName = "RuPay" }
               }
           },
           new PaymentType(){
               TypeName = "Digital Wallet",
               Providers = new HashSet<Provider>()
               {
                   new Provider(){ProviderName = "Stripe" },
                   new Provider(){ProviderName = "Paypal" },
                   new Provider(){ProviderName = "ApplePay" },
                   new Provider(){ProviderName = "Zelle" }
               }
           },
           new PaymentType(){
               TypeName = "Cash",
               Providers = new HashSet<Provider>()
               {
                   new Provider(){ProviderName = "Physical Payment" }
               }
           }
        };

        _context.PaymentType.AddRange(paymentTypes);

        await _unitOfWork.SaveChangesAsync();
    }
}
