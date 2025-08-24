using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Op.Prueba.Application.Interfaces;

namespace Op.Prueba.Persistence.Repository
{
    public class MongoRepository<T> : IRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(spec);
            return await query.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
            await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);

        public async Task UpdateAsync(string id, T entity, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            await _collection.DeleteOneAsync(filter, cancellationToken);
        }

        public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(spec);
            return (int)await query.CountDocumentsAsync(cancellationToken);
        }

        private IFindFluent<T, T> ApplySpecification(ISpecification<T> spec)
        {
            var filter = Builders<T>.Filter.Empty;

            foreach (var criteria in spec.WhereExpressions)
            {
                filter &= Builders<T>.Filter.Where(criteria.Filter);
            }

            var query = _collection.Find(filter);

            if (spec.OrderExpressions.Any())
            {
                foreach (var order in spec.OrderExpressions)
                {
                    if (order.OrderType == OrderTypeEnum.OrderBy)
                        query = query.SortBy(order.KeySelector);
                    else if (order.OrderType == OrderTypeEnum.OrderByDescending)
                        query = query.SortByDescending(order.KeySelector);
                }
            }

            if (spec.Skip > 0)
                query = query.Skip(spec.Skip);

            if (spec.Take > 0)
                query = query.Limit(spec.Take);

            return query;
        }
    }
}