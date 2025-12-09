using Play.Catalog.Service.Entities;
using MongoDB.Driver;
namespace Play.Catalog.Service.Repositories
{
    public class ItemsRepository
    {
        private const string collectionName = "items"; // MongoDB collection name = sql table name
        private readonly IMongoCollection<Item> dbCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public ItemsRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017"); // MongoDB connection string
            var database = mongoClient.GetDatabase("catalog"); // MongoDB database name = sql database name
            dbCollection = database.GetCollection<Item>(collectionName); // get collection from database
        }
    }
}