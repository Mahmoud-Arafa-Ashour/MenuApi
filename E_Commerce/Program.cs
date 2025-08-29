using E_Commerce;

var builder = WebApplication.CreateBuilder(args);
// Add CORS policy
builder.Services.AddCors(
    options => options.AddPolicy("AllowAll" , builder =>
    {
        builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin();
    }));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDependencies(builder.Configuration);

// Register the database seeder
builder.Services.AddScoped<DatabaseSeeder>();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");  // CORS must be applied before controllers
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();
app.UseRateLimiter();
app.Run();