using Data.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Data.Abstract
{
    public interface IData<T>
    {
        DataResult<T> Insert(T t);
        DataResult<bool> Update(T data, Expression<Func<T, bool>> predicate, bool IsUpSert = false);
        DataResult<bool> Delete(Expression<Func<T, bool>> predicate);
        T GetByKey(string id);

        List<T> GetAll();
        List<T> GetBy(Expression<Func<T, bool>> predicate);
        List<T> GetBy(Expression<Func<T, bool>> predicate, int limit);
        List<T> GetRandom(int limit);
        List<T> GetByPage(Expression<Func<T, bool>> predicate, int pageNumber, int pageCount, string orderBy = "Id", bool isDesc = false);

        int GetCount();
        int GetCount(Expression<Func<T, bool>> predicate);

    }
}
