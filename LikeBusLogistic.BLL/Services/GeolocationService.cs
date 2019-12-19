using LikeBusLogistic.BLL.Results;
using LikeBusLogistic.DAL.Models;
using LikeBusLogistic.VM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LikeBusLogistic.BLL.Services
{
    public class GeolocationService : BaseService
    {
        public GeolocationService(string connection) : base(connection) { }

        public BaseResult<CountryVM> GetCountry(int? countryId)
        {
            var result = new BaseResult<CountryVM>();
            try
            {
                var country = UnitOfWork.CountryDao.FindById(countryId, RoleName == Variables.RoleName.Administrator);
                result.Data = Mapper.Map<CountryVM>(country);
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
        public BaseResult<DistrictVM> GetDistrict(int? districtId)
        {
            var result = new BaseResult<DistrictVM>();
            try
            {
                var district = UnitOfWork.StoredProcedureDao.GetDistrict(districtId.Value).FirstOrDefault();
                result.Data = Mapper.Map<DistrictVM>(district);
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
        public BaseResult<CityVM> GetCity(int? cityId)
        {
            var result = new BaseResult<CityVM>();
            try
            {
                var city = UnitOfWork.StoredProcedureDao.GetCity(cityId.Value).FirstOrDefault();
                result.Data = Mapper.Map<CityVM>(city);
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
        public BaseResult<LocationVM> GetLocation(int? locationId)
        {
            var result = new BaseResult<LocationVM>();
            try
            {
                var location = UnitOfWork.StoredProcedureDao.GetLocation(locationId.Value).FirstOrDefault();
                result.Data = Mapper.Map<LocationVM>(location);
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

        public BaseResult<CountryVM> GetCountryByCity(int? cityId)
        {
            var result = new BaseResult<CountryVM>();
            try
            {
                var city = UnitOfWork.CityDao.FindById(cityId, RoleName == Variables.RoleName.Administrator);
                var district = UnitOfWork.DistrictDao.FindById(city.DistrictId, RoleName == Variables.RoleName.Administrator);
                var country = UnitOfWork.CountryDao.FindById(district.CountryId, RoleName == Variables.RoleName.Administrator);
                result.Data = Mapper.Map<CountryVM>(country);
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
        public BaseResult<CountryVM> GetCountryByDistrict(int? districtId)
        {
            var result = new BaseResult<CountryVM>();
            try
            {
                var district = UnitOfWork.DistrictDao.FindById(districtId, RoleName == Variables.RoleName.Administrator);
                var country = UnitOfWork.CountryDao.FindById(district.CountryId, RoleName == Variables.RoleName.Administrator);
                result.Data = Mapper.Map<CountryVM>(country);
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
        public BaseResult<DistrictVM> GetDistrictByCity(int? cityId)
        {
            var result = new BaseResult<DistrictVM>();
            try
            {
                var city = UnitOfWork.CityDao.FindById(cityId, RoleName == Variables.RoleName.Administrator);
                var district = UnitOfWork.StoredProcedureDao
                               .GetDistrict(city.DistrictId, RoleName == Variables.RoleName.Administrator)
                               .FirstOrDefault();
                result.Data = Mapper.Map<DistrictVM>(district);
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
        public BaseResult<IEnumerable<DistrictVM>> GetDistrictsByCountry(int? countryId)
        {
            var result = new BaseResult<IEnumerable<DistrictVM>>();
            try
            {
                var district = UnitOfWork.StoredProcedureDao
                               .GetDistrict(withDeleted: RoleName == Variables.RoleName.Administrator)
                               .Where(x => x.CountryId == countryId);
                result.Data = Mapper.Map<IEnumerable<DistrictVM>>(district);
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
        public BaseResult<IEnumerable<CityVM>> GetCitiesByCountry(int? countryId)
        {
            var result = new BaseResult<IEnumerable<CityVM>>();
            try
            {
                var cities = UnitOfWork.StoredProcedureDao
                             .GetCity(withDeleted: RoleName == Variables.RoleName.Administrator)
                             .Where(x => x.CountryId == countryId);
                result.Data = Mapper.Map<IEnumerable<CityVM>>(cities);
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
        public BaseResult<IEnumerable<CityVM>> GetCitiesByDistrict(int? districtId)
        {
            var result = new BaseResult<IEnumerable<CityVM>>();
            try
            {
                var cities = UnitOfWork.StoredProcedureDao
                             .GetCity(withDeleted: RoleName == Variables.RoleName.Administrator)
                             .Where(x => x.DistrictId == districtId);
                result.Data = Mapper.Map<IEnumerable<CityVM>>(cities);
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

        public BaseResult<IEnumerable<CountryVM>> GetCountries()
        {
            var result = new BaseResult<IEnumerable<CountryVM>>();
            try
            {
                var country = UnitOfWork.CountryDao.FindAll(RoleName == Variables.RoleName.Administrator);
                result.Data = Mapper.Map<IEnumerable<CountryVM>>(country);
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
        public BaseResult<IEnumerable<DistrictVM>> GetDistricts()
        {
            var result = new BaseResult<IEnumerable<DistrictVM>>();
            try
            {
                var district = UnitOfWork.StoredProcedureDao.GetDistrict(withDeleted: RoleName == Variables.RoleName.Administrator);
                result.Data = Mapper.Map<IEnumerable<DistrictVM>>(district);
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
        public BaseResult<IEnumerable<CityVM>> GetCities()
        {
            var result = new BaseResult<IEnumerable<CityVM>>();
            try
            {
                var city = UnitOfWork.StoredProcedureDao.GetCity(withDeleted: RoleName == Variables.RoleName.Administrator);
                result.Data = Mapper.Map<IEnumerable<CityVM>>(city);
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
        public BaseResult<IEnumerable<LocationVM>> GetLocations(bool withDeleted = true)
        {
            var result = new BaseResult<IEnumerable<LocationVM>>();
            try
            {
                var location = UnitOfWork.StoredProcedureDao.GetLocation(withDeleted: RoleName == Variables.RoleName.Administrator && withDeleted);
                result.Data = Mapper.Map<IEnumerable<LocationVM>>(location);
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

        public BaseResult MergeCountry(CountryVM countryVM)
        {
            var result = new BaseResult();
            try
            {
                var country = Mapper.Map<Country>(countryVM);
                if (countryVM.Id == 0)
                {
                    country.CreatedBy = AccountId;
                }
                country.ModifiedBy = AccountId;
                result.Success = UnitOfWork.CountryDao.Merge(country);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult MergeDistrict(DistrictVM districtVM)
        {
            var result = new BaseResult();
            try
            {
                var district = Mapper.Map<District>(districtVM);
                if (districtVM.Id == 0)
                {
                    district.CreatedBy = AccountId;
                }
                district.ModifiedBy = AccountId;
                result.Success = UnitOfWork.DistrictDao.Merge(district);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult MergeCity(CityVM cityVM)
        {
            var result = new BaseResult();
            try
            {
                var city = Mapper.Map<City>(cityVM);
                if (cityVM.Id == 0)
                {
                    city.CreatedBy = AccountId;
                }
                city.ModifiedBy = AccountId;
                result.Success = UnitOfWork.CityDao.Merge(city);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult MergeLocation(LocationVM locationVM)
        {
            var result = new BaseResult();
            try
            {
                var location = Mapper.Map<Location>(locationVM);
                if (locationVM.Id == 0)
                {
                    location.CreatedBy = AccountId;
                }
                location.ModifiedBy = AccountId;
                result.Success = UnitOfWork.LocationDao.Merge(location);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }

        public BaseResult DeleteOrRestoreCountry(int countryId)
        {
            var result = new BaseResult();
            try
            {
                result.Success = UnitOfWork.CountryDao.DeleteOrRestore(countryId);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult DeleteOrRestoreDistrict(int disctrictId)
        {
            var result = new BaseResult();
            try
            {
                result.Success = UnitOfWork.DistrictDao.DeleteOrRestore(disctrictId);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult DeleteOrRestoreCity(int cityId)
        {
            var result = new BaseResult();
            try
            {
                result.Success = UnitOfWork.CityDao.DeleteOrRestore(cityId);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult DeleteOrRestoreLocation(int locationId)
        {
            var result = new BaseResult();
            try
            {
                result.Success = UnitOfWork.LocationDao.DeleteOrRestore(locationId);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
    }
}
