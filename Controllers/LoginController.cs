using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace FinalProject.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // GET: Login
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(LoginClass lc)
        {

            string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            SqlCommand sqlcomm = new SqlCommand("sp_FinalLogin", sqlconn);

            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Parameters.AddWithValue("@EmailAddress", lc.EmailAddress);
            sqlcomm.Parameters.AddWithValue("@Password", lc.Password);

            sqlconn.Open();
            SqlDataReader sqr = sqlcomm.ExecuteReader();

            if (sqr.Read())
            {
                FormsAuthentication.SetAuthCookie(lc.EmailAddress, true);
                Session["emailid"] = lc.EmailAddress.ToString();
                return RedirectToAction("welcome", "Login");
                

            }
            else
            {
                ViewData["message"] = "Username & Password are wrong !";
            }
            sqlconn.Close();
            return View();
        }

        public ActionResult welcome()
        {
            string displayimg = (string)Session["emailid"];
            string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlquery = "select * from [dbo].[UserReg] where EmailAddress='" + displayimg + "'";
            sqlconn.Open();
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlcomm.Parameters.AddWithValue("EmailAddress", Session["emailid"].ToString());
            SqlDataReader sdr = sqlcomm.ExecuteReader();

            UserClass user = new UserClass();
            if (sdr.Read())
            {
                string s = sdr["Photo"].ToString();
                ViewData["Img"] = s;
                TempData["Oldimg"] = s;


                user.FirstName = sdr["FirstName"].ToString();
                user.LastName = sdr["LastName"].ToString();
                user.DateOfBirth = (DateTime)sdr["DateOfBirth"];
                user.Gender = sdr["Gender"].ToString();
                user.PhoneNumber = sdr["PhoneNumber"].ToString();
                user.EmailAddress = sdr["EmailAddress"].ToString();
                user.Address = sdr["Address"].ToString();
                user.Country = sdr["Country"].ToString();
                user.State = sdr["State"].ToString();
                user.City = sdr["City"].ToString();
                user.Postcode = sdr["Postcode"].ToString();
                user.PassportNumber = sdr["PassportNumber"].ToString();
                user.AdharNumber = sdr["AdharNumber"].ToString();
                user.Username = sdr["Username"].ToString();
                user.Password = sdr["Password"].ToString();

            }
            sqlconn.Close();
            return View(user);
        }






       
        public ActionResult userimgchange(HttpPostedFileBase file)
        {
            var emailId = (string)Session["emailid"];

            string imgpath = Server.MapPath((string)TempData["Oldimg"]);
            string fileimgpath = imgpath;
            FileInfo fi = new FileInfo(fileimgpath);
            if (fi.Exists)
            {
                fi.Delete();
            }

            if (file != null && file.ContentLength > 0)
            {
                string filename = Path.GetFileName(file.FileName);
                string filepath = Path.Combine(Server.MapPath("/User-Images/"), filename);
                file.SaveAs(filepath);

                string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
                using (SqlConnection sqlconn = new SqlConnection(mainconn))
                {
                    sqlconn.Open();
                    string sqlquery = "UPDATE [dbo].[UserReg] SET  Photo = @Photo WHERE EmailAddress = @EmailAddress";
                    SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
                    sqlcomm.Parameters.AddWithValue("@Photo", "/User-Images/" + filename);
                    sqlcomm.Parameters.AddWithValue("@EmailAddress", emailId);
                    sqlcomm.ExecuteNonQuery();

                }
            }

            return RedirectToAction("welcome", "Login");
        }


        // POST: Login/UpdateUser
        [HttpPost]
        [ActionName("UpdateUser")]
        public ActionResult UpdateUser(UserClass user)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
            using (SqlConnection sqlconn = new SqlConnection(mainconn))
            {
                sqlconn.Open();
                string sqlquery = @"UPDATE [dbo].[UserReg] SET
                                    FirstName = @FirstName,
                                    LastName = @LastName,
                                    DateOfBirth = @DateOfBirth,
                                    Gender = @Gender,
                                    PhoneNumber = @PhoneNumber,
                                    Address = @Address,
                                    Country = @Country,
                                    State = @State,
                                    City = @City,
                                    Postcode = @Postcode,
                                    PassportNumber = @PassportNumber,
                                    AdharNumber = @AdharNumber,
                                    Username = @Username,
                                    Password = @Password
                                    WHERE EmailAddress = @EmailAddress";
                SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
                sqlcomm.Parameters.AddWithValue("@FirstName", user.FirstName);
                sqlcomm.Parameters.AddWithValue("@LastName", user.LastName);
                sqlcomm.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                sqlcomm.Parameters.AddWithValue("@Gender", user.Gender);
                sqlcomm.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                sqlcomm.Parameters.AddWithValue("@Address", user.Address);
                sqlcomm.Parameters.AddWithValue("@Country", user.Country);
                sqlcomm.Parameters.AddWithValue("@State", user.State);
                sqlcomm.Parameters.AddWithValue("@City", user.City);
                sqlcomm.Parameters.AddWithValue("@Postcode", user.Postcode);
                sqlcomm.Parameters.AddWithValue("@PassportNumber", user.PassportNumber);
                sqlcomm.Parameters.AddWithValue("@AdharNumber", user.AdharNumber);
                sqlcomm.Parameters.AddWithValue("@Username", user.Username);
                sqlcomm.Parameters.AddWithValue("@Password", user.Password);
                sqlcomm.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);

                sqlcomm.ExecuteNonQuery();
            }

            return RedirectToAction("Welcome");
        }

        // POST: Login/DeleteUser
        [HttpPost]
        public ActionResult DeleteUser(string emailAddress)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
            using (SqlConnection sqlconn = new SqlConnection(mainconn))
            {
                sqlconn.Open();
                string sqlquery = "DELETE FROM [dbo].[UserReg] WHERE EmailAddress = @EmailAddress";
                SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
                sqlcomm.Parameters.AddWithValue("@EmailAddress", emailAddress);
                sqlcomm.ExecuteNonQuery();
            }

            // Clear the session and redirect to the login page
            Session.Clear();
            return RedirectToAction("Index");
        }




    }
}
