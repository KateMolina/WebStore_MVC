using WebStore_MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore_MVC.Controllers;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore_MVC.Services.Interfaces;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{

    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_Returns_Views()
        {
            var configuration_mock = new Mock<IConfiguration>();
            var productData_mock = new Mock<IProductData>();
            productData_mock.Setup(s => s.GetProducts(It.IsAny<ProductFilter>()))
                .Returns(Enumerable.Empty<Product>());
            //.Returns(Enumerable.Range(1, 10).Select(i => new Product { Name = $"Product {i}" }));

            var controller = new HomeController(configuration_mock.Object);

            var result = controller.Index(productData_mock.Object);

            Assert.IsType<ViewResult>(result);
        }

        public void Privacy_Returns_View()
        {
            var configuration_mock = new Mock<IConfiguration>();

            var controller = new HomeController(configuration_mock.Object);

            var result = controller.Privacy();

            Assert.IsType<ViewResult>(result);

        }
    }
}
