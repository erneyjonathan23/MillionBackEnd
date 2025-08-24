using MongoDB.Bson.Serialization.Attributes;

namespace Op.Prueba.Domain.Entities
{
    public class PropertyImage
    {
        [BsonElement("file")]
        public string File { get; set; }

        [BsonElement("enabled")]
        public bool Enabled { get; set; }
    }
}