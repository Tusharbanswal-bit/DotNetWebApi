using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace webAPIApp.Models {
    public class User {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id {get; set;} = null!;

        [Required]
        public string Username {get; set;}

        [Required]
        public string Password {get; set;}
    }
}