using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ADO_EXAMPLES.Models;

namespace ADO_EXAMPLES.Data
{
    public class Product_Data
    {
        string conString = ConfigurationManager.ConnectionStrings["adoCoonectionstring"].ToString();

        public List<Product> GetAllProducts()
        {
            List<Product> ProductList = new List<Product>();

            using (SqlConnection connection = new SqlConnection(conString)) 
            { 
                SqlCommand command=connection.CreateCommand(); 
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetAllProducts";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    ProductList.Add(new Product
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Qty = Convert.ToInt32(dr["qty"]),
                        Remarks = dr["Remarks"].ToString()
                    });
                }
            }

            return ProductList;
        }


       // Insert product
       public bool InsertProduct(Product product)
        {
            int id = 0;
            using (SqlConnection connection=new SqlConnection(conString)) 
            {
                SqlCommand command = new SqlCommand("sp_InsertProducts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Qty", product.Qty);
                command.Parameters.AddWithValue("@Remarks", product.Remarks);

                connection.Open();
                id =command.ExecuteNonQuery();
                connection.Close();
            }
            if(id>0)
            { 
                return true;
            }
            else
            {
                return false;
            }
        }


        //Get Product by Product ID

        public List<Product> GetProductByID(int ProductID)
        {
            List<Product> ProductList = new List<Product>();
            using (SqlConnection connection=new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetProductByID";
                command.Parameters.AddWithValue("@ProductID",ProductID);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts); 
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows) 
                {
                    ProductList.Add(new Product
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Qty = Convert.ToInt32(dr["qty"]),
                        Remarks = dr["Remarks"].ToString()

                    });
                }
            }
            return ProductList;
        }


        // update products
        public bool UpdateProduct(Product product)
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateProducts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductID", product.ProductID);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Qty", product.Qty);
                command.Parameters.AddWithValue("@Remarks", product.Remarks);

                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Delete

         public string DeleteProduct(int productid)
        {
            String result = "";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_DeleteProduct", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@productid", productid);
                command.Parameters.Add("@outputMessage", SqlDbType.VarChar,50).Direction= ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();
                result = command.Parameters["@outputMessage"].Value.ToString();
                connection.Close();
            }

                return result;
         }

        internal static object GetProductsByID(int id)
        {
            throw new NotImplementedException();
        }
    }
}