using Data.Concrete;
using Model;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace Data.Repos
{
    public class UserDataRepo : EntityBaseMongoData<User>
    {
        public UserDataRepo() : base(MongoTableName.User)
        {
        }
    }
}
