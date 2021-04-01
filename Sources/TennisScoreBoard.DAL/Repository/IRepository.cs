using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TennisScoreBoard.EF.Repository
{
    public interface IRepository <T>
    {
        bool Add(T entity);
        bool Update(T entity);
        T Get(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void SaveChanges();
    }
}
