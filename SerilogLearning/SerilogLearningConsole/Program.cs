using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

var services = new ServiceCollection();

// Add logging with Serilog
services.AddLogging(loggingBuilder =>
{
    //loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(dispose: true);
});

services.AddTransient<ILoggerFactory, LoggerFactory>();
services.AddTransient(typeof(ILogger<>), typeof(Logger<>));
services.AddTransient<SomeService>();
services.AddDbContext<InMemoryContext>(options =>
{
    options.UseInMemoryDatabase(databaseName: "InMemoryDb");
});



var serviceProvider = services.BuildServiceProvider();

var myService = serviceProvider.GetRequiredService<SomeService>();

myService.DoDomething(123);
myService.AddPerson("Person1");
myService.AddPerson("Person2");
myService.AddPerson("Person3");
myService.AddPerson("Person4");
myService.AddPerson("Person5");

var anotherInstance = serviceProvider.GetRequiredService<SomeService>();
var people = anotherInstance.LoadAll();

Console.ReadKey();