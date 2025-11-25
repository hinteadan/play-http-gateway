using H.Necessaire;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace H.Replication.MongoDB
{
    internal class MongoDbDebugger : IDebug, ImADependency
    {
        const string dbName = "HDebug";
        const string collectionName = "HDebugData";
        MongoDbConfig mongoDbConfig;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            mongoDbConfig = dependencyProvider.Get<MongoDbConfigProvider>()?.GetMongoDbConfig()?.Return();
        }

        public async Task Debug()
        {
            using IMongoClient mongoClient = new MongoClient(mongoDbConfig.ConnectionString);
            IMongoCollection<BsonDocument> mongoCollection = mongoClient.GetDatabase(dbName).GetCollection<BsonDocument>(collectionName);

            MongoTestData newData = GenerateDummyData();

            BsonDocument newDoc = newData.ToBsonDocument();

            MongoTestData justToTest = BsonSerializer.Deserialize<MongoTestData>(newDoc);

            await mongoCollection.ReplaceOneAsync(x => x["ID"] == newDoc["ID"], newDoc, new ReplaceOptions { IsUpsert = true });
        }

        static MongoTestData GenerateDummyData()
        {
            return
                new MongoTestData
                {
                    DisplayName = $"Name {DateTime.Now.PrintTimeStampAsIdentifier()}",
                    Description = $"Description {DateTime.Now.PrintTimeStampAsIdentifier()}",
                };
        }

        class MongoTestData : EphemeralTypeBase, IStringIdentity
        {
            public string ID { get; set; } = Guid.NewGuid().ToString();
            public string DisplayName { get; set; }
            public string Description { get; set; }
        }
    }
}
