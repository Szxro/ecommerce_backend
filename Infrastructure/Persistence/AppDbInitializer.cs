using Application.Common.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.Extensions;

namespace Infrastructure.Persistence;

public class AppDbInitializer : IAppDbInitializer
{
    private readonly ILogger<AppDbInitializer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppDbContext _context;
    private readonly IRoleRepository _roleRepository;

    public AppDbInitializer(
        ILogger<AppDbInitializer> logger,
        IUnitOfWork unitOfWork,
        AppDbContext context,
        IRoleRepository roleRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _context = context;
        _roleRepository = roleRepository;
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

    public async Task EnsuredDatabaseCreated()
    {
        try 
        {
            await _context.Database.EnsureCreatedAsync(); // check the database have any table and the database exists

        } catch (Exception ex)
        {
            _logger.DatabaseCreatedError(_context.Database.ProviderName, ex.Message);
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
            await TrySeedRolesAndScopeAsync();

        } catch (Exception ex)
        {
            _logger.SeedDatabaseError(ex.Message);

            throw;
        }
    }

    private async Task TrySeedRolesAndScopeAsync()
    {
        if (!_context.Role.Any())
        {
            await _roleRepository.AddDefaultRolesAndScope();
        }
    }
}
