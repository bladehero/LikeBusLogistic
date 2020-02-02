using AutoDataLayerGenerator.Data;
using System;
using System.Data.SqlClient;
using System.IO;

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
            };

            var connection = new SqlConnection("Data Source=localhost;Initial Catalog=LikeBusLogisticDatabase;Integrated Security=True;");
            var generator = new Generator(connection);
            generator.GenerateStructure(modelData);

            Console.ReadKey();
        }
    }
}
