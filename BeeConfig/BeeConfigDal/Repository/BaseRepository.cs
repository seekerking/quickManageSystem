using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BeeConfigDal.Repository
{

        public class BaseRepository<T> : IDisposable where T : class 
    {
            protected readonly BaseContext Context = new BaseContext();

            /// <summary>
            /// 根据过滤条件，获取记录
            /// </summary>
            /// <param name="exp">The exp.</param>
            public IQueryable<T> Find(Expression<Func<T, bool>> exp = null)
            {
                return Filter(exp);
            }

            public bool IsExist(Expression<Func<T, bool>> exp)
            {
                return Context.Set<T>().Any(exp);
            }

            /// <summary>
            /// 查找单个，且不被上下文所跟踪
            /// </summary>
            public T FindSingle(Expression<Func<T, bool>> exp)
            {
                return Context.Set<T>().AsNoTracking().FirstOrDefault(exp);
            }

            /// <summary>
            /// 得到分页记录
            /// </summary>
            /// <param name="pageindex">The pageindex.</param>
            /// <param name="pagesize">The pagesize.</param>
            /// <param name="orderby">排序，格式如："Id"/"Id descending"</param>
            public IQueryable<T> Find(int pageindex, int pagesize, Func<T, string> orderby, Expression<Func<T, bool>> exp = null)
            {
                if (pageindex < 1) pageindex = 1;
                return Filter(exp).OrderBy(orderby).Skip(pagesize * (pageindex - 1)).Take(pagesize).AsQueryable();
            }

            /// <summary>
            /// 根据过滤条件获取记录数
            /// </summary>
            public int GetCount(Expression<Func<T, bool>> exp = null)
            {
                return Filter(exp).Count();
            }

            public void Add(T entity)
            {
                Context.Set<T>().Add(entity);
                Save();
            }

            /// <summary>
            /// 批量添加
            /// </summary>
            /// <param name="entities">The entities.</param>
            public void BatchAdd(T[] entities)
            {
                Context.Set<T>().AddRange(entities);
                Save();
            }

            public void Update(T entity)
            {
                var entry = this.Context.Entry(entity);
                entry.State = EntityState.Modified;

                //如果数据没有发生变化
                if (!this.Context.ChangeTracker.HasChanges())
                {
                    return;
                }

                Save();
            }

       

            public void Delete(T entity)
            {
                Context.Set<T>().Remove(entity);
                Save();
            }

            public void Save()
            {
                Context.SaveChanges();
            }

            private IQueryable<T> Filter(Expression<Func<T, bool>> exp)
            {
                var dbSet = Context.Set<T>().AsNoTracking().AsQueryable();
                if (exp != null)
                    dbSet = dbSet.Where(exp);
                return dbSet;
            }

            public void Dispose()
            {
                this.Context.Dispose();
            }
        }
    }

