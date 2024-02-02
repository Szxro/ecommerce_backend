using Microsoft.Extensions.Configuration;

namespace Infrastructure.Options.Database;

public class DatabaseOptionsSetup : BaseSetup<DatabaseOptions>
{
    public DatabaseOptionsSetup(IConfiguration configuration) : base(configuration, "DatabaseOptions") { }

    public override void Configure(DatabaseOptions options)
    {
        options.ConnectionString = _configuration.GetConnectionString("default");

        base.Configure(options);
    }
}
