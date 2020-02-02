using AutoDataLayerGenerator.Data;
using Dapper;
using System.Data;
using System.Data.SqlClient;
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

        private string _connectionString;
        public IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task GenerateStructure(BaseData data)
        {
            try
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
                    if (data.ReplaceWhenExists || !File.Exists(path))
                    {
                        using var sw = new StreamWriter(path);
                        await sw.WriteAsync(model.Content);
                    }
                });
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public Generator(string connection) => _connectionString = connection;
    }
}
