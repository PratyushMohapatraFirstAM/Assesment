
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Proj1_SampleConApp.Week_2
{
    class InvalidIdException : Exception
    {
        public InvalidIdException() : base("Invalid Id !!!!!!!")
        {

        }
        public InvalidIdException(string message) : base(message) { }
    }
    class ProductData
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int CategoryId { get; set; }

        public int ProductStocks { get; set; }
    }
    class Program
    {
        const string STRSELECT = "SELECT * FROM PRODUCTTABLE";
        const string STRCONNECTION = @"Data Source=W-674PY03-2;Initial Catalog=Pratyush_DB;Persist Security Info=True;User ID=SA;Password=Password@123456-=";
        static List<ProductData> getAllProducts()
        {
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(STRSELECT, con);
            List<ProductData> records = new List<ProductData>();
            try
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var prod = new ProductData
                    {
                        ProductId = Convert.ToInt32(reader[0]),
                        ProductName = reader[1].ToString(),
                        ProductPrice = Convert.ToDouble(reader[2]),
                        CategoryId = Convert.IsDBNull(reader[3]) ? 1 : Convert.ToInt32(reader[3]),
                        ProductStocks=Convert.ToInt32(reader[4])
                    };
                    records.Add(prod);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return records;
        }
        private static void readAllProducts()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = STRCONNECTION;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = STRSELECT;
            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"ProductName : {reader["ProductName"]}   ProductPrice: {reader["ProductPrice"]} ProductStocks : {reader["ProductStocks"]}");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        static void Main(string[] args)
        {

            bool processing = true;
            do
            {
                Console.WriteLine("Menu");
                Console.WriteLine("1- Show All Products");
                Console.WriteLine("2- Insert a Product into warehose");
                Console.WriteLine("3- Update a product in warehouse");
                Console.WriteLine("4- Delete a product in warehouse");
                Console.WriteLine("5- Exit");

                Console.WriteLine("Enter Your Choice: ");
                string choice = Console.ReadLine();

                switch(choice)
                {

                    case "1": readAllProducts(); break;
                    case "2":
                        {
                            string name = Utilities.GetString("enter the product to be inserted in warehouse: ");
                            int price = Utilities.GetInteger("enter the price of the product i.e to be inserted in warehouse: ");
                            int category = Utilities.GetInteger("enter the cateogry type {1-electronics, 2-food, 3-clothes, or 4-beverages}");
                            int stocks = Utilities.GetInteger("enter the stocks of the product: ");
                            insertProductToRecord(name, price, category, stocks);
                            List<ProductData> productlist = getAllProducts();
                            foreach (var product in productlist)
                            {
                                Console.WriteLine($"productid: {product.ProductId} product named {product.ProductName} is of price {product.ProductPrice} rupees only");
                            }
                        } break;
                    case "3":
                        {
                            int id = Utilities.GetInteger("enter id of the product to be updated: ");
                            string name = Utilities.GetString("enter the name of the product to be updated");
                            double price = Utilities.GetDouble("enter the price of the product to be updated");
                            int categoryid = Utilities.GetInteger("enter the category of the product to be updated i,e 1-electronics, 2-food, 3-clothes, or 4-beverages");
                            int stocks = Utilities.GetInteger("enter the stocks of the product to be updated: ");

                            ProductData productup = new ProductData { ProductId = id, ProductName = name, ProductPrice = price, CategoryId = categoryid, ProductStocks=stocks };
                            List<ProductData> productlist = getAllProducts();
                            foreach (var product in productlist)
                                if (product.ProductId == id)
                                {
                                    updateProductToRecord(id, productup);
                                    readAllProducts();
                                }
                            

                        }break;
                    case "4":
                        {
                            List<ProductData> productlist = getAllProducts();
                            try
                            {
                                int id = Utilities.GetInteger("enter id of the product to be deleted: ");
                                foreach (var product in productlist)
                                    if (product.ProductId == id)
                                    {
                                        deleteProductFromRecord(id);
                                        readAllProducts();
                                    }
                            }
                            catch
                            {
                                throw new InvalidIdException("Invalid Id !!!!!!!");
                            }
                            //List<ProductData> productlist2 = getAllProducts();
                            //foreach (var product in productlist2)
                            //    Console.WriteLine($"productid: {product.ProductId} Product named {product.ProductName} is of price {product.ProductPrice} rupees only");

                        }
                        break;
                    case "5": processing = false; break;
                    default: Console.WriteLine("Invalid choice, Please try again !!!"); break;
                }

            } while (processing);
           
           
           
        }
        private static void deleteProductFromRecord(int id)
        {
            string query = $"Delete from ProductTable where ProductId ={id}";
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows == 1)
                {
                    Console.WriteLine("Product deleted successfully");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        private static void updateProductToRecord(int id, ProductData product)
        {
            string query = $"update ProductTable set ProductName = '{product.ProductName}',ProductPrice={product.ProductPrice},CategoryId ={product.CategoryId},ProductStocks={product.ProductStocks} where ProductId ={id}";
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows == 1)
                    Console.WriteLine("Product updated successfully");
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void insertProductToRecord(string name, double price, int category, int stocks)
        {
            string query = $"insert into ProductTable values('{name}',{price},{category},{stocks})";
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows == 1)
                    Console.WriteLine("Product inserted successfully");
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

