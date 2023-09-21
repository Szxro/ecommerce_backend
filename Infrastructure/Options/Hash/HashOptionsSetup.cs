using Microsoft.Extensions.Configuration;

namespace Infrastructure.Options.Hash;

public class HashOptionsSetup : BaseSetup<HashOptions>
{
    public HashOptionsSetup(IConfiguration configuration) : base(configuration, "HashOptions") { }
}
