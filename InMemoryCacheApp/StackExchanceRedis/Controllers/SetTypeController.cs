using Microsoft.AspNetCore.Mvc;
using StackExchanceRedis.Services;
using StackExchange.Redis;

namespace StackExchanceRedis.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly IDatabase db;
        private string listKey = "hashNames"; //unique rasgele yerleşim
        public SetTypeController(RedisCacheService redis)
        {
            db = redis.GetDb(2);
        }
        public IActionResult Index()
        {
            HashSet<string> namesList = new HashSet<string>();
            if (db.KeyExistsAsync(listKey).Result)
            {
                db.SetMembers(listKey).ToList().ForEach(i =>
                {
                    namesList.Add(i);
                });
            }
            return View(namesList);
        }

        [HttpPost]
        public IActionResult Add( string name)
        {
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));// her defasında istek bulunuldukça 5dk ekler
            var result = db.SetAdd(listKey, name);//sonuna ekler
            return RedirectToAction(nameof(Index));
         
        }

        public async Task<IActionResult> Delete(string name)
        {
            await  db.SetRemoveAsync(listKey, name);
            return RedirectToAction(nameof(Index));
        }
    }
}
