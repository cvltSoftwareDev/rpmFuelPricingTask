using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using rpmFuelPricingTask.Services;
using rpmFuelPricingTask.Services.Interfaces;

var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();

var configuration = builder.Build();

var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddScoped<IConfiguration>(_ => configuration)
            .AddSingleton<IApiCaller, ApiCaller>()
            .AddSingleton<ITaskHandler, TaskHandler>()
            .AddSingleton<IRecordService, RecordService>()
            .BuildServiceProvider();

var startService = serviceProvider.GetService<ITaskHandler>();

startService.HandleTask();

Console.ReadLine();