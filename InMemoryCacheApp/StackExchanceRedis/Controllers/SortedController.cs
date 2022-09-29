using Microsoft.AspNetCore.Mvc;
using StackExchanceRedis.Services;
using StackExchange.Redis;

namespace StackExchanceRedis.Controllers
{
    public class SortedController : Controller
    {
        //score değeri belirleyerek sıralama ayarı yapıyoruz
        /// <summary>
        /// mehmet 10
        /// veli 2
        /// recep1
        /// sıralama recep veli a-mehmet
        /// aynı scoreda olabilirler fakat aynı isimde olamazlar 
        /// aynı isimde farklı skor değeri ile gelinirse skor değerini güncellemeye alır  
        /// </summary>

        private readonly IDatabase db;
        private string listKey = "sortedName";
        public SortedController(RedisCacheService redis)
        {
            db = redis.GetDb(3);
        }
        public IActionResult Index()
        {
            HashSet<string> list = new HashSet<string>();

            if (db.KeyExists(listKey))
            {
                db.SortedSetScan(listKey).ToList().ForEach(i =>
                {
                    list.Add(i.Element.ToString());
                });

              var buyuktenKucuge=  db.SortedSetRangeByRank(listKey, order: Order.Descending).ToList();
                var aralık = db.SortedSetRangeByRank(listKey,0,5, order: Order.Descending).ToList();
            }

          
            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string name,int score)
        {
         
            db.SortedSetAdd(listKey,name,score);
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));
            return RedirectToAction(nameof(Index));
        }

        public  IActionResult Delete(string name)
        {
            bool result= db.SortedSetRemove(listKey, name.Split(":")[0]);
            return RedirectToAction(nameof(Index));
        }
    }
}
