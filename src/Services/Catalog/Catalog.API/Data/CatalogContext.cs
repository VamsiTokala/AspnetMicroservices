using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;// we can inject configuration in asp.net application
using MongoDB.Driver;

namespace Catalog.API.Data
{
    /*
     * Once CatalogContext class is created, this will create a connection with mongo database. this first usaege of catelog context class is to create mongo connection 
     * and seed database. After that whole application use existing configuration
     */
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration) //By injecting Configuration that means we can reach reach appsettings.json. Dependency Injection
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString")); // to create connection with mongo database
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));// Will create Database if it doesn't exist

            Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));//populate product collection from database

            //CatalogContextSeed  is a new class for seeding
            CatalogContextSeed.SeedData(Products);//Data seeding is the process of populating a database with an initial set of data.


        }

// public IMongoCollection<Product> Products => throw new System.NotImplementedException();
        public IMongoCollection<Product> Products { get;}

    }
}
