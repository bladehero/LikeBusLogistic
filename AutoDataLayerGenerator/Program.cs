using AutoDataLayerGenerator.Data;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AutoDataLayerGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var folder = "D:/Projects/LikeBusLogistic";

            var modelData = new ModelData(
                Path.Combine(folder, "AutoDataLayerGenerator/Scripts/Models.sql"),
                Path.Combine(folder, "LikeBusLogistic.DAL/Models"))
            {
                BaseClass = "BaseEntity",
                IgnorableColumns = "('Id'),('DateCreated'),('DateModified'),('IsDeleted'),('CreatedBy'),('ModifiedBy')",
                Namespace = "LikeBusLogistic.DAL.Models",
                Using = "using System;"
            };

            var daoData = new DaoData(
                Path.Combine(folder, "AutoDataLayerGenerator/Scripts/DataAccessObjects.sql"),
                Path.Combine(folder, "LikeBusLogistic.DAL/Dao"))
            {
                BaseClass = "BaseDao",
                Namespace = "LikeBusLogistic.DAL.Dao",
                Using = "using System.Data;",
                ModelsNamespace = modelData.Namespace
            };

            var generator = new Generator("Data Source=localhost;Initial Catalog=LikeBusLogisticDatabase;Integrated Security=True;");
            var watch = new Stopwatch();

            Console.WriteLine(modelData);
            watch.Start();
            generator.GenerateStructure(modelData);
            watch.Stop();
            Console.WriteLine($"Finished with the time: {watch.ElapsedMilliseconds}ms");

            watch.Reset();

            Console.WriteLine(daoData);
            watch.Start();
            generator.GenerateStructure(daoData);
            watch.Stop();
            Console.WriteLine($"Finished with the time: {watch.ElapsedMilliseconds}ms");

            Console.ReadKey();
        }
    }
}
