using AutoDataLayerGenerator.Data;

namespace AutoDataLayerGenerator
{
    public class DaoData : BaseData
    {
        public DaoData(string scriptPath, string folderPath) : base(scriptPath, folderPath)
        {
        }

        public string Namespace { get; set; }
        public string ModelsNamespace { get; set; }
        public string Using { get; set; }
        public string BaseClass { get; set; }
    }
}
