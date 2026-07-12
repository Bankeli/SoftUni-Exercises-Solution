using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext dbContext = new ProductShopContext();

            string jsonFileName = "users-and-products.json";
            string jsonFilePath = GetJsonResultPath(jsonFileName);

            string result = GetUsersWithProducts(dbContext);

            File.WriteAllText(jsonFilePath, result);
            Console.WriteLine(result);
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IEnumerable<ImportUserDto>? importUserDtos
                = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

            if (importUserDtos == null)
            {
                importUserDtos = Array.Empty<ImportUserDto>();
            }

            var usersToPersist = new List<User>();
            foreach (var userDto in importUserDtos)
            {
                if (!IsValid(userDto))
                    { continue; }

                User newUser = new User()
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Age = userDto.Age
                };

                usersToPersist.Add(newUser);
            }

            context.Users.AddRange(usersToPersist);

            context.SaveChanges();

            return $"Successfully imported {usersToPersist.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IEnumerable<ImportProductDto>? importProductDtos
                = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);

            if (importProductDtos == null)
            {
                importProductDtos = Array.Empty<ImportProductDto>();
            }

            var productsToPersist = new List<Product>();

            foreach (var productDto in importProductDtos)
            {
                if (!IsValid(productDto)) continue;

                Product newProduct = new Product()
                {
                    Name = productDto.ProductName,
                    Price = productDto.Price,
                    SellerId = productDto.SellerId,
                    BuyerId = productDto.BuyerId
                };
                productsToPersist.Add(newProduct);
            }

            context.Products.AddRange(productsToPersist);
            context.SaveChanges();

            return $"Successfully imported {productsToPersist.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IEnumerable<ImportCategoriesDto>? importCategoriesDtos
                = JsonConvert.DeserializeObject<ImportCategoriesDto[]>(inputJson);

            if (importCategoriesDtos == null)
            {
                importCategoriesDtos = Array.Empty<ImportCategoriesDto>();
            }

            var categoriesToPersist = new List<Category>();

            foreach  (var categoryDto in importCategoriesDtos)
            {
                if (!IsValid(categoryDto)) continue;

                var newCategory = new Category()
                {
                    Name = categoryDto.Name
                };

                categoriesToPersist.Add(newCategory);
            }

            context.Categories.AddRange(categoriesToPersist);
            context.SaveChanges();

            return $"Successfully imported {categoriesToPersist.Count}";
        }


        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {

            IEnumerable<ImportProductCategoriesDto>? importProdCatDtos
                = JsonConvert.DeserializeObject<ImportProductCategoriesDto[]>(inputJson);

            if (importProdCatDtos == null)
            {
                importProdCatDtos = Array.Empty<ImportProductCategoriesDto>();
            }

            var productsCategoriesToPersist = new List<CategoryProduct>();

            foreach (var  prodCategoriesDto in importProdCatDtos)
            {
                if (!IsValid(prodCategoriesDto)) continue;

                var newProduct = new CategoryProduct()
                {
                    CategoryId = prodCategoriesDto.CategoryId,
                    ProductId = prodCategoriesDto.ProductId,

                };

                productsCategoriesToPersist.Add(newProduct);
            }

            context.AddRange(productsCategoriesToPersist);
            context.SaveChanges();

            return $"Successfully imported {productsCategoriesToPersist.Count}";
        }


        //problem 6//
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                        .Where(p => p.BuyerId != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                            buyerFirstName = p.Buyer!.FirstName,
                            buyerLastName = p.Buyer.LastName
                        })
                })
                .ToList();

            string result = JsonConvert.SerializeObject(users, Formatting.Indented);

            return result;
        }


        //problem 7//
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
        .Select(c => new
        {
            category = c.Name,
            productsCount = c.CategoriesProducts.Count,
            averagePrice = c.CategoriesProducts.Average(cp => cp.Product.Price),
            totalRevenue = c.CategoriesProducts.Sum(cp => cp.Product.Price)
        })
        .OrderByDescending(c => c.productsCount)
        .ToList()
        .Select(c => new
        {
            c.category,
            c.productsCount,
            averagePrice = c.averagePrice.ToString("F2"),
            totalRevenue = c.totalRevenue.ToString("F2")
        });

            string result = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return result;
        }

        //problem 8//
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Count(p => p.BuyerId != null),
                        products = u.ProductsSold
                            .Where(p => p.BuyerId != null)
                            .Select(p => new
                            {
                                name = p.Name,
                                price = p.Price
                            })
                    }
                })
                .OrderByDescending(u => u.soldProducts.count)
                .ToList();

            var resultObject = new
            {
                usersCount = users.Count,
                users
            };

            string result = JsonConvert.SerializeObject(resultObject, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return result;
        }

        //problem 5//

        public static string GetProductsInRange(ProductShopContext context)
        {
            var getProduct = context.Products
                .Where(p => (p.Price >= 500 && p.Price <= 1000))
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName

                })
                .ToList();


            string result = JsonConvert.SerializeObject (getProduct, Formatting.Indented);

            return result;
                
        }

        private static string GetJSONFilePath (string jsonFile)
        {
            string jsonFolderPath = "../../../Datasets";
            string jsonFilePath = Path.Combine(jsonFolderPath, jsonFile);

            return Path.GetFullPath(jsonFilePath);
        }

        private static string GetJsonResultPath (string jsonFileName)
        {
            string jsonFolderPath = "../../../Results";
            string jsonFilePath = Path.Combine(jsonFolderPath, jsonFileName);

            return Path.GetFullPath(jsonFilePath);
        }
        

        private static bool IsValid (object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            ICollection<ValidationResult> validResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validResult);

            return isValid;

        }
    }
}
