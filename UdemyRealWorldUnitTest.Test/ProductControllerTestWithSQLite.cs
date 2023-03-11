using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UdemyRealWorldUnitTest.Test;
using UdemyRealWorldUnitTest.Web.Controllers;
using UdemyRealWorldUnitTest.Web.Models;
using Xunit;

namespace UdemyRealWordUnitTest.Test
{
    public class ProductControllerTestWithSQLite : ProductControllerTest
    {
        public ProductControllerTestWithSQLite()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            SetContextOptions(new DbContextOptionsBuilder<UdemyUnitTestDBContext>().UseSqlite(connection).Options);
        }

        [Fact]
        public async Task Create_ModelValidProduct_ReturnRedirectToActionWithSaveProduct()
        {
            var newProduct = new Product { Name = "Kalem 30", Price = 200, Stock = 100, Color = "Mavi" };
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

        [Theory]
        [InlineData(1)]
        public async Task DeleteCategory_ExistCategoryId_DeletedAllProducts(int categoryId)
        {
            using (var context = new UdemyUnitTestDBContext(_contextOptions))

            {
                var category = await context.Categories.FindAsync(categoryId);

                Assert.NotNull(category);

                context.Categories.Remove(category);

                context.SaveChanges();
            }

            using (var context = new UdemyUnitTestDBContext(_contextOptions))
            {
                var products = await context.Products.Where(x => x.CategoryId == categoryId).ToListAsync();

                Assert.Empty(products);
            }
        }
    }
}