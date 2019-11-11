using AutoMapper;
using LikeBusLogistic.DAL;
using LikeBusLogistic.VM.MapperExtensions;
using LikeBusLogistic.VM.ViewModels;
using System;
using System.Data;

namespace LikeBusLogistic.BLL.Services
{
    public abstract class BaseService : IDisposable
    {
        public const string GeneralSuccessMessage = "Успешно выполнено!";
        public const string GeneralErrorMessage = "Успешно выполнено!";

        protected IMapper Mapper => new ServiceMapperExtension().Mapper;
        protected UnitOfWork UnitOfWork { get; set; }

        protected int? AccountId { get; set; }

        public BaseService(IDbConnection connection)
        {
            UnitOfWork = new UnitOfWork(connection);
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
