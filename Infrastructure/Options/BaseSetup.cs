using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options; // (need to install the nugget Microsoft.Extensions.Configuration.Binder to use the Bind method)

namespace Infrastructure.Options;

public abstract class BaseSetup<T> : IConfigureOptions<T> where T : class
{
    // Can use the Depedency Injection

    protected readonly IConfiguration _configuration;
    private string ConfigurationSectionName { get; set; }

    public BaseSetup(IConfiguration configuration, string configurationSectionName)
    {
        _configuration = configuration;
        ConfigurationSectionName = configurationSectionName;
    }

    // virtual to override the method if its neccesary (abstract have to implement the method in all the classes)
    public virtual void Configure(T options)
    {
        _configuration.GetSection(ConfigurationSectionName).Bind(options);
        // The Key have to be equals to the properties in the class to bind correctly
    }
}
