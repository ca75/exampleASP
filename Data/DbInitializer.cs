using MaritimeCode.Models;
using System.Linq;

namespace MaritimeCode.Data
{
    public static class DbInitializer
    {
        public static void Initialize(NumberContext context)
        {
            context.Database.EnsureCreated();

            
            if (context.RandomNumbers.Any())
            {
                if (context.RandomNumbers.Count() > 1)
                {
                    var allRan = context.RandomNumbers.ToList();
                    context.RandomNumbers.RemoveRange(allRan);               
                }
                else
                {
                    return;
                }
               
            }

            var randomNumber = new RandomNumber { NumberValue = 1000 };
            context.RandomNumbers.Add(randomNumber);
            context.SaveChanges();
        }
    }
}
