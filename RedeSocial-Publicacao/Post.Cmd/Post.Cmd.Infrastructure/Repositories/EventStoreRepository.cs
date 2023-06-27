using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Cmd.Infrastructure.Config;

namespace Post.Cmd.Infrastructure.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _eventStoreCollection;

        public EventStoreRepository(IOptions<MongoDbConfig> config)
        {
            MongoClient mongoClient = new(config.Value.ConnectionString);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(config.Value.Database);

            _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.Collection);
        }

        public async Task<List<EventModel>> FindAllAsync()
        {
            //listar todos
            return await _eventStoreCollection.Find(_ => true).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
        {
            try
            {
                return await _eventStoreCollection.Find(_ => _.AggregateIdentifier == aggregateId).ToListAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SaveAsync(EventModel @event)
        {
            await _eventStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
        }
    }
}