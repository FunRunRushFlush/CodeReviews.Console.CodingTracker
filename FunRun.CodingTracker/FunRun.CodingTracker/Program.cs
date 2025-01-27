using FunRun.CodingTracker;
using FunRun.CodingTracker.Data;
using FunRun.CodingTracker.Data.Interfaces;
using FunRun.CodingTracker.Services;
using FunRun.CodingTracker.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;
using Microsoft.Extensions.Configuration;

var host = Host.CreateDefaultBuilder(args)
   .ConfigureServices((context, services) =>
   {
       var configuration = context.Configuration;
       string connectionString = configuration.GetConnectionString("SQLiteConnection")!;
       services.AddSingleton(provider => new SQLiteConnectionFactory(connectionString));
       services.AddScoped<IDbConnection>(provider =>
        {
            var factory = provider.GetRequiredService<SQLiteConnectionFactory>();
            return factory.CreateConnection();
        });

        services.AddSingleton<CodingTrackerApp>();

        services.AddScoped<ISessionCrudService, SessionCrudService>();
        services.AddScoped<IUserInputService, UserInputService>();
        services.AddScoped<IDataAccess, DataAccess>();

    })
    .ConfigureLogging(logger =>
    {
        logger.ClearProviders();
        logger.AddDebug();

    }).Build();


var init = host.Services.GetRequiredService<SQLiteConnectionFactory>();
init.InitializeDatabase();

var app = host.Services.GetRequiredService<CodingTrackerApp>();
await app.RunApp();