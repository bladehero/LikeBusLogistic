namespace AutoDataLayerGenerator.Data
{
    public abstract class BaseData
    {
        public string ScriptPath { get; }
        public string FolderPath { get; }
        public abstract bool ReplaceWhenExists { get; }

        public BaseData(string scriptPath, string folderPath) =>
            (ScriptPath, FolderPath) = (scriptPath, folderPath);

        public override string ToString()
        {
            return $"{GetType()}\nScript: {ScriptPath}\nFolder: {FolderPath}";
        }
    }
}
