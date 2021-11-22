using System.Collections.Generic;
using System.Threading.Tasks;
using MaritimeCode.Models;
using MaritimeCode.ViewModels;

namespace Maritimecode.Services
{
    public interface ICalculationServices
    {
        Task<bool> CanProcess();
        Task ProcessFile(UploadModel vm);
        Task<double> MeansCalAsync(IEnumerable<RandomNumber> nums = null);
        Task<double> StandardDevAsync();
        Task<FreqViewModels> FequCalAsync();
    }
}