using System;
using Catalog.API.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Catalog.API.Repositories
{
	//abtraction layer for data operations
	public interface IProductRepository
    {
        //Task - async method
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(string id); //aync method
        Task<IEnumerable<Product>> GetProductByName(string name); //search  by using name
        Task<IEnumerable<Product>> GetProductByCategory(string categoryName);//category name

        //elow are for crud operations
        Task CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string id);
    }

}