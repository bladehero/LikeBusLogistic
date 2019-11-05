using AutoMapper;

namespace LikeBusLogistic.VM.MapperExtensions
{
    public class BLLMapperExtension : BaseMapperExtension
    {
        public BLLMapperExtension()
        {
            _config = new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<Source, Destination>();
            });
        }
    }
}
