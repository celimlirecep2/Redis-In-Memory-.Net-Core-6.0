using StackExchange.Redis;

namespace StackExchanceRedis.Services
{
    public class RedisCacheService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;
        public IDatabase db;
        private IConnectionMultiplexer _redis;

        public RedisCacheService(IConfiguration configuration)
        {
            _redisHost = configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];
        }

        public void Connect()
        {
            string connStr = $"{_redisHost}:{_redisPort}";
            try
            {
                _redis = ConnectionMultiplexer.Connect(connStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
         
        }
        public IDatabase GetDb(int id)
        {
            return _redis.GetDatabase(id);
        }
    }
}
