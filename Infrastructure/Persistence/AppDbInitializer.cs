using Application.Common.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public class AppDbInitializer : IAppDbInitializer
{
    private readonly ILogger<AppDbInitializer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppDbContext _context;

    public AppDbInitializer(
        ILogger<AppDbInitializer> logger,
        IUnitOfWork unitOfWork,
        AppDbContext context)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _context = context;
    }
    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.MigrateAsync(); // Looking for any new migration to do it automatically the api start
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occured trying to do migration to the database <{database}> : <{message}>",_context.Database.ProviderName,ex.Message);

            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();

        } catch (Exception ex)
        {
            _logger.LogError("An error ocurred trying to seed the database <{message}>",ex.Message);

            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (!_context.Role.Any())
        {
            ICollection<Role> roles = new HashSet<Role>()
            {
                new Role(){ RoleName = "User", Description = "Normal User"},
                new Role(){RoleName = "Admin", Description = "Super User" }
            };

            _context.Role.AddRange(roles);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
