namespace Application.Common.Interfaces;

public interface IAppDbInitializer
{
    Task InitializeAsync();

    Task SeedAsync();
}
