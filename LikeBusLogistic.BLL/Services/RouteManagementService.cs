using LikeBusLogistic.BLL.Results;
using LikeBusLogistic.VM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeBusLogistic.BLL.Services
{
    public class RouteManagementService : BaseService
    {
        public RouteManagementService(string connection) : base(connection) { }

        public BaseResult<IEnumerable<RouteVM>> GetRoutes()
        {
            var result = new BaseResult<IEnumerable<RouteVM>>();
            try
            {
                var country = UnitOfWork.RouteDao.FindAll(RoleName == Variables.RoleName.Administrator);
                result.Data = Mapper.Map<IEnumerable<RouteVM>>(country);
                result.Success = true;
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
    }
}
