using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.Extensions;

namespace Infrastructure.Persistence;

public class AppDbInitializer : IAppDbInitializer
{
    private readonly ILogger<AppDbInitializer> _logger;
    private readonly AppDbContext _context;
    private readonly IRoleRepository _roleRepository;
    private readonly IPaymentTypeRepository _paymentTypeRepository;
    private readonly IOrderStatusRepository _orderStatusRepository;
    private readonly IShippingMethodRepository _shippingMethodRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly ICategoryRepository _categoryRepository;

    public AppDbInitializer(
        ILogger<AppDbInitializer> logger,
        AppDbContext context,
        IRoleRepository roleRepository,
        IPaymentTypeRepository paymentTypeRepository,
        IOrderStatusRepository orderStatusRepository,
        IShippingMethodRepository shippingMethodRepository,
        ICountryRepository countryRepository,
        ICategoryRepository categoryRepository)
    {
        _logger = logger;
        _context = context;
        _roleRepository = roleRepository;
        _paymentTypeRepository = paymentTypeRepository;
        _orderStatusRepository = orderStatusRepository;
        _shippingMethodRepository = shippingMethodRepository;
        _countryRepository = countryRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task ConnectAsync()
    {
        try 
        {
            await _context.Database.CanConnectAsync(); // Determines whether the database is avaliable
        } catch (Exception ex)
        {
            _logger.ConnectDatabaseError(_context.Database.ProviderName, ex.Message);
        }
    }

    public async Task MigrateAsync()
    {
        try
        {
            await _context.Database.MigrateAsync(); // Looking for any new migration to do it automatically the api start
        }
        catch (Exception ex)
        {
            _logger.MigrateDatabaseError(_context.Database.ProviderName, ex.Message);

            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            // In this case can be use Task.WhenAll but the dbcontext instance cant be shared with the differents thread (dbcontex is not thread-safe)
            await TrySeedRolesAndScope();

            await TrySeedCountry();

            await TrySeedShippingMethod();

            await TrySeedPaymentTypeAndProvider();

            await TrySeedOrderStatus();

            await TrySeedCategories();

        } catch(Exception ex)
        {
            _logger.SeedDatabaseError(ex.Message);

            throw;
        }
    }

    private async Task TrySeedRolesAndScope()
    {
        if (!_roleRepository.CheckHaveAnyData())
        {
            await _roleRepository.AddDefaultRolesAndScope();
        }
    }

    private async Task TrySeedPaymentTypeAndProvider()
    {
        if (!_context.PaymentType.Any()
            && !_context.Provider.Any())
        {
            await _paymentTypeRepository.AddDefaultPaymentTypeAndProvider();
        }
    }

    private async Task TrySeedOrderStatus()
    {
        if (!_orderStatusRepository.CheckHaveAnyData())
        {
            await _orderStatusRepository.AddDefaultOrderStatus();
        }
    }

    private async Task TrySeedShippingMethod()
    {
        if (!_shippingMethodRepository.CheckHaveAnyData())
        {
            await _shippingMethodRepository.AddDefaultShippingMethods();
        }
    }

    private async Task TrySeedCountry()
    {
        if (!_countryRepository.CheckHaveAnyData())
        {
            await _countryRepository.AddDefaultCountries();
        }
    }

    private async Task TrySeedCategories()
    {
        if (!_categoryRepository.CheckHaveAnyData())
        {
            await _categoryRepository.AddDefaultCategories();
        }
    }
}
