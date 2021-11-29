using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MaritimeCode.Services;
using MaritimeCode.Models;
using System.Diagnostics;

namespace MaritimeCode.Controllers
{
    public class RandomNumbersController : Controller
    {
        private readonly IRandomNumberRepository _rNR;

        public RandomNumbersController(IRandomNumberRepository randomNumberRepository)
        {            
            _rNR = randomNumberRepository;
        }

        // GET: RandomNumbers
        public async Task<IActionResult> Index()
        {            
            return View(await _rNR.GetRanNumbersAsync());
        }       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
