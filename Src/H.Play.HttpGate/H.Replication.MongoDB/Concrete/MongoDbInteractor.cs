using H.Necessaire;

namespace H.Replication.MongoDB.Concrete
{
    internal class MongoDbInteractor
    {
        #region Construct
        readonly ImALogger log;
        readonly MongoDbConfigProvider mongoDbConfigProvider;
        public MongoDbInteractor(MongoDbConfigProvider mongoDbConfigProvider)
        {
            this.mongoDbConfigProvider = mongoDbConfigProvider;
            this.log = HApp.Deps.GetLogger<MongoDbInteractor>("H.Replication.MongoDB");
        }
        #endregion

        public async Task<OperationResult<HMongoSession>> NewSession()
        {
            return
                await
                    HSafe.Run(
                        async () => (HMongoSession)(await mongoDbConfigProvider.GetMongoDbConfig().LogError(log, "mongoDbConfigProvider.GetMongoDbConfig()")).ThrowOnFailOrReturn()
                    )
                    .LogError(log, "(HMongoSession)MongoDbConfig")
                    ;
        }
    }
}
