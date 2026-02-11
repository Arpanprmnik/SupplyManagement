using Dapper;
using SupplyChain.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SupplyChain.Data
{
    public class ProductRepository
    {
        private readonly string connStr =
            ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;

        public IEnumerable<Products> GetAllProducts()
        {
            using (var db = new SqlConnection(connStr))
            {
                var result = db.Query<Products, Categories, Suppliers, Products>(
                    "sp1_GetAllProducts",
                    (product, category, supplier) =>
                    {
                        product.Categories = category;
                        product.Suppliers = supplier;
                        return product;
                    },
                    commandType: CommandType.StoredProcedure,
                    splitOn: "CategoryId,SupplierId"
                );

                return result.ToList();
            }
        }
        public Products GetProductsbyId(int id)
        {
            using (var db = new SqlConnection(connStr))
            {
                return db.Query<Products, Categories, Suppliers, Products>(
                    "sp2_GetProductById",
                    (product, category, supplier) =>
                    {
                        product.Categories = category;
                        product.Suppliers = supplier;
                        return product;
                    },
                    new { ProductId = id },  
                    commandType: CommandType.StoredProcedure,
                    splitOn: "CategoryId,SupplierId"
                ).FirstOrDefault();
            }
        }

    }
}
