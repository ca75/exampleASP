using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MaritimeCode.Data;
using MaritimeCode.Models;
using MaritimeCode.ViewModels;

namespace MaritimeCode.Services
{
    public class CalculationServices : ICalculationServices
    {        
        private readonly IRandomNumberRepository _rNR;

        public CalculationServices(IRandomNumberRepository rNR)
        {           
            _rNR = rNR;
        }       

        public async Task ProcessFile(UploadModel vm, bool saveData = true)
        {
            var error = string.Empty;
            var ranEntityList = new List<RandomNumber>();
            var filPath = Path.GetTempFileName();

            using (var stream = new FileStream(filPath, FileMode.Create))
            {
                await vm.File.CopyToAsync(stream);
            }
            string ReadCSV = System.IO.File.ReadAllText(filPath);
            foreach (var item in ReadCSV.Split(','))
            {
                try
                {
                    ranEntityList.Add(new RandomNumber { NumberValue = double.Parse(item) });
                }
                catch (System.Exception ex)
                {
                    error = ex.Message;
                    break;
                }

            }
            if (System.IO.File.Exists(filPath))
            {
                System.IO.File.Delete(filPath);
            }
                       
            if(error != string.Empty)
            {               
                throw new Exception(error);                             
            }
            else if (ranEntityList.Count == 1)
            {
                throw new Exception("Only 1 number detected");
            }
            else if (saveData)
                await _rNR.UpdateRanNumbers(ranEntityList);
        }

        public async Task<double> MeansCalAsync(IEnumerable<RandomNumber> nums = null)
        {
            nums = nums == null ? await _rNR.GetRanNumbersAsync() : nums;
            double d = 0;
            foreach (var item in nums)
            {
                d += item.NumberValue;
            }
            return d / nums.Count();
        }

        public async Task<double> StandardDevAsync(bool isPopulation)
        {            
            var nums =await  _rNR.GetRanNumbersAsync();
            double d = 0;
            double mean = await MeansCalAsync(nums);
            foreach (var item in nums)
            {
                var firstStep = item.NumberValue - mean;
                firstStep *= firstStep;
                d += firstStep;
            }
            if (isPopulation)
            {
                var secondStep = d / nums.Count(); 
                return Math.Sqrt(secondStep);
            }
            else
            {
                var secondStep = d / (nums.Count() - 1); // as its a sample
                return Math.Sqrt(secondStep);
            }
            
        }

        public async Task<FreqViewModels> FequCalAsync()
        {
            var nums =await _rNR.GetRanNumbersAsync();
            var FVM = new FreqViewModels();
            var Results = FVM.ValuePairs;

            try
            {
                foreach (var item in nums)
                {
                    var a = item.NumberValue;

                    if (a > 0 && a < 10)
                    {
                        if (Results.ContainsKey(1))
                        {
                            Results[1].Add(item.NumberValue);
                        }
                        else
                        {
                            Results[1] = new List<double> { item.NumberValue };
                        }
                    }
                    else if (a >= 10 && a < 20)
                    {
                        if (Results.ContainsKey(2))
                        {
                            Results[2].Add(item.NumberValue);
                        }
                        else
                        {
                            Results[2] = new List<double> { item.NumberValue };
                        }
                    }
                    else if (a >= 20 && a < 30)
                    {
                        if (Results.ContainsKey(3))
                        {
                            Results[3].Add(item.NumberValue);
                        }
                        else
                        {
                            Results[3] = new List<double> { item.NumberValue };
                        }
                    }
                    else if (a >= 30 && a < 40)
                    {
                        if (Results.ContainsKey(4))
                        {
                            Results[4].Add(item.NumberValue);
                        }
                        else
                        {
                            Results[4] = new List<double> { item.NumberValue };
                        }
                    }
                    else if (a >= 40 && a < 50)
                    {
                        if (Results.ContainsKey(5))
                        {
                            Results[5].Add(item.NumberValue);
                        }
                        else
                        {
                            Results[5] = new List<double> { item.NumberValue };
                        }

                    }
                    else if (a >= 50 && a < 60)
                    {
                        if (Results.ContainsKey(6))
                        {
                            Results[6].Add(item.NumberValue);
                        }
                        else
                        {
                            Results[6] = new List<double> { item.NumberValue };
                        }
                    }
                    else if (a >= 60 && a < 70)
                    {
                        if (Results.ContainsKey(7))
                        {
                            Results[7].Add(item.NumberValue);
                        }
                        else
                        {
                            Results[7] = new List<double> { item.NumberValue };
                        }
                    }
                    else if (a >= 70 && a < 80)
                    {
                        if (Results.ContainsKey(8))
                        {
                            Results[8].Add(item.NumberValue);
                        }
                        else
                        {
                            Results[8] = new List<double> { item.NumberValue };
                        }
                    }
                    else if (a >= 80 && a < 90)
                    {
                        if (Results.ContainsKey(9))
                        {
                            Results[9].Add(item.NumberValue);
                        }
                        else
                        {
                            Results[9] = new List<double> { item.NumberValue };
                        }
                    }
                    else if (a >= 90 && a < 100)
                    {
                        if (Results.ContainsKey(10))
                        {
                            Results[10].Add(item.NumberValue);
                        }
                        else
                        {
                            Results[10] = new List<double> { item.NumberValue };
                        }
                    }
                    else
                    {
                        if (Results.ContainsKey(11))
                        {
                            Results[11].Add(item.NumberValue);
                        }
                        else
                        {
                            Results[11] = new List<double> { item.NumberValue };
                        }
                    }

                }
            }
            catch (System.Exception ex)
            {
                FVM.Errors = ex.Message;
            }
            FVM.ValuePairs = Results;

            return FVM;
        }


    }
}
