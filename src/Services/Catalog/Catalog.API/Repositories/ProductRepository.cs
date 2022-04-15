using MongoDB.Driver;
using MyMicroServices.Catalog.API.Domain.Entities;
using MyMicroServices.Catalog.API.Infrastructure;

namespace MyMicroServices.Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _context;

    private readonly FindOptions _findOptions = new()
    {
        Collation = new Collation("en", strength: CollationStrength.Primary)
    };
    
    private readonly ReplaceOptions _replaceOptions = new()
    {
        Collation = new Collation("en", strength: CollationStrength.Primary)
    };
    
    private readonly DeleteOptions deleteOptions = new()
    {
        Collation = new Collation("en", strength: CollationStrength.Primary)
    };

    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
        => await _context
            .Products
            .Find(p => true)
            .ToListAsync();

    public async Task<Product> GetProductById(string id)
        => await _context
            .Products
            .Find(p => p.Id.Equals(id),_findOptions)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Product>> GetProductByName(string name)
        => await _context
            .Products
            .Find(p => p.Name.Equals(name),_findOptions)
            .ToListAsync();

    public async Task<IEnumerable<Product>> GetProductByCategory(string category)
        => await _context
            .Products
            .Find(p => p.Category.Equals(category),_findOptions)
            .ToListAsync();

    public async Task AddProduct(Product product)
        => await _context.Products.InsertOneAsync(product);

    public async Task UpdateProduct(Product product)
        => await _context.Products.ReplaceOneAsync(p => p.Id.Equals(product.Id),product,_replaceOptions);

    public async Task DeleteProduct(string id)
        => await _context.Products.DeleteOneAsync(p => p.Id.Equals(id),deleteOptions);
}