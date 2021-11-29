using MaritimeCode.Services;
using MaritimeCode.Data;
using MaritimeCode.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MaritimeCode.Controllers
{
    public class HomeController : Controller
    {
        private readonly CanProcessService _canProcessService;

        public HomeController( CanProcessService canProcessService)
        {
           
            _canProcessService = canProcessService;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult CheckUploaded()
        {            
            if ( _canProcessService.CanProcess)            
                return StatusCode(200);
            
            return StatusCode(404);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

