using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using FinalProject.Models;

namespace FinalProject.Repository
{
    public class User_Repository
    {

        String conString = ConfigurationManager.ConnectionStrings["Myconnection"].ToString();

        //Get All Products
        public List<UserClass> GetAllUsers()
        {
            List<UserClass> UserList = new List<UserClass>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetAllUsers";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtUsers = new DataTable();

                connection.Open();
                sqlDA.Fill(dtUsers);
                connection.Close();

                foreach (DataRow dr in dtUsers.Rows)
                {
                    UserList.Add(new UserClass
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]),
                        Gender = dr["Gender"].ToString(),
                        PhoneNumber = dr["PhoneNumber"].ToString(),
                        EmailAddress = dr["EmailAddress"].ToString(),
                        Address = dr["Address"].ToString(),
                        Country = dr["Country"].ToString(),
                        State = dr["State"].ToString(),
                        City = dr["City"].ToString(),
                        Postcode = dr["Postcode"].ToString(),
                        PassportNumber = dr["PassportNumber"].ToString(),
                        AdharNumber = dr["AdharNumber"].ToString(),
                        Username = dr["Username"].ToString(),
                    });
                }
            }
            return UserList;
        }


        //Get All Products by ID
        public List<UserClass> GetUsersByID(int ID)
        {
            List<UserClass> UserList = new List<UserClass>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetUsersByID";
                command.Parameters.AddWithValue("@ID", ID);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    UserList.Add(new UserClass
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]),
                        Gender = dr["Gender"].ToString(),
                        PhoneNumber = dr["PhoneNumber"].ToString(),
                        EmailAddress = dr["EmailAddress"].ToString(),
                        Address = dr["Address"].ToString(),
                        Country = dr["Country"].ToString(),
                        State = dr["State"].ToString(),
                        City = dr["City"].ToString(),
                        Postcode = dr["Postcode"].ToString(),
                        PassportNumber = dr["PassportNumber"].ToString(),
                        AdharNumber = dr["AdharNumber"].ToString(),
                        Username = dr["Username"].ToString(),
                    });
                }
            }
            return UserList;
        }


        //Update Products
        public bool UpdateUsers(UserClass user)
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateUsers", connection);
                command.Parameters.AddWithValue("@ID", user.ID);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                command.Parameters.AddWithValue("@Gender", user.Gender);
                command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                command.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);
                command.Parameters.AddWithValue("@Address", user.Address);
                command.Parameters.AddWithValue("@Country", user.Country);
                command.Parameters.AddWithValue("@State", user.State);
                command.Parameters.AddWithValue("@City", user.City);
                command.Parameters.AddWithValue("@Postcode", user.Postcode);
                command.Parameters.AddWithValue("@PassportNumber", user.PassportNumber);
                command.Parameters.AddWithValue("@AdharNumber", user.AdharNumber);
                command.Parameters.AddWithValue("@Username", user.Username);

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

        //Delete Product
        public string DeleteProduct(int userid)
        {
            string result = "";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("SP_DELETEUSER", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID",userid);
                command.Parameters.Add("@OUTPUTMESSAGE", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();
                result = command.Parameters["@OUTPUTMESSAGE"].Value.ToString();
                connection.Close();
            }
            return result;
        }

    }
}