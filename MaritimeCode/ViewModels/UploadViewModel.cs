using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MaritimeCode.ViewModels
{
    public class UploadViewModel
    {
        [Required]        
        public IFormFile File { get; set; }
    }

    public class FreqViewModels
    {
        [Required]
        public Dictionary<int, List<double>> ValuePairs { get; set; } = new Dictionary<int, List<double>>();
        public string Errors { get; set; }
    }
}
