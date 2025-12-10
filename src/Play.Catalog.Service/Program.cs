var builder = WebApplication.CreateBuilder(args);

// Register MongoDB GUID serializer (only once at startup)
MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(typeof(Guid), new MongoDB.Bson.Serialization.Serializers.GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));

// Add services
builder.Services.AddControllers(options =>
                                {
                                    options.SuppressAsyncSuffixInActionNames = false;
                                });

// Needed by Swashbuckle to discover endpoints
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
