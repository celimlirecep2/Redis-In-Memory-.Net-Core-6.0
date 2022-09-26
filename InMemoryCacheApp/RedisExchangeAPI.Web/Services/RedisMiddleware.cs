namespace RedisExchangeAPI.Web.Services
{
    public static class RedisMiddleware
    {
        public static IApplicationBuilder Redis(this IApplicationBuilder app,RedisService redisService)
        {

            redisService.Connect();
            return app;
        }
    }
}
