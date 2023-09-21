using Microsoft.Extensions.Configuration;

namespace Infrastructure.Options.JWT;

public class JwtOptionsSetup : BaseSetup<JwtOptions>
{
    public JwtOptionsSetup(IConfiguration configuration) : base(configuration, "JWTOptions") { }
}
