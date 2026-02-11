using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SupplyChain.Models;

namespace SupplyChain.Data
{
    public class OrderRepository
    {
        private readonly string connStr =
            ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
        public int CreateOrder(int customerId)
        {
            using (var db = new SqlConnection(connStr))
            {
                var p = new DynamicParameters();
                p.Add("CustomerId", customerId);
                p.Add("@OrderId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                db.Execute("sp3_CreateOrder",
                    p,
                    commandType: CommandType.StoredProcedure);
                return p.Get<int>("@OrderId");
            }
        }
        public void AddOrderItem(int orderId, int productId,int quantity)
        {
            using(var db = new SqlConnection(connStr))
            {
                db.Execute("sp21_AddOrderItem",
                    new
                    {
                        OrderId = orderId,
                        ProductId = productId,
                        Quantity = quantity
                    },
                    commandType: CommandType.StoredProcedure
                    );

            }
        }
        public IEnumerable<OrderViewModel> GetAllOrders()
        {
            using (var db = new SqlConnection(connStr))
            {
                return db.Query<OrderViewModel>(
                    "sp20_GetAllOrders",
                    commandType: CommandType.StoredProcedure
                    );
            }
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            using (var db = new SqlConnection(connStr))
            {
                db.Execute(
                    "sp_UpdateOrderStatus",
                    new { Status = status, OrderId = orderId },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public void UpdateInventory(int orderId)
        {
            using (var db = new SqlConnection(connStr))
            {
                db.Execute("sp_UpdateInventoryAfterOrder",
                new { OrderId = orderId },
                commandType: CommandType.StoredProcedure
                );
            }
        }

        public void IncreaseInventory(int orderId)
        {
            using (var db = new SqlConnection(connStr))
            {
                db.Execute(@"
            sp_RestoreInventoryAfterOrder
        ",
                new { OrderId = orderId }, commandType: CommandType.StoredProcedure);
            }
        }
        
        public IEnumerable<Orders> GetOrdersByCustomerId(int customerId)
        {
            using (var db = new SqlConnection(connStr))
            {
                return db.Query<Orders>(
                "dbo.sp_GetOrdersByCustomer",
                new { CustomerId = customerId },
                commandType: CommandType.StoredProcedure
                    );

            }
        }


    }
}