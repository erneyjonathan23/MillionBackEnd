using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Op.Prueba.Domain.Entities
{
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdProperty { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("codeInternal")]
        public string CodeInternal { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("idOwner")]
        public string IdOwner { get; set; }

        [BsonElement("images")]
        public List<PropertyImage> Images { get; set; } = new();
    }
}
