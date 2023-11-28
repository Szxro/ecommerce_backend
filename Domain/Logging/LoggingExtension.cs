using Microsoft.Extensions.Logging;

namespace Domain.Logging;

public static partial class LoggingExtension
{
    // These are call logging source generators it may improve perfomance (it check if the log level is in use and its a better way to perfomance logging in .net app)
    [LoggerMessage(
        EventId = 1,
        EventName = "Request Perfomance Warning",
        Level = LogLevel.Warning, 
        Message = "The current request {requestName} was maded by {username} and complete in {elapsedMilliseconds} ms")]
    public static partial void RequestPerfomanceWarning(this ILogger logger,
                                                 string username,
                                                 string requestName,
                                                 long elapsedMilliseconds);
    [LoggerMessage(
        EventId = 2,
        EventName = "Unhandled Exception Error",
        Level = LogLevel.Error,
        Message = "Unhandled Exception for {origin} maded by {username}, message: {message}"
        )]
    public static partial void UnhandleRequestError(this ILogger logger,
                                               string origin,
                                               string username,
                                               string message);

    [LoggerMessage(
        EventId = 3,
        EventName = "Cant Connect Database Error",
        Level = LogLevel.Error,
        Message = "An error occured trying to connect to the database {databaseProviderName} : {message}"
        )]
    public static partial void ConnectDatabaseError(this ILogger logger,
                                               string? databaseProviderName,
                                               string message);

    [LoggerMessage(
        EventId = 4,
        EventName = "Migrate Database Error",
        Level = LogLevel.Error,
        Message = "An error occured trying to do migration to the database {databaseProviderName} : {message}"
        )]
    public static partial void MigrateDatabaseError(this ILogger logger,
                                                    string? databaseProviderName,
                                                    string message);

    [LoggerMessage(
        EventId = 5,
        EventName = "Seed Database Error",
        Level = LogLevel.Error,
        Message = "An error ocurred trying to seed the database: {message}"
        )]
    public static partial void SeedDatabaseError(this ILogger logger, string message);

    [LoggerMessage(
        EventId = 6,
        EventName = "Http Request Error",
        Level = LogLevel.Error,
        Message = "An error ocurred trying to make a http request to {uri} with the following message: {message}"
        )]
    public static partial void HttpRequestError(this ILogger logger,string uri,string message);
}
