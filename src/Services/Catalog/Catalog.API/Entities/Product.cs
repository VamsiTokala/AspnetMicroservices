using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] //Object id is 24  charecter
        // by this way we make ID column as Bson ID in Mongo DB nd this ID will be generating from Mongo DB as a object id
        public string Id { get; set; }

        [BsonElement("Name")]
        public string  Name { get; set; }
        public string Category { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
        public decimal Price { get; set; }
    }
}
