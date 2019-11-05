using AutoMapper;

namespace LikeBusLogistic.VM.MapperExtensions
{
    public class WebMapperExtension : BaseMapperExtension
    {
        public WebMapperExtension()
        {
            _config = new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<Source, Destination>();
            });
        }
    }
}
