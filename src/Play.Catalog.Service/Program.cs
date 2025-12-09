var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

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
