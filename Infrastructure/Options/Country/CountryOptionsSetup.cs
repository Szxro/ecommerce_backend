using Microsoft.Extensions.Configuration;

namespace Infrastructure.Options.Country;

public class CountryOptionsSetup : BaseSetup<CountryOptions>
{
    public CountryOptionsSetup(IConfiguration configuration) : base(configuration, "CountryOptions")
    {
    }
}
