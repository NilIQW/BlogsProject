using BlogsProject.Repositories;
using BlogsProject.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// ------------------- MongoDB Setup -------------------
builder.Services.AddSingleton<IMongoClient>(
    new MongoClient(builder.Configuration["MongoDB:ConnectionString"]));

builder.Services.AddScoped<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>()
        .GetDatabase(builder.Configuration["MongoDB:Database"]));

// ------------------- Repositories -------------------
builder.Services.AddScoped<IBlogRepository, MongoBlogRepository>();
builder.Services.AddScoped<IPostRepository, MongoPostRepository>();

// ------------------- Services -------------------
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddSingleton(RedisCacheFactory.Create());
// ------------------- Controllers -------------------
builder.Services.AddControllers();

// ------------------- Swagger/OpenAPI -------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ------------------- Middleware -------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();