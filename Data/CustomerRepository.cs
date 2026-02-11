using Dapper;
using SupplyChain.Models;
using SupplyChain.Utils;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace SupplyChain.Data
{
    public class CustomerRepository
    {
        private readonly string connStr =
           ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;

        public Customers GetByEmail(string email)
        {
            using (var db= new SqlConnection(connStr))
            {
                return db.QueryFirstOrDefault<Customers>(
                    "sp10_GetCustomerByEmail",
                    new { Email = email },
                    commandType: System.Data.CommandType.StoredProcedure
                );
            }
        }
        public int InsertCustomer(string fullName, string email, string phone, string password)
        {
            string hash = PasswordHelper.HashPassword(password);

            using (var db = new SqlConnection(connStr))
            {
                return db.ExecuteScalar<int>(
                    "sp11_InsertCustomer",
                    new
                    {
                        FullName = fullName,
                        Email = email,
                        Phone = phone,
                        PasswordHash = hash
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

    }
}