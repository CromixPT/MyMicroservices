using MyMicroServices.Catalog.API.Domain.Entities;

namespace MyMicroServices.Catalog.API.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProducts();
    Task<Product> GetProductById(string id);
    Task<IEnumerable<Product>> GetProductByName(string name);
    Task<IEnumerable<Product>> GetProductByCategory(string category);
    Task AddProduct(Product product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(string id);

}