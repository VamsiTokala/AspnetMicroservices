using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;


namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        /*injecting ICatalogContext. need to use for data operations
        //context object is a mongo db driver and almost every mongocli commands includes in this context class 
        //is a method member. so we have implimented these methods into our API requirments
        //for example find filter,replace one and so on. All these methods are same as maongo cli commands  */
        private readonly ICatalogContext _context;
        

        //Generating the contructor for the ICatlogContext, basically injecting into my repository
        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //impliment interface members one by one

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context  //context object for performing database operations
                            .Products // product collection under catalogue context
                            .Find(p => true) //mongo cli command which is Find
                            .ToListAsync();
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context
                           .Products
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync(); //Asynchronously returns the first element of a sequence that satisfies a specified condition or a default value if no such element is found
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            //FilterDefinition comes from mongo db driver nuget package in order to perform filter operation
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _context
                            .Products
                            .Find(filter)// filter
                            .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            return await _context
                            .Products
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {//insertone, insert many options are there
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context
                                        .Products
                                        .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Products
                                                .DeleteOneAsync(filter);

            //verifying if the operations performed or not
            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }






    }
}