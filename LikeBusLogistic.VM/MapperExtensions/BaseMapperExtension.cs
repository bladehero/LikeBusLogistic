using AutoMapper;

namespace LikeBusLogistic.VM.MapperExtensions
{
    public class BaseMapperExtension
    {
        protected MapperConfiguration _config;
        private IMapper _mapper;
        public IMapper Mapper => _mapper ?? (_mapper = Initialize());

        private IMapper Initialize()
        {
            return _config.CreateMapper();
        }
    }
}
