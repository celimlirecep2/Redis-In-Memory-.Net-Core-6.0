using InMemoryCacheApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCacheApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private  IMemoryCache _memory;

        public ProductController(IMemoryCache memory)
        {
            this._memory = memory;
        }
        public IActionResult Index()
        {
            //süresiz
            _memory.Set<string>("zaman", DateTime.Now.ToString());

            //süreli
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(5);
            _memory.Set<string>("zaman2", DateTime.Now.ToString(), options);

            //10 saniye içerisinde istek atılırsa kendini yeniler
            MemoryCacheEntryOptions options2 = new MemoryCacheEntryOptions();
            //options2.AbsoluteExpiration = DateTime.Now.AddSeconds(5);
            options2.SlidingExpiration = TimeSpan.FromSeconds(10);
            options2.Priority=CacheItemPriority.Normal;
            //cache in neden silindiği bilgisini tutar
            options2.RegisterPostEvictionCallback((key, value, reason, state) => {
            
                _memory.Set("callback",$"{key}->{value}=> sebep: {reason}");
            });
            _memory.Set<string>("zaman3", DateTime.Now.ToString(), options2);
        

            Product product1 = new Product()
            {
                ID = 1,
                Name = "Samsung s7",
                Price = 200
            };
            _memory.Set<Product>("Product:1", product1);
            return View();
        }
        public IActionResult Show()
        {
           ViewBag.zaman= _memory.Get<string>("zaman");
            return View();
        }
        public IActionResult Show2()
        {
            //cache de eğer değer varsa onu alır ve onu zamanCache e atar ve true döner yoksa false döner
            _memory.TryGetValue("zaman2", out string zamanCache);
            ViewBag.zaman2 = zamanCache;

            //istek devam ederse
            _memory.TryGetValue("zaman3", out string zamanCache2);
            ViewBag.zaman3 = zamanCache2;

            //cache izleme option2
            _memory.TryGetValue("callback", out string callback);
            ViewBag.callback = callback;

            _memory.TryGetValue("Product:1", out Product product1);
            ViewBag.product1 = product1;


            return View();
        }

        public void RemoveZamanCache()
        {
            _memory.Remove("zaman");
        }

        public void varsaGetirYoksaOluştur()
        {
            _memory.GetOrCreate<string>("string", entry=> { return DateTime.Now.ToString(); } );
        }
    }
}
