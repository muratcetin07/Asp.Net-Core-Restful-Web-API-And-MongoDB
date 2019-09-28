using Data.Abstract;
using Data.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Utilities;
using MongoDB.Driver;
using System.Linq;
using Model.Core;

namespace Data.Concrete
{
    public class EntityBaseMongoData<T> : IData<T> where T : BaseModel
    {
        private static IMongoDatabase _database;
        private readonly string collectionName = string.Empty;
        private static readonly Object sync = new object();

        public static IMongoDatabase database
        {
            get
            {
                if (_database == null)
                {
                    lock (sync)
                    {
                        if (_database == null)
                        {
                            var client = new MongoClient(Configuration.Configuration.MongoDbConnectionString);
                            _database = client.GetDatabase(Configuration.Configuration.MongoDbName);
                        }
                    }
                }
                return _database;
            }
        }


        public EntityBaseMongoData(string collectionName)
        {
            this.collectionName = collectionName;
        }

        public DataResult<T> Insert(T t)
        {
            try
            {
                var collection = database.GetCollection<T>(collectionName);
                collection.InsertOne(t);
                return new DataResult<T>(t);

            }
            catch (Exception ex)
            {
                return new DataResult<T>(null) { Code = ResponseCode.BadRequest, Message = ex.Message };
            }
        }

        public DataResult<bool> Update(T data, Expression<Func<T, bool>> predicate, bool IsUpSert = false)
        {
            try
            {
                var collection = database.GetCollection<T>(collectionName);

                var result = collection.ReplaceOne(predicate, data, new UpdateOptions() { IsUpsert = IsUpSert });

                if (result.IsModifiedCountAvailable)
                    return new DataResult<bool>(true);

                return new DataResult<bool>(false) { Code = ResponseCode.BadRequest, Message = "unsuccessful" };
            }
            catch (Exception ex)
            {
                return new DataResult<bool>(false) { Code = ResponseCode.BadRequest, Message = ex.Message };
            }
        }

        public DataResult<bool> Delete(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var result = database.GetCollection<T>(collectionName).DeleteOne(predicate);
                if (result.DeletedCount > 0)
                {
                    return new DataResult<bool>(true);
                }

                return new DataResult<bool>(false) { Code = ResponseCode.BadRequest, Message = "unsuccessful" };

            }
            catch (Exception e)
            {

                throw;
            }
        }

        public T GetByKey(string id)
        {
            return database.GetCollection<T>(collectionName).AsQueryable().FirstOrDefault(i => i.Id == id);
        }

        public List<T> GetAll()
        {
            return database.GetCollection<T>(collectionName).AsQueryable().ToList();
        }

        public List<T> GetBy(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return database.GetCollection<T>(collectionName).Find(predicate).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<T> GetBy(Expression<Func<T, bool>> predicate, int limit)
        {
            try
            {
                return database.GetCollection<T>(collectionName).Find(predicate).Limit(limit).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public List<T> GetRandom(int limit)
        {
            try
            {
                var count = database.GetCollection<T>(collectionName).EstimatedDocumentCount();
                var random = new Random().Next(0, ((int)count - limit));
                return database.GetCollection<T>(collectionName).AsQueryable().Skip(random).Take(limit).ToList();
            }
            catch (Exception e)
            {

                return null;
            }

        }

        public List<T> GetByPage(Expression<Func<T, bool>> predicate, int pageNumber, int pageCount, string orderBy = "Id", bool isDesc = false)
        {
            try
            {
                return database.GetCollection<T>(collectionName).AsQueryable().Where(predicate).OrderByDescending(a => a.Id).Skip(((pageNumber - 1) * pageCount)).Take(pageCount).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public int GetCount()
        {
            return (int)database.GetCollection<T>(collectionName).EstimatedDocumentCount();
        }

        public int GetCount(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return (int)database.GetCollection<T>(collectionName).AsQueryable().Where(predicate).Count();
            }
            catch (Exception e)
            {
                return -1;
            }

        }

        
    }
}
