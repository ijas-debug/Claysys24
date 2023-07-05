using FinalProject.Models;
using FinalProject.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;


namespace FinalProject.Controllers
{
    public class AdminRegController : Controller
    {
        // GET: AdminReg
        public ActionResult Index()
        {
            return View();
        }

        // Post: AdminReg
        [HttpPost]
        public ActionResult Index(Admin ac, HttpPostedFileBase file)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            SqlCommand sqlCommand = new SqlCommand("sp_InsertAdminReg", sqlconn);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@FirstName", ac.FirstName);
            sqlCommand.Parameters.AddWithValue("@LastName", ac.LastName);
            sqlCommand.Parameters.AddWithValue("@DateOfBirth", ac.DateOfBirth);
            sqlCommand.Parameters.AddWithValue("@Gender", ac.Gender);
            sqlCommand.Parameters.AddWithValue("@PhoneNumber", ac.PhoneNumber);
            sqlCommand.Parameters.AddWithValue("@EmailAddress", ac.EmailAddress);
            sqlCommand.Parameters.AddWithValue("@Address", ac.Address);
            sqlCommand.Parameters.AddWithValue("@Country", ac.Country);
            sqlCommand.Parameters.AddWithValue("@State", ac.State);
            sqlCommand.Parameters.AddWithValue("@City", ac.City);
            sqlCommand.Parameters.AddWithValue("@Postcode", ac.Postcode);
            sqlCommand.Parameters.AddWithValue("@PassportNumber", ac.PassportNumber);
            sqlCommand.Parameters.AddWithValue("@AdharNumber", ac.AdharNumber);
            sqlCommand.Parameters.AddWithValue("@Username", ac.Username);
            sqlCommand.Parameters.AddWithValue("@Password", ac.Password);


            if (file != null && file.ContentLength > 0)
            {

                string filename = Path.GetFileName(file.FileName);
                string imgpath = Path.Combine(Server.MapPath("/Admin-Images/"), filename);
                file.SaveAs(imgpath);
                sqlCommand.Parameters.AddWithValue("@Photo", "/Admin-Images/" + filename);
            }
            else
            {
                sqlCommand.Parameters.AddWithValue("@Photo", DBNull.Value);
            }
            sqlconn.Open();
            sqlCommand.ExecuteNonQuery();
            sqlconn.Close();

            ViewData["Message"] = "User Record " + ac.FirstName + " Is Saved Successfully!";
            return View();


        }


        
    }
}