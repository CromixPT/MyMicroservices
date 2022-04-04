using MongoDB.Driver;
using MyMicroServices.Catalog.API.Domain.Entities;

namespace MyMicroServices.Catalog.API.Infrastructure;

public interface ICatalogContext
{
    IMongoCollection<Product> Products { get; }

}