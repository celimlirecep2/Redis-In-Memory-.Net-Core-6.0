using Microsoft.AspNetCore.Mvc;
using StackExchanceRedis.Services;
using StackExchange.Redis;

namespace StackExchanceRedis.Controllers
{
    public class StringTypeController : Controller
    {
        
        private readonly IDatabase db;
        public StringTypeController(RedisCacheService redis)
        {
            db = redis.GetDb(0);
        }
        public IActionResult Index()
        {
            
           
            db.StringSet("key2", 3);
            //resimler byte çevrilip kullanılabilir
            Byte[] bytes = default(byte[]);
            db.StringSet("resim", bytes);
            var deger= db.StringGet("key2");
            if (deger.HasValue)
            {
                db.StringIncrement("key2",1);//1 er 1 er arttırsın 
            }
            deger = db.StringGet("key2");

            var count = db.StringDecrementAsync("key2", 1).Result; // await yerine result kullanılabilir


            db.StringSet("key2", "mehmet");

            var ilk3karakter = db.StringGetRangeAsync("key2", 0, 2).Result;
            var key2Uzunluk = db.StringLength("key2");

            return View();
        }
        public IActionResult Delete()
        {
            
            
            return View();
        }
    }
}
