using Dapper;
using LikeBusLogistic.DAL.Attributes;
using LikeBusLogistic.DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace LikeBusLogistic.DAL.Dao
{
    public abstract class BaseDao<T> : DataAccessObject, IEnumerable<T>, IEnumerable where T : BaseEntity
    {
        protected string TableName { get; set; }

        public BaseDao(string table, IDbConnection connection) : base(connection)
        {
            TableName = table;
        }

        protected string SelectFromString => $"select * from {TableName}";

        public IEnumerator GetEnumerator() => FindAll().GetEnumerator();
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => FindAll().GetEnumerator();

        public int Count(bool withDeleted = false) => Connection.QueryFirstOrDefault<int>($"select count(Id) from {TableName}{(withDeleted ? string.Empty : " where IsDeleted = 0")}");

        public T FindById(int? id, bool withDeleted = false) => id.HasValue ? Connection.QueryFirstOrDefault<T>($"select top 1 * from {TableName} where Id = {id.Value}{(withDeleted ? string.Empty : " and IsDeleted = 0")}") : default(T);
        public virtual IEnumerable<T> FindAll(bool withDeleted = false) => Connection.Query<T>($"{SelectFromString}{(withDeleted ? string.Empty : " where IsDeleted = 0")} order by Id desc");
        public virtual IEnumerable<T> Find(Func<T, bool> predicate, bool withDeleted = false) => Connection.Query<T>($"{SelectFromString}{(withDeleted ? string.Empty : " where IsDeleted = 0")}").Where(predicate);
        public virtual T FirstOrDefault(Func<T, bool> predicate, bool withDeleted = false) => Connection.Query<T>($"{SelectFromString}{(withDeleted ? string.Empty : " where IsDeleted = 0")}").FirstOrDefault(predicate);
        public virtual IEnumerable<T> Take(int count, int skip = 0, bool withDeleted = false)
        {
            return Connection.Query<T>($"{SelectFromString}{(withDeleted ? string.Empty : " where IsDeleted = 0")} order by Id desc offset ({skip}) rows fetch next ({count}) rows only");
        }

        public virtual IEnumerable<T> Random(bool withDeleted = false)
        {
            return Connection.Query<T>($"select * from {TableName}{(withDeleted ? string.Empty : " where IsDeleted = 0")} order by newid()");
        }
        public virtual IEnumerable<T> Random(int count, bool withDeleted = false)
        {
            return Connection.Query<T>($"select top {count} * from {TableName}{(withDeleted ? string.Empty : " where IsDeleted = 0")} order by newid()");
        }
        public virtual T FirstOrDefaultRandom(bool withDeleted = false)
        {
            return Connection.QueryFirstOrDefault<T>($"select top 1 * from {TableName}{(withDeleted ? string.Empty : " where IsDeleted = 0")} order by Id newid()");
        }

        public virtual int Insert(T item)
        {
            var properties = _getProperties(item, DatabaseActions.Insert);
            var sql = $"insert into {TableName} ({string.Join(",", properties.Select(x => x.Name))}) values ({string.Join(",", properties.Select(x => "@" + x.Name))}) SELECT CAST(SCOPE_IDENTITY() as int)";
            return item.Id = Connection.Query<int>(sql, item).Single();
        }
        public virtual bool Update(T item)
        {
            var properties = _getProperties(item, DatabaseActions.Update);
            var sql = $"update {TableName} set {string.Join(",", properties.Select(x => x.Name + " = @" + x.Name))} where Id = @Id";
            return Connection.Execute(sql, item) > 0;
        }
        public virtual bool UpdateDateModified(int id, DateTime? dateTime = null)
        {
            var sql = $"update {TableName} set DateModified = '{dateTime.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-ddTHH:mm:ss")}' where Id = {id}";
            return Connection.Execute(sql) > 0;
        }
        public virtual bool HardDelete(int id) => Connection.Execute($"delete {TableName} where Id = {id}") > 0;
        public virtual bool Delete(int id) => Connection.Execute($"update {TableName} set IsDeleted = 1, DateModified = getdate() where Id = {id}") > 0;
        public virtual bool Delete(T item)
        {
            item.IsDeleted = true;
            return Delete(item.Id);
        }
        public virtual bool Restore(int id) => Connection.Execute($"update {TableName} set IsDeleted = 0, DateModified = getdate() where Id = {id}") > 0;
        public virtual bool Restore(T item)
        {
            item.IsDeleted = false;
            return Restore(item.Id);
        }
        public virtual bool DeleteOrRestore(int id) => FindById(id, true) is T item ? item.IsDeleted ? Restore(item) : Delete(item) : false;
        public virtual bool DeleteOrRestore(T item) => item.IsDeleted ? Restore(item) : Delete(item);
        public virtual bool Merge(T item) => item?.Id == 0 ? Insert(item) > 0 : Update(item);

        protected IEnumerable<T> Query(string sql) => Connection.Query<T>(sql);
        protected T QueryFirstOrDefault(string sql) => Connection.QueryFirstOrDefault<T>(sql);

        private IEnumerable<PropertyInfo> _getProperties(T item, DatabaseActions action)
        {
            var _props = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var properties = new Stack<PropertyInfo>(_props.Length);
            foreach (var property in _props)
            {
                var attributes = property.GetCustomAttributes(true);

                switch (action)
                {
                    case DatabaseActions.Insert:

                        if (!attributes.Any(x => x is IgnoreAttribute attribute && attribute.WhenInsert))
                        {
                            properties.Push(property);
                        }

                        break;
                    case DatabaseActions.Update:

                        if (!attributes.Any(x => x is IgnoreAttribute attribute && attribute.WhenUpdate))
                        {
                            properties.Push(property);
                        }

                        break;
                    case DatabaseActions.Delete:
                        properties.Push(property);
                        break;
                    case DatabaseActions.Select:
                    default:
                        properties.Push(property);
                        break;
                }
            }
            return properties;
        }
    }
}
