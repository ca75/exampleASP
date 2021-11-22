using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MaritimeCode.Models;

namespace Maritimecode.Services
{
    public interface IRandonNumberRepository : IDisposable
    {

        Task<IEnumerable<RandomNumber>> GetRanNumbersAsync();
        Task UpdateRanNumbers(List<RandomNumber> ranEntityList);
       
    }
}