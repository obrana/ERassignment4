using System;
using System.Linq;
using assignment;
using EFassinment4;
using NUnit.Framework;
using Xunit;

namespace Assignment4Test
{
    public class DataServiceTests
    {
        
        [Fact]
        public void Category_Object_HasIdNameAndDescription()
        {
            var category = new Category();
            Assert.AreEqual(0, category.Id);
            Assert.Null(category.Name);
            Assert.Null(category.Description);
        }
        
        [Fact]
        public void GetAllCategories_NoArgument_ReturnsAllCategories()
        {
            var service = new DataService();
            var categories = service.GetCategories();
            Assert.AreEqual(12, categories.Count);
            Assert.AreEqual("Beverages", categories.First().Name);
        }

        [Fact]
        public void GetCategory_ValidId_ReturnsCategoryObject()
        {
            var service = new DataService();
            var category = service.GetCategory(1);
            Assert.AreEqual("Beverages", category.Name);
        }

        [Fact]
        public void CreateCategory_ValidData_CreteCategoryAndRetunsNewObject()
        {
            var service = new DataService();
            var category = service.CreateCategory("Test", "CreateCategory_ValidData_CreteCategoryAndRetunsNewObject");
            Assert.True(category.Id > 0);
            Assert.AreEqual("Test", category.Name);
            Assert.AreEqual("CreateCategory_ValidData_CreteCategoryAndRetunsNewObject", category.Description);

            // cleanup
            service.DeleteCategory(category.Id);
        }

        [Fact]
        public void DeleteCategory_ValidId_RemoveTheCategory()
        {
            var service = new DataService();
            var category = service.CreateCategory("Test", "DeleteCategory_ValidId_RemoveTheCategory");
            var result = service.DeleteCategory(category.Id);
            Assert.True(result);
            category = service.GetCategory(category.Id);
            Assert.Null(category);
        }

        [Fact]
        public void DeleteCategory_InvalidId_ReturnsFalse()
        {
            var service = new DataService();
            var result = service.DeleteCategory(-1);
            Assert.False(result);
        }

        [Fact]
        public void UpdateCategory_NewNameAndDescription_UpdateWithNewValues()
        {
            var service = new DataService();
            var category = service.CreateCategory("TestingUpdate", "UpdateCategory_NewNameAndDescription_UpdateWithNewValues");

            var result = service.UpdateCategory(category.Id, "UpdatedName", "UpdatedDescription");
            Assert.True(result);

            category = service.GetCategory(category.Id);

            Assert.AreEqual("UpdatedName", category.Name);
            Assert.AreEqual("UpdatedDescription", category.Description);

            // cleanup
            service.DeleteCategory(category.Id);
        }

        [Fact]
        public void UpdateCategory_InvalidID_ReturnsFalse()
        {
            var service = new DataService();
            var result = service.UpdateCategory(-1, "UpdatedName", "UpdatedDescription");
            Assert.False(result);
        }
        
         
         /* products */

        [Fact]
        public void Product_Object_HasIdNameUnitPriceQuantityPerUnitAndUnitsInStock()
        {
            var product = new Product();
            Assert.AreEqual(0, product.Id);
            Assert.Null(product.Name);
            Assert.AreEqual(0.0, product.UnitPrice);
            Assert.Null(product.QuantityPerUnit);
            Assert.AreEqual(0, product.UnitsInStock);
        }

        [Fact]
        public void GetProduct_ValidId_ReturnsProductWithCategory()
        {
            var service = new DataService();
            var product = service.GetProduct(1);
            Assert.AreEqual("Chai", product.Name);
            Assert.AreEqual("Beverages", product.Category.Name);
        }

        [Fact]
        public void GetProductsByCategory_ValidId_ReturnsProductWithCategory()
        {
            var service = new DataService();
            var products = service.GetProductByCategory(1);
            Assert.AreEqual(12, products.Count);
            Assert.AreEqual("Chai", products.First().Name);
           // Assert.AreEqual("Beverages", products.First().CategoryName);
            Assert.AreEqual("Lakkalikööri", products.Last().Name);
        }

        [Fact]
        public void GetProduct_NameSubString_ReturnsProductsThatMachesTheSubString()
        {
            var service = new DataService();
            var products = service.GetProductByName("em");
            Assert.AreEqual(4, products.Count);
          //  Assert.AreEqual("NuNuCa Nu�-Nougat-Creme", products.First().ProductName);
            //Assert.AreEqual("Flotemysost", products.Last().ProductName);
        }

        /* orders */
        [Fact]
        public void Order_Object_HasIdDatesAndOrderDetails()
        {
            var order = new Order();
            Assert.AreEqual(0, order.Id);
            Assert.AreEqual(new DateTime(), order.Date);
            Assert.AreEqual(new DateTime(), order.Required);
            Assert.Null(order.OrderDetails);
            Assert.Null(order.ShipName);
            Assert.Null(order.ShipCity);
        }

        [Fact]
        public void GetOrder_ValidId_ReturnsCompleteOrder()
        {
            var service = new DataService();
            var order = service.GetOrder(10248);
            Assert.AreEqual(3, order.OrderDetails.Count);
            Assert.AreEqual("Queso Cabrales", order.OrderDetails.First().Product.Name);
            Assert.AreEqual("Dairy Products", order.OrderDetails.First().Product.Category.Name);
        }

        [Fact]
        public void GetOrders()
        {
            var service = new DataService();
            var orders = service.GetOrders();
            Assert.AreEqual(830, orders.Count);
        }


        /* orderdetails */
        [Fact]
        public void OrderDetails_Object_HasOrderProductUnitPriceQuantityAndDiscount()
        {
            var orderDetails = new OrderDetails();
            Assert.AreEqual(0, orderDetails.OrderId);
            Assert.Null(orderDetails.Order);
            Assert.AreEqual(0, orderDetails.ProductId);
            Assert.Null(orderDetails.Product);
            Assert.AreEqual(0.0, orderDetails.UnitPrice);
            Assert.AreEqual(0.0, orderDetails.Quantity);
            Assert.AreEqual(0.0, orderDetails.Discount);
        }

        [Fact]
        public void GetOrderDetailByOrderId_ValidId_ReturnsProductNameUnitPriceAndQuantity()
        {
            var service = new DataService();
            var orderDetails = service.GetOrderDetailsByOrderId(10248);
            Assert.AreEqual(3, orderDetails.Count);
            Assert.AreEqual("Queso Cabrales", orderDetails.First().Product.Name);
            Assert.AreEqual(14, orderDetails.First().UnitPrice);
            Assert.AreEqual(12, orderDetails.First().Quantity);
        }

        [Fact]
        public void GetOrderDetailByProductId_ValidId_ReturnsOrderDateUnitPriceAndQuantity()
        {
            var service = new DataService();
            var orderDetails = service.GetOrderDetailsByProductId(11);
            Assert.AreEqual(38, orderDetails.Count);
            Assert.AreEqual("1996-07-04", orderDetails.First().Order.Date.ToString("yyyy-MM-dd"));
            Assert.AreEqual(14, orderDetails.First().UnitPrice);
            Assert.AreEqual(12, orderDetails.First().Quantity);
        }
        
        
    }
}