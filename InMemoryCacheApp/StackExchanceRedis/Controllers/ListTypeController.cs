using Microsoft.AspNetCore.Mvc;
using StackExchanceRedis.Services;
using StackExchange.Redis;

namespace StackExchanceRedis.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly IDatabase db;
        private string listKey ="names";
        public ListTypeController(RedisCacheService redis)
        {
            db = redis.GetDb(1);
        }
        public IActionResult Index()
        {
            List<string> list = new List<string>();
            if (db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(i =>
                {
                    list.Add(i.ToString());
                }) ;
            }
         
            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            var result= db.ListRightPush(listKey, name);//sonuna ekler
            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult DeleteList(string name)
        {
          db.ListRemoveAsync(listKey,name).Wait();
            //db.ListLeftPop(listKey); dizinin ilk elemanını siler
            return RedirectToAction(nameof(Index));
        }
    }
}
