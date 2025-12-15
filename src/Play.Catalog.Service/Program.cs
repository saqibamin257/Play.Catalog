using Play.Common.Settings;
using MassTransit;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Settings;
using Play.Common.MongoDB;


var builder = WebApplication.CreateBuilder(args);
// Add services
Play.Common.Settings.ServiceSettings serviceSettings = builder.Configuration.GetSection(nameof(Play.Common.Settings.ServiceSettings)).Get<Play.Common.Settings.ServiceSettings>();
//builder.Services.AddMongo().AddMongoRepository<Item>("items");
Play.Common.MongoDB.Extensions.AddMongo(builder.Services).AddMongoRepository<Item>("items");

builder.Services.AddMassTransit(x =>
{
  x.UsingRabbitMq((context, configurator) =>
  {
    var rabbitMQSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
    configurator.Host(rabbitMQSettings.Host);
    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));

  });
});

//builder.Services.AddMassTransitHostedService();



builder.Services.AddControllers(options =>
                                {
                                  options.SuppressAsyncSuffixInActionNames = false;
                                });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger UI (do this even outside Development when you're working locally)
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
