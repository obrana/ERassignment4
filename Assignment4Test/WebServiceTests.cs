using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Xunit;
using EFassinment4;
using assignment;

namespace Assignment4Test
{
    
    public class WebServiceTests
    {
        private const string CategoriesApi = "http://localhost:5001/api/categories";
        private const string ProductsApi = "http://localhost:5001/api/products";

        /* /api/categories */

        [Fact]
        public void ApiCategories_GetWithNoArguments_OkAndAllCategories()
        {
            var (data, statusCode) = GetArray(CategoriesApi);

            Assert.AreEqual(HttpStatusCode.OK, statusCode);
            Assert.AreEqual(9, data.Count);
            Assert.AreEqual("Beverages", data.First()["name"]);
            Assert.AreEqual("Testing", data.Last()["name"]);
        }

        [Fact]
        public void ApiCategories_GetWithValidCategoryId_OkAndCategory()
        {
            var (category, statusCode) = GetObject($"{CategoriesApi}/1");

            Assert.AreEqual(HttpStatusCode.OK, statusCode);
            Assert.AreEqual("Beverages", category["name"]);
        }

        [Fact]
        public void ApiCategories_GetWithInvalidCategoryId_NotFound()
        {
            var (_, statusCode) = GetObject($"{CategoriesApi}/0");

            Assert.AreEqual(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public void ApiCategories_PostWithCategory_Created()
        {
            var newCategory = new
            {
                Name = "Created",
                Description = ""
            };
            var (category, statusCode) = PostData(CategoriesApi, newCategory);

            Assert.AreEqual(HttpStatusCode.Created, statusCode);

            DeleteData($"{CategoriesApi}/{category["id"]}");
        }

        [Fact]
        public void ApiCategories_PutWithValidCategory_Ok()
        {

            var data = new
            {
                Name = "Created",
                Description = "Created"
            };
            var (category, _) = PostData($"{CategoriesApi}", data);

            var update = new
            {
                Id = category["id"],
                Name = category["name"] + "Updated",
                Description = category["description"] + "Updated"
            };

            var statusCode = PutData($"{CategoriesApi}/{category["id"]}", update);

            Assert.AreEqual(HttpStatusCode.OK, statusCode);

            var (cat, _) = GetObject($"{CategoriesApi}/{category["id"]}");

            Assert.AreEqual(category["name"] + "Updated", cat["name"]);
            Assert.AreEqual(category["description"] + "Updated", cat["description"]);

            DeleteData($"{CategoriesApi}/{category["id"]}");
        }

        [Fact]
        public void ApiCategories_PutWithInvalidCategory_NotFound()
        {
            var update = new
            {
                Id = -1,
                Name = "Updated",
                Description = "Updated"
            };

            var statusCode = PutData($"{CategoriesApi}/-1", update);

            Assert.AreEqual(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public void ApiCategories_DeleteWithValidId_Ok()
        {

            var data = new
            {
                Name = "Created",
                Description = "Created"
            };
            var (category, _) = PostData($"{CategoriesApi}", data);

            var statusCode = DeleteData($"{CategoriesApi}/{category["id"]}");

            Assert.AreEqual(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void ApiCategories_DeleteWithInvalidId_NotFound()
        {

            var statusCode = DeleteData($"{CategoriesApi}/-1");

            Assert.AreEqual(HttpStatusCode.NotFound, statusCode);
        }

        /* /api/products */

        [Fact]
        public void ApiProducts_ValidId_CompleteProduct()
        {
            var (product, statusCode) = GetObject($"{ProductsApi}/1");

            Assert.AreEqual(HttpStatusCode.OK, statusCode);
            Assert.AreEqual("Chai", product["name"]);
            Assert.AreEqual("Beverages", product["category"]["name"]);
        }

        [Fact]
        public void ApiProducts_InvalidId_CompleteProduct()
        {
            var (_, statusCode) = GetObject($"{ProductsApi}/-1");

            Assert.AreEqual(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public void ApiProducts_CategoryValidId_ListOfProduct()
        {
            var (products, statusCode) = GetArray($"{ProductsApi}/category/1");

            Assert.AreEqual(HttpStatusCode.OK, statusCode);
            Assert.AreEqual(12, products.Count);
            Assert.AreEqual("Chai", products.First()["name"]);
            Assert.AreEqual("Beverages", products.First()["categoryName"]);
            Assert.AreEqual("Lakkalikööri", products.Last()["name"]);
        }

        [Fact]
        public void ApiProducts_CategoryInvalidId_EmptyListOfProductAndNotFound()
        {
            var (products, statusCode) = GetArray($"{ProductsApi}/category/1000001");

            Assert.AreEqual(HttpStatusCode.NotFound, statusCode);
            Assert.AreEqual(0, products.Count);
        }

        [Fact]
        public void ApiProducts_NameContained_ListOfProduct()
        {
            var (products, statusCode) = GetArray($"{ProductsApi}/name/em");

            Assert.AreEqual(HttpStatusCode.OK, statusCode);
            Assert.AreEqual(4, products.Count);
            Assert.AreEqual("NuNuCa Nuß-Nougat-Creme", products.First()["productName"]);
            Assert.AreEqual("Flotemysost", products.Last()["productName"]);
        }

        [Fact]
        public void ApiProducts_NameNotContained_EmptyListOfProductAndNotFound()
        {
            var (products, statusCode) = GetArray($"{ProductsApi}/name/RAWDATA");

            Assert.AreEqual(HttpStatusCode.NotFound, statusCode);
            Assert.AreEqual(0, products.Count);
        }



        // Helpers

        (JArray, HttpStatusCode) GetArray(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JArray)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) GetObject(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) PostData(string url, object content)
        {
            var client = new HttpClient();
            var requestContent = new StringContent(
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");
            var response = client.PostAsync(url, requestContent).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        HttpStatusCode PutData(string url, object content)
        {
            var client = new HttpClient();
            var response = client.PutAsync(
                url,
                new StringContent(
                    JsonConvert.SerializeObject(content),
                    Encoding.UTF8,
                    "application/json")).Result;
            return response.StatusCode;
        }

        HttpStatusCode DeleteData(string url)
        {
            var client = new HttpClient();
            var response = client.DeleteAsync(url).Result;
            return response.StatusCode;
        }
    }
}