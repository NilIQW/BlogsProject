using BlogsProject.Application.Events;
using BlogsProject.Application.Handlers;
using BlogsProject.Application.Messaging;
using BlogsProject.Domain.Interfaces;
using BlogsProject.Infrastructure.Mongo.Repositories;
using StackExchange.Redis;
using BlogsProject.Infrastructure.Redis;
using BlogsProject.Infrastructure.Sql;
using BlogsProject.Infrastructure.Sql.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoPostReadRepository = BlogsProject.Infrastructure.Mongo.Repositories.MongoPostReadRepository;

var builder = WebApplication.CreateBuilder(args);

// ---------------- SQL ----------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Sql")));

// ---------------- Mongo ----------------
builder.Services.AddSingleton<IMongoClient>(
    new MongoClient(builder.Configuration["MongoDB:ConnectionString"]));

builder.Services.AddScoped<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>()
        .GetDatabase(builder.Configuration["MongoDB:Database"]));

// ---------------- Redis ----------------
var redisConfig = builder.Configuration["Redis:Connection"];

if (string.IsNullOrWhiteSpace(redisConfig))
    throw new Exception("Redis connection string is missing");

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = ConfigurationOptions.Parse(redisConfig);

    config.AbortOnConnectFail = false; // important for production
    config.ConnectRetry = 5;
    config.ReconnectRetryPolicy = new ExponentialRetry(5000);

    return ConnectionMultiplexer.Connect(config);
});

builder.Services.AddScoped<IPostCache, PostCache>();
builder.Services.AddSingleton<LocalMessageBus>();

// ---------------- Repositories ----------------
builder.Services.AddScoped<IBlogWriteRepository, SqlBlogWriteRepository>();
builder.Services.AddScoped<IPostWriteRepository, SqlPostWriteRepository>();

builder.Services.AddScoped<IBlogReadRepository, MongoBlogReadRepository>();
builder.Services.AddScoped<IPostReadRepository, MongoPostReadRepository>();

// ---------------- Services ----------------
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<CommentRateLimiterService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var bus = app.Services.GetRequiredService<LocalMessageBus>();
var handler = app.Services.GetRequiredService<PostCreatedEventHandler>();

bus.Subscribe<PostCreatedEvent>(handler.Handle);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();