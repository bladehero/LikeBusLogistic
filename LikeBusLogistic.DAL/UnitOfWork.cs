using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LikeBusLogistic.DAL
{
    public sealed class UnitOfWork : IDisposable
    {
        private IDbConnection _connection;

        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection;


        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
