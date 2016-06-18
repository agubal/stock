using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Stock.DataAccess;
using Stock.Entities.Calculations;
using Stock.Entities.Common;
using Stock.Entities.Stocks;
using Stock.Services.Stocks;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Stock.Services.Tests
{
    [TestFixture]
    public class StockServiceTest
    {

        private Mock<IRepository<StockData, Guid>> _stockRespositoryMock;
        private Mock<IService<Calculation, Guid>> _calculationServiceMock;
        private IStockService _stockService;


        [SetUp]
        public void Initialize()
        {
            _stockRespositoryMock = new Mock<IRepository<StockData, Guid>>();
            _calculationServiceMock = new Mock<IService<Calculation, Guid>>();
            _stockService = new StockService(_stockRespositoryMock.Object, _calculationServiceMock.Object);
        }

        [Test]
        [Description("Test logic when input data is null")]
        public void Create_WhenEntityIsNull()
        {
            //Arrange
            StockData stockData = null;
            const string expectedError = "Stock data was not provided";

            //Act
            // ReSharper disable once ExpressionIsAlwaysNull
            ServiceResult<StockData> result = _stockService.Create(stockData);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Succeeded);
            Assert.IsNull(result.Result);
            Assert.IsNotNull(result.Errors);
            Assert.AreEqual(result.Errors.First(), expectedError);
            _stockRespositoryMock.Verify(g => g.Create(It.IsAny<StockData[]>()), Times.Never);
        }

        [Description("Test behavior for various validation errors")]
        [TestCase("", 2, 3, 200, 10, "Stock Name was not provided")]
        [TestCase("Apple", 0, 3, 200, 10, "Price is invalid or was not provided")]
        [TestCase("Apple", -1, 3, 200, 10, "Price should have positive value")]
        [TestCase("Apple", 2, 0, 200, 10, "Percentage is invalid or was not provided")]
        [TestCase("Apple", 2, -1, 200, 10, "Percentage should have positive value")]
        [TestCase("Apple", 2, 3, 0, 10, "Quantity is invalid or was not provided")]
        [TestCase("Apple", 2, 3, -1, 10, "Quantity should have positive value")]
        [TestCase("Apple", 2, 3, 200, 0, "Years is invalid or was not provided")]
        [TestCase("Apple", 2, 3, 200, -1, "Years should have positive value")]
        [TestCase("Apple", 2, 3, 200, 101, "Maximum vakue for Years is 100")]
        public void Create_WhenEntityIsInvalid(string name, decimal price, decimal persentage, int quantity, int years, string expectedError)
        {
            //Arrange
            var stockData = new StockData
            {
                Name = name,
                Price = price,
                Percentage = persentage,
                Quantity = quantity,
                Years = years
            };

            //Act
            ServiceResult<StockData> result = _stockService.Create(stockData);
            
            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Succeeded);
            Assert.IsNull(result.Result);
            Assert.IsNotNull(result.Errors);
            Assert.AreEqual(result.Errors.First(), expectedError);
            _stockRespositoryMock.Verify(g => g.Create(It.IsAny<StockData[]>()), Times.Never);
        }

        [Test]
        [Description("Test behavior, when it fails to create new Stock")]
        public void Create_WhenCreateStockFailed()
        {
            //Arrange
            var stockData = new StockData
            {
                Name = "Apple",
                Price = 2,
                Percentage = 3,
                Quantity = 200,
                Years = 10
            };
            _stockRespositoryMock.Setup(g => g.Create(stockData)).Returns(false);

            //Act
            ServiceResult<StockData> result = _stockService.Create(stockData);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Succeeded);
            Assert.IsNull(result.Result);
            Assert.IsNotNull(result.Errors);
            _stockRespositoryMock.Verify(g => g.Create(It.IsAny<StockData[]>()), Times.Once);
            _calculationServiceMock.Verify(g=>g.Create(It.IsAny<Calculation[]>()), Times.Never);
        }

        [Description("Test different Calculations")]
        [TestCase("Apple", 2, 3, 200, 10, 537.57)]
        [TestCase("HP", 3, 4, 50, 5, 182.5)]
        public void Create_CalculationsResult_PositiveTest(string name, decimal price, decimal persentage, int quantity, int years, decimal expectedPrincipal)
        {
            //Arrange
            var stockData = new StockData
            {
                Name = name,
                Price = price,
                Percentage = persentage,
                Quantity = quantity,
                Years = years
            };

            var calcSaveResult = new ServiceResult<IEnumerable<Calculation>>(new List<Calculation>().ToArray());
            _stockRespositoryMock.Setup(g => g.Create(stockData)).Returns(true);
            _calculationServiceMock.Setup(g => g.Create(It.IsAny<Calculation[]>())).Returns(calcSaveResult);

            //Act
            ServiceResult<StockData> result = _stockService.Create(stockData);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Succeeded);
            Assert.IsNotNull(result.Result);
            CollectionAssert.IsNotEmpty(result.Result.Calculations);
            Assert.IsTrue(result.Result.Calculations.Length == years + 1);
            Assert.IsTrue(result.Result.Calculations[years].Value == expectedPrincipal);
        }
    }
}
