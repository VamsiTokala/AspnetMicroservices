using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration) //By injecting Configuration that means we can reach reach appsettings.json. Dependency Injection
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString")); // to create connection with mongo database
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));// Will create Database if it doesn't exist

            Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
           // CatalogContextSeed.SeedData(Products);
        }

// public IMongoCollection<Product> Products => throw new System.NotImplementedException();
        public IMongoCollection<Product> Products { get;}

    }
}
