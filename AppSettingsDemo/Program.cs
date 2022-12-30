using AppSettingsDemo;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var appConnectionString = builder.Configuration.GetConnectionString("AppConfig");
var useAppConfiguration =    !string.IsNullOrWhiteSpace(appConnectionString);

if (useAppConfiguration)
{
    builder.Host.ConfigureAppConfiguration(cfg => {
        cfg.AddAzureAppConfiguration(options =>
        {
            options.Connect(appConnectionString)
                .ConfigureRefresh(refresh =>
                {
                    refresh.Register("Test").SetCacheExpiration(
                        TimeSpan.FromSeconds(20));
                });

            if (Convert.ToBoolean(builder.Configuration["UsingAzureKeyVault"]))
                options.ConfigureKeyVault(kv =>
                {
                    kv.SetCredential(new DefaultAzureCredential());
                });
        });
    });

    builder.Services.AddAzureAppConfiguration();
}


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (useAppConfiguration)
    app.UseAzureAppConfiguration();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
