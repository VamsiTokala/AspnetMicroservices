using Catalog.API.Entities;
using MongoDB.Driver;

//In order to manage data operations for the catalog related entities. 
namespace Catalog.API.Data
{
    //This interface basically sorts the  product collection of mongo database
    public interface ICatalogContext
    { 
        //property under the local context interface, that means any class which inherit from the interface should be provided for the collection, which returning Imongocolectiom
        IMongoCollection<Product> Products { get; }
    }
}
