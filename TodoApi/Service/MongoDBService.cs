using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoApi.Models;

namespace TodoApi.Service
{
    public class MongoDBService
    {
        private readonly IMongoCollection<TodoItem> _todosCollection;
        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _todosCollection = database.GetCollection<TodoItem>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<List<TodoItem>> GetAsync() =>
            await _todosCollection.Find(_ => true).ToListAsync();

        public async Task<TodoItem> GetAsync(string id) =>
            await _todosCollection.Find(x =>  x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(TodoItem todo) =>
            await _todosCollection.InsertOneAsync(todo);

        public async Task UpdateAsync(string id, TodoItem updatedTodo) =>
            await _todosCollection.ReplaceOneAsync(x => x.Id == id, updatedTodo);

        public async Task RemoveAsync(string id) =>
            await _todosCollection.DeleteOneAsync(x => x.Id == id);
    }
}
