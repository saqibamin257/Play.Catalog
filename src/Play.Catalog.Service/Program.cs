using System.Runtime.CompilerServices;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Settings;



var builder = WebApplication.CreateBuilder(args);


// Register MongoDB GUID serializer (only once at startup)
MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(typeof(Guid), new MongoDB.Bson.Serialization.Serializers.GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));

// Add services
ServiceSettings serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
builder.Services.AddSingleton(serviceProvider =>
{
    var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
    var mongoClient = new MongoClient(mongoDbSettings.Connectiontring);
    return mongoClient.GetDatabase(serviceSettings.ServiceName);
});

builder.Services.AddSingleton<IItemsRepository, ItemsRepository>();

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
