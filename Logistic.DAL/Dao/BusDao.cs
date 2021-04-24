﻿using Logistic.DAL.Models;
using System.Data;
namespace Logistic.DAL.Dao
{
    public class BusDao : BaseDao<Bus>
    {
        public BusDao(IDbConnection connection) : base("dbo.Bus", connection) { }
    }
}