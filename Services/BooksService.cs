using Microsoft.Extensions.Options;
using MongoDB.Driver;
using webAPIApp.Models;

namespace webAPIApp.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book>? _bookCollection;

        public BookService(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);
            _bookCollection = mongoDatabase.GetCollection<Book>(bookStoreDatabaseSettings.Value.BooksCollectionName);
        }

        public async Task<List<Book>> GetAsync() => await _bookCollection.Find(_ => true).ToListAsync();

        public async Task<Book?> GetAsync(string id) => await _bookCollection.Find(book => book.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Book newBook) => await _bookCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, Book updateBook) => await _bookCollection.ReplaceOneAsync(book => book.Id == id, updateBook);

        public async Task DeleteAsync(string id) => await _bookCollection.DeleteOneAsync(book => book.Id == id);
    }
}
