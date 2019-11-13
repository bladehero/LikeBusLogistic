﻿using AutoMapper;
using LikeBusLogistic.DAL;
using LikeBusLogistic.VM.MapperExtensions;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LikeBusLogistic.BLL.Services
{
    public abstract class BaseService : IDisposable
    {
        private string _connectionString;

        public const string GeneralSuccessMessage = "Успешно выполнено!";
        public const string GeneralErrorMessage = "Успешно выполнено!";

        protected IMapper Mapper => new ServiceMapperExtension().Mapper;
        protected UnitOfWork UnitOfWork { get; set; }
        protected IDbConnection Connection => new SqlConnection(_connectionString);

        protected int? AccountId { get; set; }

        public BaseService(string connection)
        {
            _connectionString = connection;
            UnitOfWork = new UnitOfWork(Connection);
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
