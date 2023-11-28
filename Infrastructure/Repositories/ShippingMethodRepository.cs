using Application.Common.Interfaces;
using Domain;
using Infrastructure.Common;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class ShippingMethodRepository : GenericRepository<ShippingMethod>, IShippingMethodRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public ShippingMethodRepository(AppDbContext context,IUnitOfWork unitOfWork) : base(context)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddDefaultShippingMethodsAsnyc()
    {
        ICollection<ShippingMethod> shippingMethods = new HashSet<ShippingMethod>()
        {
            new ShippingMethod(){MethodName = "TruckLoad (Local)",Price = 150 },
            new ShippingMethod(){MethodName = "Air Freight (International)", Price = 200 },
            new ShippingMethod(){MethodName = "Ocean Freight (International)", Price = 180}
        };

        _context.ShippingMethod.AddRange(shippingMethods);

        await _unitOfWork.SaveChangesAsync();
    }
}
