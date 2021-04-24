using AutoDataLayerGenerator.Data;
using System;
using System.Diagnostics;
using System.IO;

namespace AutoDataLayerGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var folder = "D:/Projects/Logistic";

            var modelData = new ModelData(
                Path.Combine(folder, "AutoDataLayerGenerator/Scripts/Models.sql"),
                Path.Combine(folder, "Logistic.DAL/Models"))
            {
                BaseClass = "BaseEntity",
                IgnorableColumns = "('Id'),('DateCreated'),('DateModified'),('IsDeleted'),('CreatedBy'),('ModifiedBy')",
                Namespace = "Logistic.DAL.Models",
                Using = "using System;"
            };

            var daoData = new DaoData(
                Path.Combine(folder, "AutoDataLayerGenerator/Scripts/DataAccessObjects.sql"),
                Path.Combine(folder, "Logistic.DAL/Dao"))
            {
                BaseClass = "BaseDao",
                Namespace = "Logistic.DAL.Dao",
                Using = "using System.Data;",
                ModelsNamespace = modelData.Namespace
            };

            var generator = new Generator("Data Source=localhost;Initial Catalog=LogisticDatabase;Integrated Security=True;");
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
