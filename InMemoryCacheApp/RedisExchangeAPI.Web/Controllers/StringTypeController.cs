using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly ICacheService _cacheService;

        public StringTypeController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        public async Task<IActionResult> Index()
        {
          await  _cacheService.SetValueAsync("a","b");

            return View();
        }
    }
}
