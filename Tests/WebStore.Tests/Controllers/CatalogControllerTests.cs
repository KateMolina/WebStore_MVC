using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assert = Xunit.Assert;
using System.Threading.Tasks;
using Moq;
using WebStore_MVC.Services.Interfaces;
using WebStore_MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using WebStore_MVC.ViewModels;
using WebStore.Domain.Entities;

namespace WebStore.Tests.Controllers
{
    [TestClass]
   public class CatalogControllerTests
    {
        [TestMethod]
        public void ProductDetails_Returns_CorrectView()
        {
            const decimal price = 10m;
            const int expectedId = 1;
            const string expectedName = "Product 1";

            var productData_mock = new Mock<IProductData>();
            productData_mock.Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns<int>(id => new Product
                {
                    Id = id,
                    Name = $"Product {id}",
                    Order = 1,
                    Price = price,
                    Brand = new Brand { Id = 1, Name = "brand", Order = 1 },
                    Section = new Section { Id = 1, Name = "section", Order = 1 },
                    ImageUrl = $"img_{id}.png"
                });
            var controller = new CatalogController(productData_mock.Object);

            var res = controller.ProductDetails(expectedId);

            var view_result = Assert.IsType<ViewResult>(res);
            var model_view = Assert.IsAssignableFrom<ProductViewModel>(view_result.Model);

            Assert.Equal(expectedId, model_view.Id);
            Assert.Equal(expectedName, model_view.Name);
            Assert.Equal(price, model_view.Price);

            productData_mock.Verify(s => s.GetProductById(It.IsAny<int>()));
            productData_mock.VerifyNoOtherCalls();
        }
    }
}
