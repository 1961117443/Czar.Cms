using Czar.Cms.Core.Options;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Czar.Cms.Core.Repository
{
    public class BaseRepository<T, TKey> : IBaseRepository<T, TKey> where T : class
    {
        protected DbOpion _dbOpion;
        protected IDbConnection _dbConnection;

        #region 同步方法
        public T Get(TKey id)
        {
            return _dbConnection.Get<T>(id);
        }
        public IEnumerable<T> GetList()
        {
            return _dbConnection.GetList<T>();
        }

        public IEnumerable<T> GetList(object whereCondition)
        {
            return _dbConnection.GetList<T>(whereCondition);
        }

        public IEnumerable<T> GetList(string condition, object parameters = null)
        {
            return _dbConnection.GetList<T>(condition,parameters);
        }
        public IEnumerable<T> GetListPaged(int pageNumber, int rowsPerpage, string conditions, string orderby, object parameters = null)
        {
            return _dbConnection.GetListPaged<T>(pageNumber,rowsPerpage, conditions,orderby, parameters);
        } 

        public int? Insert(T entity)
        {
            return _dbConnection.Insert<T>(entity);
        }
        public int RecordCount(string conditions = "", object parameters = null)
        {
            return _dbConnection.RecordCount<T>(conditions, parameters);
        } 

        public int Update(T entity)
        {
            return _dbConnection.Update<T>(entity);
        }

        public int Delete(TKey id)
        {
            return _dbConnection.Delete<T>(id);
        }

        public int Delete(T entity)
        {
            return _dbConnection.Delete(entity);
        }

        public int DeleteList(object whereCondition, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _dbConnection.DeleteList<T>(whereCondition, transaction, commandTimeout);
        }

        public int DeleteList(string condition, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _dbConnection.DeleteList<T>(condition, parameters, transaction, commandTimeout);
        }
        #endregion

        #region 异步方法
        public Task<T> GetAsync(TKey id)
        {
            return _dbConnection.GetAsync<T>(id);
        }



        public Task<IEnumerable<T>> GetListAsync()
        {
            return _dbConnection.GetListAsync<T>();
        }

        public Task<IEnumerable<T>> GetListAsync(object whereCondition)
        {
            return _dbConnection.GetListAsync<T>(whereCondition);
        }

        public Task<IEnumerable<T>> GetListAsync(string condition, object parameters = null)
        {
            return _dbConnection.GetListAsync<T>(condition,parameters);
        }
        public Task<IEnumerable<T>> GetListPagedAsync(int pageNumber, int rowsPerpage, string conditions, string orderby, object parameters = null)
        {
            return _dbConnection.GetListPagedAsync<T>(pageNumber, rowsPerpage,conditions,orderby, parameters);
        }

        public Task<int?> InsertAsync(T entity)
        {
            return _dbConnection.InsertAsync(entity);
        }
        public Task<int> RecordCountAsync(string conditions = "", object parameters = null)
        {
            return _dbConnection.RecordCountAsync<T>(conditions, parameters);
        }
        public Task<int> UpdateAsync(T entity)
        {
            return _dbConnection.UpdateAsync(entity);
        }
        public Task<int> DeleteAsync(TKey id)
        {
            return _dbConnection.DeleteAsync(id);
        }

        public Task<int> DeleteAsync(T entity)
        {
            return _dbConnection.DeleteAsync(entity);
        }
        public Task<int> DeleteListAsync(object whereCondition, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _dbConnection.DeleteListAsync<T>(whereCondition,transaction,commandTimeout);
        }

        public Task<int> DeleteListAsync(string condition, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _dbConnection.DeleteListAsync<T>(condition,parameters, transaction, commandTimeout);
        }


        #endregion


        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~BaseRepository() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion




    }
}
