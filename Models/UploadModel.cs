using System.ComponentModel.DataAnnotations;
using Maritimecode.ValidationAttributes;
using Microsoft.AspNetCore.Http;

namespace MaritimeCode.Models
{
    public class UploadModel
    {
        [Required]
        [HeadersAllowedAttribute("text/",".csv")]
        public IFormFile File { get; set; }
    }
}





