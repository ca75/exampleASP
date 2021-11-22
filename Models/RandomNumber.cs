using System.ComponentModel.DataAnnotations.Schema;

namespace MaritimeCode.Models
{
    [Table("RandomNumber")]
    public class RandomNumber
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double NumberValue { get; set; }
    }
}
