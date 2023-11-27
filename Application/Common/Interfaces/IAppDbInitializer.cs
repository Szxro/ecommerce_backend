namespace Application.Common.Interfaces;

public interface IAppDbInitializer
{
    Task MigrateAsync();

    Task SeedAsync();

    Task ConnectAsync();
}
