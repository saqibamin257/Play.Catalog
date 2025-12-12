
using Play.Catalog.Service.Entities;
using Play.Common.MongoDB;
using Play.Common.Settings;

var builder = WebApplication.CreateBuilder(args);
// Add services
ServiceSettings serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
//builder.Services.AddMongo().AddMongoRepository<Item>("items");
Play.Common.MongoDB.Extensions.AddMongo(builder.Services).AddMongoRepository<Item>("items");

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
