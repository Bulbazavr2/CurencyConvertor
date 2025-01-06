var builder = WebApplication.CreateBuilder(args);

// Adding CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()  // Allow requests from any origin
                  .AllowAnyMethod()  // Allow any HTTP method (GET, POST, etc.)
                  .AllowAnyHeader(); // Allow any headers
        });
});

// Adding controllers
builder.Services.AddControllers();

var app = builder.Build();

// Using CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
