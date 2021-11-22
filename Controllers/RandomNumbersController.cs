
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaritimeCode.Data;

namespace MaritimeCode.Controllers
{
    public class RandomNumbersController : Controller
    {
        private readonly NumberContext _context;

        public RandomNumbersController(NumberContext context)
        {
            _context = context;
        }

        // GET: RandomNumbers
        public async Task<IActionResult> Index()
        {
            return View(await _context.RandomNumbers.ToListAsync());
        }

        private bool RandomNumberExists(int id)
        {
            return _context.RandomNumbers.Any(e => e.Id == id);
        }
    }
}
