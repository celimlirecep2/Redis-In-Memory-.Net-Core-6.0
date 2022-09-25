using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions=new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1);
            _distributedCache.SetString("name", "Recep",cacheEntryOptions);
            string value=string.Empty;
            ViewBag.nameValue=_distributedCache.GetString("name");
            await _distributedCache.SetStringAsync("asyncname", "recep", cacheEntryOptions);
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return Redirect("/Home/index");
        }

        public async Task<IActionResult> ClassSet()
        {
            Product product = new Product()
            {
                Id = 1,
                Name = "İphone",
                Price = 12323
            };
            //Yazma işlemi Json
            string jsonProduct= JsonConvert.SerializeObject(product);
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1);
            await _distributedCache.SetStringAsync("product:1", jsonProduct,cacheEntryOptions);
            // okuma işlemi json
            string jsonValue = await _distributedCache.GetStringAsync("product:1");
            if (!string.IsNullOrEmpty(jsonValue))
            {
               ViewBag.product = JsonConvert.DeserializeObject<Product>(jsonValue);
            }

            //yazma işlemi byte
            Byte[] byteProduct=Encoding.UTF8.GetBytes(jsonValue);
            await _distributedCache.SetAsync("product:2",byteProduct);
            //okuma işlemi byte
            Byte[] getByteProduct = _distributedCache.Get("product:2");
            string jsonproduct= Encoding.UTF8.GetString(getByteProduct);
            if (!string.IsNullOrEmpty(jsonproduct))
            {
                ViewBag.product2 = JsonConvert.DeserializeObject<Product>(jsonproduct);
            }

            return View();
        }


    }
}
