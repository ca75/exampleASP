using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MaritimeCode.Models;

namespace MaritimeCode.Services
{
    public interface IRandomNumberRepository : IDisposable
    {
        Task<IEnumerable<RandomNumber>> GetRanNumbersAsync();
        Task UpdateRanNumbers(List<RandomNumber> ranEntityList);
       
    }
}