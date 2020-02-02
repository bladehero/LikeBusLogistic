namespace AutoDataLayerGenerator.Data
{
    public class ModelData : BaseData
    {
        public ModelData(string scriptPath, string folderPath) : base(scriptPath, folderPath)
        {
        }

        public string Namespace { get; set; }
        public string Using { get; set; }
        public string BaseClass { get; set; }
        public string IgnorableColumns { get; set; }
    }
}
