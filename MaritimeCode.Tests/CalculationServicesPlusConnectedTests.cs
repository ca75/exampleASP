using MaritimeCode.Controllers;
using MaritimeCode.Models;
using MaritimeCode.Services;
using MaritimeCode.ViewModels;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MaritimeCode.Tests
{
    public class CalculationServicesPlusConnectedTests
    {
        private readonly Mock<ICalculationServices> _calculationServicesMock;
        private readonly Mock<IRandomNumberRepository> _randomNumberRepositoryMock;
        private readonly CalculationServices _calculationServices;
        private readonly ProcessController _processController;
        private readonly HomeController _homeController;
        private readonly RandomNumbersController _randomNumbersController;
        private CanProcessService _canProcessService;
        private readonly IEnumerable<RandomNumber> _rNList;
        private List<RandomNumber> localList = null;
        private Mock<IFormFile> _file;
        private UploadModel _uploadModel;

        public CalculationServicesPlusConnectedTests()
        {
            _rNList = new List<RandomNumber>() { 
                new RandomNumber { Id = 1, NumberValue = 5 }, new RandomNumber { Id = 2, NumberValue = 11 }, 
                new RandomNumber { Id = 3, NumberValue = 16 },new RandomNumber { Id = 4, NumberValue = 7 }, 
                new RandomNumber { Id = 5, NumberValue = 31 },new RandomNumber { Id = 6, NumberValue = 71 } 
            };
            _canProcessService = new CanProcessService();
            _calculationServicesMock = new Mock<ICalculationServices>();
            _randomNumberRepositoryMock = new Mock<IRandomNumberRepository>();
            
            _calculationServices = new CalculationServices(_randomNumberRepositoryMock.Object);
            _processController = new ProcessController(_calculationServicesMock.Object);
            _homeController = new HomeController(_canProcessService);
            _randomNumbersController = new RandomNumbersController(_randomNumberRepositoryMock.Object);

            _randomNumberRepositoryMock.Setup(x => x.GetRanNumbersAsync()).ReturnsAsync(_rNList).Verifiable();

            _file = new Mock<IFormFile>();
            _uploadModel = new UploadModel();
            _uploadModel.File = _file.Object;
        }

        [Theory]
        [InlineData("12,15,29,47,16,63,98,10,65,81")]
        [InlineData("2,5,9,7,6,3,8,11,25,87")]
        public void ShouldProcessAsMoqFileForUpload_CalculationServices(string expectedFileContents)
        {
            _file.Setup(ff => ff.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns<Stream,CancellationToken>((s, ct) => 
                {
                    byte[] buffer = Encoding.Default.GetBytes(expectedFileContents);
                    s.Write(buffer, 0, buffer.Length);
                    return Task.CompletedTask;
                }).Verifiable();

            var calculationProcess =  _calculationServices.ProcessFile(_uploadModel, false);
            try
            {
                calculationProcess.GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {

                var sss = ex.Message;
            }

            _file.Verify(x => x.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
            
            var status = calculationProcess.Status;
            var exception = calculationProcess.Exception;
            Assert.NotNull(calculationProcess);
            Assert.Equal("RanToCompletion", status.ToString());
            Assert.Null(exception);
        }

        [Fact]
        public void ShouldProcessAsMoqFileForUploadException_CalculationServices()
        {
            var expectedStatus = "Faulted";
            var expectedExceptMessage = "Input string was not in a correct format.";           

            var calculationProcess = _calculationServices.ProcessFile(_uploadModel, false);
            try
            {
                calculationProcess.GetAwaiter().GetResult();
            }
            catch (Exception)
            {
            }
            var status = calculationProcess.Status;
            var exceptionMessage = calculationProcess.Exception.InnerExceptions[0].Message;

            Assert.NotNull(calculationProcess);
            Assert.Equal(expectedStatus, status.ToString());
            Assert.Equal(expectedExceptMessage, exceptionMessage);

        }

        [Fact]
        public void ShouldProcessAsMoqCSVfileForUpload_ProcessController()
        {           
            var expectedStatus = 200;
            var expectedValue = "File Upload success 'SampleData.csv'";
            var fileName = "SampleData.csv";
            _file.Setup(f => f.FileName).Returns(fileName).Verifiable();

            var processUpload = _processController.Upload(_uploadModel);
            var objectResult = (Microsoft.AspNetCore.Mvc.OkObjectResult)processUpload.Result;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value;

            _file.Verify(f => f.FileName, Times.Once);
            Assert.Equal(expectedStatus, statusCode);
            Assert.Equal(expectedValue, value);
            Assert.NotNull(objectResult);
        }

        [Fact]
        public void ShouldReturnMoqDatabaseWithRandonNUmberList_RandomNumbersController()
        {
            var expected = 6;          
            var process = _randomNumbersController.Index();
            var viewResult = (Microsoft.AspNetCore.Mvc.ViewResult)process.Result;
            localList = (List<RandomNumber>)viewResult.Model;

            _randomNumberRepositoryMock.Verify(x => x.GetRanNumbersAsync(), Times.Once);

            Assert.NotNull(localList);
            Assert.Equal(expected, localList.Count);
        }

        [Fact]
        public void ShouldUpdateMoqDatabaseWithRandomNumbersList_IRandomNumberRepository()
        {
            var expected = 6;
            _randomNumberRepositoryMock.Setup(x => x.UpdateRanNumbers(It.IsAny<List<RandomNumber>>())).Callback<List<RandomNumber>>(listRN => 
            {
                localList = listRN;
            
            });

            _randomNumberRepositoryMock.Object.UpdateRanNumbers(_rNList.ToList());

            _randomNumberRepositoryMock.Verify(x => x.UpdateRanNumbers(It.IsAny<List<RandomNumber>>()), Times.Once);
            Assert.NotNull(localList);
            Assert.Equal(expected, localList.Count);
        }

        [Fact]
        public void ShouldReturnFequencyInMoqRandomNumberListInBinsof10s_CalculationServices()
        {            
            var dictionaryCReated= _calculationServices.FequCalAsync().Result.ValuePairs;
            var amountOfKeys = dictionaryCReated.Values;            
            var key1 = dictionaryCReated[1].Count;
            var key2 = dictionaryCReated[2].Count;
            var key4 = dictionaryCReated[4].Count;
            var key8 = dictionaryCReated[8].Count;
           

            _randomNumberRepositoryMock.Verify(x => x.GetRanNumbersAsync(), Times.Once);

            Assert.Equal(2,key1);
            Assert.Equal(2,key2);
            Assert.Equal(1,key4);
            Assert.Equal(1,key8);            
            Assert.Equal(4,amountOfKeys.Count);
        }

        [Fact]
        public void ShouldReturnFequencyInMoqRandomNumberListInBinsof10s_ProcessController_Invocation()
        {
            FreqViewModels freqVMS = new FreqViewModels();
            _calculationServicesMock.Setup(a => a.FequCalAsync()).ReturnsAsync(_calculationServices.FequCalAsync().Result).Verifiable();

            var process = _processController.FreqChart();
            var viewResult = (Microsoft.AspNetCore.Mvc.PartialViewResult)process.Result;
            freqVMS = (FreqViewModels)viewResult.Model;

            _calculationServicesMock.Verify(x => x.FequCalAsync(), Times.AtLeastOnce);
            _randomNumberRepositoryMock.Verify(x => x.GetRanNumbersAsync(), Times.Once);
            Assert.NotNull(freqVMS);
            Assert.Equal(4, freqVMS.ValuePairs.Count);
        }

        [Theory]
        [InlineData(false, 25.057932875638404)]
        [InlineData(true, 22.874658467395747)]        
        public void ShouldReturnExpectDeviationFromMoqRandomNumberList_CalculationServices(bool isPopulation,double expected)
        {
            var deviationResult = _calculationServices.StandardDevAsync(isPopulation);

            _randomNumberRepositoryMock.Verify(x => x.GetRanNumbersAsync(), Times.Once);
            Assert.Equal(expected, deviationResult.Result);

        }

        [Fact]
        public void ShouldReturnExpectedMeanOfRandomNumberListFromMoq_CalculationServices()
        {
            var expected = 23.5;

            var mean  = _calculationServices.MeansCalAsync();

            _randomNumberRepositoryMock.Verify(x => x.GetRanNumbersAsync(), Times.Once);
            Assert.NotNull(mean);
            Assert.Equal(expected, mean.Result);

        }

        [Fact]
        public void ShouldReturnExpectedMeanOfRandomNumberListFromRaw_CalculationServices()
        {
            var expected = 23.5;
            
            var mean = _calculationServices.MeansCalAsync(_rNList);

            Assert.NotNull(mean);
            Assert.Equal(expected, mean.Result);
        }

        [Fact]
        public void ShouldReturnStatusCodeForCheckUploadData_CanProcessService()
        {           
            var expected = 404;
            
            var processable = _canProcessService.CanProcess;
            var uploadResponce = _homeController.CheckUploaded();            
            var actionResult = (Microsoft.AspNetCore.Mvc.Infrastructure.IStatusCodeActionResult)uploadResponce;
            
            Assert.False(processable);
            Assert.Equal(expected, actionResult.StatusCode);

        }
       
    }
}
