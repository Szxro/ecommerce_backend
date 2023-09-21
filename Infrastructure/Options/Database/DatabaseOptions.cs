namespace Infrastructure.Options.Database;

public class DatabaseOptions
{
    public string ConnectionString { get; set; } = string.Empty;

    public int MaxRetryCount { get; set; }
}
