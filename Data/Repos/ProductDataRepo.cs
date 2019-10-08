using Data.Abstract;
using Data.Concrete;
using Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace Data.Repos
{
    public class ProductDataRepo : EntityBaseMongoData<Product>, IProductDataRepo
    {
        public ProductDataRepo() : base(MongoTableName.Product)
        {
        }

        public decimal GetTotalProductPrice()
        {
            return database.GetCollection<Product>(MongoTableName.Product).AsQueryable().Sum(x=>x.Price);
        }
    }
}
