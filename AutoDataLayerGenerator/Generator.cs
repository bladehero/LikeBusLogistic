using AutoDataLayerGenerator.Data;
using Dapper;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoDataLayerGenerator
{
    public class Generator
    {
        public const string Pattern = "{{(.*?)}}";
        public static Regex Regex => new Regex(Pattern);

        public IDbConnection Connection { get; }

        public async Task GenerateStructure(BaseData data)
        {
            var properties = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var script = await File.ReadAllTextAsync(data.ScriptPath);
            foreach (var property in properties)
            {
                var name = $"{{{{{property.Name}}}}}";
                var flag = false;
                foreach (var match in Regex.Matches(script))
                {
                    var key = match.ToString().ToUpper();
                    if (flag = name.ToUpper() == key)
                    {
                        script = script.Replace(name, property.GetValue(data).ToString());
                    }
                }
                if (!flag)
                {
                    script = script.Replace(name, string.Empty);
                }
            }

            var models = Connection.Query<DataLayerModel>(script);
            Parallel.ForEach(models,
            async model =>
            {
                var path = Path.Combine(data.FolderPath, model.File);
                using (var sw = new StreamWriter(path))
                {
                    await sw.WriteAsync(model.Content);
                }
            });
        }

        public Generator(IDbConnection connection) => Connection = connection;
    }
}
