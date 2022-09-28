using StackExchange.Redis;
using System.Text.Json;

namespace RedisExchangeAPI.Web.Services
{
    public class RedisService:ICacheService
    {

        private readonly IConnectionMultiplexer _redisCon;
        private readonly IDatabase _db;
        private TimeSpan ExpireTime => TimeSpan.FromDays(1);

        public RedisService(IConnectionMultiplexer redisCon)
        {
            _redisCon = redisCon;
           _db= _redisCon.GetDatabase();
        }



        public async Task Clear(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public void ClearAll()
        {
            var endpoints = _redisCon.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _redisCon.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> action) where T : class
        {
            var result = await _db.StringGetAsync(key);
            if (result.IsNull)
            {
                result = JsonSerializer.SerializeToUtf8Bytes(await action());
                await SetValueAsync(key, result);
            }
            return JsonSerializer.Deserialize<T>(result);
        }

        public async Task<string> GetValueAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public async Task<bool> SetValueAsync(string key, string value)
        {
            return await _db.StringSetAsync(key, value, ExpireTime);
        }

        public T GetOrAdd<T>(string key, Func<T> action) where T : class
        {
            var result = _db.StringGet(key);
            if (result.IsNull)
            {
                result = JsonSerializer.SerializeToUtf8Bytes(action());
                _db.StringSet(key, result, ExpireTime);
            }
            return JsonSerializer.Deserialize<T>(result);
        }











        //public IDatabase db { get; set; }
        //private ConnectionMultiplexer _redis;


        //public  RedisService(IConfiguration configuration)

        //{

        //}
        //public void Connect()
        //{
        //    var configString = $"{_redisHost}:{_redisPort}";
        //    _redis=ConnectionMultiplexer.Connect(configString);
        //}
        //public IDatabase GetDb(int db)
        //{
        //    return _redis.GetDatabase(db);
        //}
    }
}
