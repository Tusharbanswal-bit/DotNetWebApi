using MongoDB.Driver;
using webAPIApp.Models;
using Microsoft.Extensions.Options;

namespace webAPIApp.Services {
    public class UserService {
        private readonly IMongoCollection<User>? _userCollection;

        public UserService(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings) {
            var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);
            _userCollection = mongoDatabase.GetCollection<User>(bookStoreDatabaseSettings.Value.UsersCollectionName);
        }

        public async Task<List<User>> GetAsync() => await _userCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetAsync(string username) => await _userCollection.Find(user => user.Username == username).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) => await _userCollection.InsertOneAsync(newUser);

    }
}