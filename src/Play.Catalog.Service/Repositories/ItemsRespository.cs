using Play.Catalog.Service.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
namespace Play.Catalog.Service.Repositories
{

    public class ItemsRepository : IItemsRepository
    {

        private const string collectionName = "items"; // MongoDB collection name = sql table name
        private readonly IMongoCollection<Item> dbCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public ItemsRepository(IMongoDatabase database)
        {
            // var settings = MongoClientSettings.FromConnectionString("mongodb://localhost:27017");
            // var mongoClient = new MongoClient(settings); // MongoDB connection with settings
            // var database = mongoClient.GetDatabase("catalog"); // MongoDB database name = sql database name

            dbCollection = database.GetCollection<Item>(collectionName); // get collection from database
        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }
        public async Task<Item> GetAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            FilterDefinition<Item> filter = filterBuilder.Eq(existingItem => existingItem.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }
        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }

    }
}