using Maritimecode.Services;
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
        private readonly ILogger<HomeController> _logger;
        private readonly ICalculationServices _cal;

        public HomeController( ILogger<HomeController> logger, ICalculationServices cal)
        {
            _logger = logger;
            _cal = cal;
        }

        public IActionResult Index()
        {

            return View();
        }
        public async Task<IActionResult> CheckUploaded()
        {            
            if ( await _cal.CanProcess())            
                return StatusCode(200);
            
            return StatusCode(404);
        }

        [HttpPost]       
        public async Task<IActionResult> Upload(UploadModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _cal.ProcessFile(vm);
                    return Ok($"File Upload success '{vm.File.FileName}'");
                }catch(Exception ex)
                {
                    return Content($"Error {ex.Message}");
                }
            }            
            
            return Content($@"Error, {ModelState.Values.SelectMany(v=>v.Errors)
            .Select(e=>e.ErrorMessage).ToList()[0]}, Please try again.");
        }

        public async Task<IActionResult> Means()
        {           
            return Content($"The Arithmetic Mean = {await _cal.MeansCalAsync()}");
        }

        public async Task<IActionResult> StdDev()
        {            
            return Content($"The Standard Deviation = {await _cal.StandardDevAsync()}");
        }
        public async Task<IActionResult> FreqChart()
        {
            return PartialView("_FreqView", await _cal.FequCalAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

