using Hangfire;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

builder.Configuration.AddJsonFile("appsettings.json", optional: false);
    
// Add services to the container.
builder.Services.AddRazorPages();


builder.Services.AddHangfire(cfg =>
{
    cfg
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"));
});

builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseHangfireDashboard();


app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(ep =>
{
    ep.MapControllers();
    ep.MapHangfireDashboard();
});

app.MapRazorPages();

app.Run();