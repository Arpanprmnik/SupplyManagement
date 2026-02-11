
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using SupplyChain.Models;
using Dapper;


namespace SupplyChain.Data
{
    public class ManagerRepository
    {
        private readonly string connStr =
        ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;

        public Manager GetByEmail(string email)
        {
            using (var db = new SqlConnection(connStr))
            {
                return db.QueryFirstOrDefault<Manager>(
                    "sp_GetManagerByEmail",
                    new { Email = email },
                    commandType: System.Data.CommandType.StoredProcedure
                    );
            }
        }
        public IEnumerable<ManagerInventoryViewModel> GetInventory()
        {
            using (var db = new SqlConnection(connStr))
            {
                return db.Query<ManagerInventoryViewModel>("sp_GetInventoryDetails",
                commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}