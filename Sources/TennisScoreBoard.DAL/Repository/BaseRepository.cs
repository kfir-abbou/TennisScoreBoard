using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using log4net;

namespace TennisScoreBoard.EF.Repository
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ScoreBoardContext m_dbContext;

        protected BaseRepository(ScoreBoardContext context)
        {
            m_dbContext = context;
        }

        public virtual bool Add(T entity)
        {
            var type = entity.GetType();
            try
            {
                s_log.DebugFormat($"[Add::{type}]");
                m_dbContext.Add(entity);
                return true;
            }
            catch (Exception e)
            {
                s_log.ErrorFormat($"[Add::{type}] Error: {e}");
                return false;
            }

        }

        public virtual bool Update(T entity)
        {
            var type = entity.GetType();
            try
            {
                s_log.ErrorFormat($"[Update::{type}]");
                m_dbContext.Update(entity);
                return true;
            }
            catch (Exception e)
            {
                s_log.ErrorFormat($"[Update:{type}] Error: {e}");
                return false;
            }
        }

        public virtual T Get(int id)
        {
            s_log.ErrorFormat($"[Get] id: {id}");
            return m_dbContext.Find<T>(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            s_log.ErrorFormat($"[GetAll]");
            return m_dbContext.Set<T>().ToList();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            s_log.Debug($"[Find]");
            
            return m_dbContext.Set<T>().Where(predicate).ToList();
        }

        public virtual void SaveChanges()
        {
            s_log.ErrorFormat($"[SaveChanges]");
            m_dbContext.SaveData();
        }
    }
}
