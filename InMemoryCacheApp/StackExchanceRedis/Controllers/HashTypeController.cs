using Microsoft.AspNetCore.Mvc;
using StackExchanceRedis.Services;
using StackExchange.Redis;

namespace StackExchanceRedis.Controllers
{
    public class HashTypeController : Controller
    {
        private readonly IDatabase db;
        private string hashkey = "sözlük";
        public HashTypeController(RedisCacheService redis)
        {
            db = redis.GetDb(4);
        }
        public IActionResult Index()
        {
            Dictionary<string,string> list = new Dictionary<string, string>();

            if (db.KeyExists(hashkey))
            {
                db.HashGetAll(hashkey).ToList().ForEach(i =>
                {
                    list.Add(i.Name.ToString(),i.Value.ToString());
                });

                //db.HashGet(hashkey, "pen"); tek bir değer almak için kullanırız
            }


            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string key, string value)
        {

            db.HashSet(hashkey, key, value);
          
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string key)
        {
            bool result = await db.HashDeleteAsync(hashkey,key);
            return RedirectToAction(nameof(Index));
        }
    }
}
