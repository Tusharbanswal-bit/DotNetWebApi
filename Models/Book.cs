using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace webAPIApp.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;

        [Required]
        public string Title { get; set; }
        public double Cost { get; set; }

        [Required]
        public string Author { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? CreatedByUserId { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ModifiedByUserId { get; set;}
        public DateTime ModifiedOn { get; set; }

    }
}