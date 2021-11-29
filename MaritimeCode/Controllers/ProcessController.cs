using MaritimeCode.Models;
using MaritimeCode.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MaritimeCode.Controllers
{
    public class ProcessController : Controller
    {
        private readonly ICalculationServices _cal;

        public ProcessController(ICalculationServices calculationServices)
        {
            _cal = calculationServices;
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
                }
                catch (Exception ex)
                {
                    return Content($"Error {ex.Message}");
                }
            }

            return Content($@"Error, {ModelState.Values.SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage).ToList()[0]}, Please try again.");
        }

        public async Task<IActionResult> Means()
        {
            return Content($"The Arithmetic Mean = {await _cal.MeansCalAsync()}");
        }

        public async Task<IActionResult> StdDev(bool isPopulation)
        {
            return Content($"The Standard Deviation = {await _cal.StandardDevAsync(isPopulation)}");
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
