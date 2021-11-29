using System.Collections.Generic;
using System.Threading.Tasks;
using MaritimeCode.Models;
using MaritimeCode.ViewModels;

namespace MaritimeCode.Services
{
    public interface ICalculationServices
    {        
        Task ProcessFile(UploadModel vm, bool saveData = true);
        Task<double> MeansCalAsync(IEnumerable<RandomNumber> nums = null);
        Task<double> StandardDevAsync(bool isPopulation);
        Task<FreqViewModels> FequCalAsync();
    }
}