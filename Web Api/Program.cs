using Application;
using Infrastructure;
using Web_Api.Extensions;
using Web_Api.Middleware;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure();
    builder.Services.AddInfrastructureAuthentication(builder.Configuration);
    builder.Services.AddControllers();
    builder.Services.RegisterMiddlewares(); // Registering the middleware (have to register because it use the IMiddleware interface)
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.ConfigureSwagger();
    builder.Configuration.AddUserSecrets<Program>(optional: false, reloadOnChange: true); // Adding the user secrets
}


var app = builder.Build();
{ 
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        await app.Services.InitializeDatabaseAsync(); // Migrating and Seeding if its necessary
    }

    app.UseMiddleware<AuthorizationMiddleware>(); // Using the middleware (the organization count on who is going to run first) 
    app.UseMiddleware<ExceptionMiddleware>(); 

    app.UseHttpsRedirection();

    app.UseAuthentication(); // Adding the authentication middleware

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

