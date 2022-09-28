using Microsoft.AspNetCore.Mvc;
using StackExchanceRedis.Models;
using StackExchanceRedis.Services;
using System.Diagnostics;

namespace StackExchanceRedis.Controllers
{
    public class HomeController : Controller
    {
        private readonly RedisCacheService _redis;

        public HomeController(RedisCacheService redis)
        {
            this._redis = redis;
        }

        public IActionResult Index()
        {
            var db = _redis.GetDb(0);
            string deger = db.StringGet("serverkey1");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}