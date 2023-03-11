using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UdemyRealWorldUnitTest.Web.Controllers;
using UdemyRealWorldUnitTest.Web.Models;
using Xunit;

namespace UdemyRealWorldUnitTest.Test
{
    public class ProductControllerTestWithInMemory : ProductControllerTest
    {
        public ProductControllerTestWithInMemory()
        {
            SetContextOptions(new DbContextOptionsBuilder<UdemyUnitTestDBContext>().UseInMemoryDatabase
                ("UdemyUnitTestInMemoryDB").Options);
        }

        [Fact]
        public async Task Create_ModelValidProduct_ReturnsRedirectToActionWithSaveProduct()
        {
            var newProduct = new Product { Name = "Kalem 30", Price = 300, Stock = 30 };

            using (var context = new UdemyUnitTestDBContext(_contextOptions))
            {
                var category = context.Categories.First();

                newProduct.CategoryId = category.Id;

                var controller = new ProductsController(context);

                var result = await controller.Create(newProduct);

                var redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);
            }

            using (var context = new UdemyUnitTestDBContext(_contextOptions))
            {
                var product = context.Products.FirstOrDefault(x => x.Name == newProduct.Name);

                Assert.Equal(newProduct.Name, product.Name);
            }

        }
    }
}
