using MaritimeCode.Models;
using Microsoft.EntityFrameworkCore;

namespace MaritimeCode.Data
{
    public class NumberContext: DbContext
    {
        public NumberContext(DbContextOptions<NumberContext> options): base(options)
        {
        }
        public DbSet<RandomNumber> RandomNumbers { get; set; }
    }
}
