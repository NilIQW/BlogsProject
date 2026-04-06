namespace BlogsProject.Services;

public static class RedisCacheFactory
{
    public static RedisCacheService Create()
    {
        return new RedisCacheService(
            host: "redis",
            port: 6379,
            ""
        );
    }
}