using Dapper;
using SupplyChain.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SupplyChain.Data
{
    public class SellerRepository
    {
        private readonly string connStr =
            ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;


        public Suppliers GetByEmail(string email)
        {
            using (var db = new SqlConnection(connStr))
            {
                return db.QueryFirstOrDefault<Suppliers>(
                    "sp_GetSupplierByEmail",
                    new { Email = email },
                    commandType:CommandType.StoredProcedure);
            }
        }

        public int AddSupplier(Suppliers supplier)
        {
            using (var db = new SqlConnection(connStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyName", supplier.CompanyName);
                parameters.Add("@ContactName", supplier.ContactName);
                parameters.Add("@Phone", supplier.Phone);
                parameters.Add("@Email", supplier.Email);
                parameters.Add("@PasswordHash", supplier.PasswordHash);
                parameters.Add("@NewSupplierId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                db.Execute(
                    "sp_InsertSupplier",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return parameters.Get<int>("@NewSupplierId");
            }
        }
        public IEnumerable<Products> GetSupplierProducts(int supplierId)
        {
            using (var db = new SqlConnection(connStr))
            {
                return db.Query<Products>(@"
                    sp_GetProductsBySupplier
                ", new { SupplierId = supplierId }, commandType: CommandType.StoredProcedure);
            }
        }

        public Products GetSupplierProductById(int productId, int supplierId)
        {
            using (var db = new SqlConnection(connStr))
            {
                return db.QueryFirstOrDefault<Products>(@"
                  sp_GetProductByIdAndSupplier
                ", new { ProductId = productId, SupplierId = supplierId }, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateProduct(Products model, int supplierId)
        {
            using (var db = new SqlConnection(connStr))
            {
                db.Execute(@"
                    sp_UpdateProductBySupplier
                ", new
                {
                    model.Id,
                    model.Name,
                    model.Price,
                    model.Description,
                    SupplierId = supplierId
                }, commandType: CommandType.StoredProcedure
                );
            }
        }


        public IEnumerable<Categories> GetCategories()
        {
            using (var db = new SqlConnection(connStr))
            {
                return db.Query<Categories>("sp_GetAllCategories",commandType: CommandType.StoredProcedure
                    );
            }
        }

        public IEnumerable<Warehouses> GetWarehouses()
        {
            using (var db = new SqlConnection(connStr))
            {
                return db.Query<Warehouses>("sp_GetAllWarehouses",commandType:CommandType.StoredProcedure);
            }
        }

        public void AddProduct(SellerProductViewModel model, int supplierId)
        {
            using (var db = new SqlConnection(connStr))
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // 1️⃣ Prepare parameters with OUTPUT
                        var parameters = new DynamicParameters();
                        parameters.Add("@Name", model.Name);
                        parameters.Add("@SKU", model.SKU);
                        parameters.Add("@Price", model.Price);
                        parameters.Add("@Description", model.Description);
                        parameters.Add("@CategoryId", model.CategoryId);
                        parameters.Add("@SupplierId", supplierId);
                        parameters.Add("@NewProductId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                        // 2️⃣ Execute SP
                        db.Execute(
                            "dbo.sp_InsertProduct",
                            parameters,
                            transaction,
                            commandType: CommandType.StoredProcedure
                        );

                        // 3️⃣ Read OUTPUT value
                        int productId = parameters.Get<int>("@NewProductId");

                        // 4️⃣ Insert Inventory
                        db.Execute(
                            "dbo.sp_InsertInventory",
                            new
                            {
                                ProductId = productId,
                                model.WarehouseId,
                                model.Quantity
                            },
                            transaction,
                            commandType: CommandType.StoredProcedure
                        );

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateInventoryQuantity(int inventoryId, int quantity, int supplierId)
        {
            using (var db = new SqlConnection(connStr))
            {
                db.Execute(@"
            sp_UpdateInventoryQuantityBySupplier
        ", new
                {
                    InventoryId = inventoryId,
                    Quantity = quantity,
                    SupplierId = supplierId
                },
                commandType:CommandType.StoredProcedure);
            }
        }
        public IEnumerable<SellerInventoryViewModel> GetSellerInventory(int supplierId)
        {
            using (var db = new SqlConnection(connStr))
            {
                return db.Query<SellerInventoryViewModel>(@"
            sp_GetInventoryBySupplier
        ", new { SupplierId = supplierId },
        commandType:CommandType.StoredProcedure);
            }
        }
        public IEnumerable<SellerDashboardViewModel> GetSellerDashboard(int supplierId)
        {
            using (var db = new SqlConnection(connStr))
            {
                return db.Query<SellerDashboardViewModel>(
                    "dbo.sp_GetSupplierProductsWithInventory",
                    new { SupplierId = supplierId },
                    commandType: CommandType.StoredProcedure
                );

            }
        }



    }
}
