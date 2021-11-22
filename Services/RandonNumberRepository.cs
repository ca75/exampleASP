using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaritimeCode.Data;
using MaritimeCode.Models;
using Microsoft.EntityFrameworkCore;

namespace Maritimecode.Services
{
    public class RandonNumberRepository : IRandonNumberRepository
    {
        private readonly NumberContext _nDb;

        public RandonNumberRepository(NumberContext nDb)
        {
            _nDb = nDb;
        }

        public async Task<IEnumerable<RandomNumber>> GetRanNumbersAsync()
        {                        
            return await _nDb.RandomNumbers.ToListAsync();
        }

        public async Task UpdateRanNumbers(List<RandomNumber> ranEntityList)
        {
            var allRan = await GetRanNumbersAsync();
             _nDb.RandomNumbers.RemoveRange(allRan);
                await _nDb.RandomNumbers.AddRangeAsync(ranEntityList);
                var result = await _nDb.SaveChangesAsync();
                if (result < (allRan.Count() + ranEntityList.Count))
                    throw new Exception("Not all entries may have been saved, please check");
        }

        protected virtual void Dispose(bool disposing)
        {

            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                _nDb.Dispose();
            }


        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}