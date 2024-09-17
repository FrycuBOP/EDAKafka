using CQRS.Core.Events;
using CQRS.Core.Infrastructure;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Cmd.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _eventsStoreCollection;

        public EventStoreRepository(IOptions<MongoDbConfig> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);

            _eventsStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.Collection);
        }
        public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId) => await _eventsStoreCollection.Find(x => x.AggregateIdentifier == aggregateId).ToListAsync().ConfigureAwait(false);

        public async Task SaveAsync(EventModel @event)
        {
            await _eventsStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
        }
    }
}
