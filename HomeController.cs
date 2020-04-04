using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using BookingWhizzAdmins.HomeModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BookingWhizzAdmins._ClassModel;
using System.Net.NetworkInformation;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace BookingWhizzAdmins.Controllers
{
    
    public class HomeController : Controller
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);//sqlServer Connection+
         
        public ActionResult afterLogin()
        {
            if (Session["LoginType"] == null)
            {
                Session["LanguageId"] = 1;
                Session["LanguageName"] = "en";
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Demo()
        {
             
            string val = "farooq";
            char[] charArray = val.ToCharArray();
            Array.Reverse(charArray);
            for(int i=0;i<charArray.Length;i++)
            {
                val += charArray[i];
            }
            string stateVal = "FAROOQ";
            char[] aryVal = stateVal.ToCharArray(); 
            for (int i= aryVal.Count()-1; i>=0; i--)
            {
                aryVal[i] += aryVal[i];
            }
            return View();
        }
        [HttpGet]
        public string GetMonthlyBookings()
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SP_GetBookingDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProcedureType", "G");
            cmd.Parameters.AddWithValue("@SessionType", "301");
            cmd.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
            //cmd.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
            SqlParameter _results = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
            _results.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(_results);

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            var set = adp.Fill(dt);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }
        public string GetWeeklyBookings()
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SP_GetBookingDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProcedureType", "G");
            cmd.Parameters.AddWithValue("@SessionType", "302");
            cmd.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
            //cmd.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
            SqlParameter _results = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
            _results.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(_results);

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            var set = adp.Fill(dt);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        //public string SreachAccommodation(string AccmommdationId)
        //{
        //    string AccmommdationName = "";

        //    string Status6 = "";
        //    DataTable dt = new DataTable();
        //    SqlCommand cmd6 = new SqlCommand("SP_Accommodations_Test", con);
        //    cmd6.CommandType = CommandType.StoredProcedure;
        //    cmd6.Parameters.AddWithValue("@ProcedureType", "G");
        //    cmd6.Parameters.AddWithValue("@SessionType", "202");
        //    cmd6.Parameters.AddWithValue("@p_MultiLanguageId", "1");
        //    cmd6.Parameters.AddWithValue("@p_UserId", Session["UserId"]);
        //    cmd6.Parameters.AddWithValue("@p_AccommodationId", AccmommdationId);

        //    SqlDataAdapter da3 = new SqlDataAdapter(cmd6);
        //    var set = da3.Fill(dt);
        //    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> row;
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        row = new Dictionary<string, object>();
        //        foreach (DataColumn col in dt.Columns)
        //        {
        //            row.Add(col.ColumnName, dr[col]);
        //        }
        //        rows.Add(row);
        //    }
        //    return serializer.Serialize(rows);
        //}

        public ActionResult Index()
        {
            
            string DomainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            var idd = Session["LanguageId"];
            var hid = Session["AccommodationId"];
            var hname = Session["AccommodationName"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                con.Open();
                string host = Request.Url.Host;
                if (host == "v2-bw.bookingwhizz.com")
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("insert into BEcheck.dbo.BE_ACS(Company,HotelId,HotelName)values('BE'," + hid + ",'" + hname + "')", con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                    da1.Fill(dt);
                    con.Close();
                }
                else
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("insert into BEcheck.dbo.BE_ACS(Company,HotelId,HotelName)values('ASC'," + hid + ",'" + hname + "')", con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                    da1.Fill(dt); 
                }
            }
            catch(Exception ex)
            { 
            }  
            string Status = "";
            SqlCommand comm = new SqlCommand("SP_BookingEngineDashboard", con);

            comm.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
            comm.CommandType = CommandType.StoredProcedure;

            SqlParameter TotalReservations_out = new SqlParameter("@P_TotalReservations", SqlDbType.NVarChar, 100);
            TotalReservations_out.Direction = ParameterDirection.Output;
            comm.Parameters.Add(TotalReservations_out);

            SqlParameter Last30Reservations_out = new SqlParameter("@P_Last30Reservations", SqlDbType.NVarChar, 100);
            Last30Reservations_out.Direction = ParameterDirection.Output;
            comm.Parameters.Add(Last30Reservations_out);

            SqlParameter WeeklyReservations_out = new SqlParameter("@P_WeeklyReservations", SqlDbType.NVarChar, 100);
            WeeklyReservations_out.Direction = ParameterDirection.Output;
            comm.Parameters.Add(WeeklyReservations_out);

            SqlParameter TodaysReservations_out = new SqlParameter("@P_TodaysReservations", SqlDbType.NVarChar, 100);
            TodaysReservations_out.Direction = ParameterDirection.Output;
            comm.Parameters.Add(TodaysReservations_out);

            SqlParameter TodayCheckIn_out = new SqlParameter("@P_TodayCheckIn", SqlDbType.NVarChar, 100);
            TodayCheckIn_out.Direction = ParameterDirection.Output;
            comm.Parameters.Add(TodayCheckIn_out);

            SqlParameter TodayCheckOut_out = new SqlParameter("@P_TodayCheckOut", SqlDbType.NVarChar, 100);
            TodayCheckOut_out.Direction = ParameterDirection.Output;
            comm.Parameters.Add(TodayCheckOut_out);
             
            comm.ExecuteNonQuery();
            Session["TotalReservations"] = comm.Parameters["@P_TotalReservations"].Value.ToString();
            Session["Last30Reservations"] = comm.Parameters["@P_Last30Reservations"].Value.ToString();
            Session["WeeklyReservations"] = comm.Parameters["@P_WeeklyReservations"].Value.ToString();
            Session["TodaysReservations"] = comm.Parameters["@P_TodaysReservations"].Value.ToString();
            Session["TodayCheckIn"] = comm.Parameters["@P_TodayCheckIn"].Value.ToString();
            Session["TodayCheckOut"] = comm.Parameters["@P_TodayCheckOut"].Value.ToString();


            con.Close();
            return View();
        } 
        public ActionResult ChannelManager()
        {
            string host = Request.Url.Host;
            if (host == "v2-bw.bookingwhizz.com")
            {
                var Domain = "http://channelmanager.bookingwhizz.com/Home/ConnectBE?LoginId=" + Session["LoginId"];
                return Redirect(Domain);
            }
            else
            { 
                var Domain = "http://channelmanager.ascendant.travel/Home/ConnectBE?LoginId=" + Session["LoginId"];
                return Redirect(Domain);
            }
        } 
        public ActionResult ConnectCM(string LoginId)
        {
            try
            {
                string Status = "";
                SqlCommand cmd = new SqlCommand("SP_Admin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProcedureType", "G");
                cmd.Parameters.AddWithValue("@SessionType", "101");
                cmd.Parameters.AddWithValue("@p_LoginId", LoginId); 

                SqlParameter _results = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                _results.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(_results);
                Status = _results.ToString();
                //
                SqlParameter AccommodationId = new SqlParameter("@p_UserAccommodationId_Return", SqlDbType.NVarChar, 100);
                AccommodationId.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(AccommodationId);
                //
                SqlParameter AccommodationName = new SqlParameter("@p_UserAccommodationName_Return", SqlDbType.NVarChar, 100);
                AccommodationName.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(AccommodationName);
                //
                SqlParameter WebSearch = new SqlParameter("@p_WebSearch_Return", SqlDbType.NVarChar, 100);
                WebSearch.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(WebSearch);
                //
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                con.Open();
                cmd.ExecuteNonQuery();

                SqlDataReader dr12 = cmd.ExecuteReader();
                if (dr12.HasRows)
                {
                    while (dr12.Read())
                    {
                        Session["UserId"] = dr12["UserId"];
                    }
                }
                con.Close();
                con.Open();
                string GetAccommodationId = cmd.Parameters["@p_UserAccommodationId_Return"].Value.ToString();
                string GetAccommodationName = cmd.Parameters["@p_UserAccommodationName_Return"].Value.ToString();
                string GetWebSearch = cmd.Parameters["@p_WebSearch_Return"].Value.ToString();
                Session["AccommodationId"] = GetAccommodationId;
                Session["AccommodationName"] = GetAccommodationName;
                Session["WebSearch"] = GetWebSearch;
                Session["LanguageId"] = 1;
                // Session["LanguageName"] = "en";
                Status = _results.Value.ToString();
                if (Status == "Record not found" || Status == "")
                {
                    TempData["error"] = "Check your email and password and try again";
                    return View();
                }
                if (Status == "Success")
                {

                    string cookievalue;
                    if (Request.Cookies[Session["UserId"].ToString()] != null)
                    {
                        SqlDataReader dr1 = cmd.ExecuteReader();
                        if (dr1.HasRows)
                        {
                            while (dr1.Read())
                            {
                                Session["guid"] = dr1["GlobalId"];
                                Session["User"] = dr1["UserTypeName"];
                            }
                        }
                        con.Close();
                        var cookeval = Request.Cookies[Session["UserId"].ToString()].Value;
                        var Uid = cookeval.Split('^')[0];
                        var Guid = cookeval.Split('^')[1];
                        string C_Gid = Session["guid"].ToString();


                        if (Guid == C_Gid)
                        {

                            con.Open();
                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    Session["LoginId"] = dr["LoginId"];
                                    Session["UserTypeId"] = dr["UserTypeId"];
                                    Session["LoginType"] = dr["UserTypeName"];
                                    Session["UserId"] = dr["UserId"];
                                    Session["guid"] = dr["GlobalId"];
                                    Session["OwnerId"] = dr["UserId"];
                                    Session["UserName"] = dr["UserName"];
                                }
                            }
                            int SessionId = int.Parse(Session["UserTypeId"].ToString());
                            if (SessionId > 4)
                            {
                                return RedirectToAction("MappedAccommodations", "Home");
                            }
                            return RedirectToAction("Index", "Home");
                        }
                        //  }
                        //
                        else
                        {
                            con.Close();
                            con.Open();
                            SqlDataReader dr2 = cmd.ExecuteReader();
                            if (dr2.HasRows)
                            {
                                while (dr2.Read())
                                {
                                    Session["LoginId"] = dr2["LoginId"];
                                    //Session["UserTypeId"] = dr2["UserTypeId"];
                                    //Session["UserId"] = dr2["UserId"];
                                    //Session["guid"] = dr2["GlobalId"];
                                }
                            }
                            con.Close();
                            con.Open();
                            Random rnd = new Random();

                            int randomNumber = rnd.Next(102, 13000);

                            SqlCommand cm = new SqlCommand("update Users set ActivationCode=" + randomNumber + "  where UserId=" + Session["UserId"].ToString(), con);
                            cm.ExecuteNonQuery();
                            con.Close();

                            //var guid = Session["guid"].ToString();
                            //var uid = Session["UserId"] + "-" + guid;
                            //Response.Cookies["cookie"].Value = uid;
                            // email 

                            string domainName = Request.Url.Host.ToLower();
                            if (domainName == "bookingengine.ascendant.travel")
                            {
                                string _toEmail = Session["LoginId"].ToString();
                                string _body = "<h1 style='width: 550px;background-color:#312020;color:white;padding: 20px 0;text-align: center;'>	Ascendant Verification Code</h1><div style='width: 509px;background-color: #e2e1e1;padding: 20px;font-size: 14px;'><h5 style='margin-top:-30px;'>Dear Ascendant User,</h5> <p style='line-height:30px;'>For added security, you are receiving this verification code to verify your identity for Ascendant Booking Engine Login Page.</p>" + "<p style='text-align:center;font-size: 14px;font - style: oblique;'>Code:<span style='font-size:24px;font-weight:700;color:#479fd4;background-color:#ffffff70;padding:4px12px;border-radius:8px;border:2px solid #ccc;'>" + randomNumber + "</span></p></div>";
                                string _subject = "Login Verification";
                                WebMail.SmtpServer = "smtp.office365.com";
                                WebMail.SmtpPort = 587;
                                WebMail.SmtpUseDefaultCredentials = true;
                                WebMail.EnableSsl = true;
                                WebMail.UserName = "customer.service@ascendant.travel";
                                WebMail.Password = "Asc@bw$051";
                                WebMail.From = "customer.service@ascendant.travel";
                                WebMail.Send(to: _toEmail, subject: _subject, body: _body, isBodyHtml: true);
                                ViewBag.Status = "Email Sent Successfully.";
                            }
                            else
                            {
                                string toemail = Session["LoginId"].ToString();
                                string Body = "<h1 style='width: 550px;background-color:#312020;color:white;padding: 20px 0;text-align: center;'>	BookingWhizz Verification Code</h1><div style='width: 509px;background-color: #e2e1e1;padding: 20px;font-size: 14px;'><h5 style='margin-top:-30px;'>Dear BookingWhizz User,</h5> <p style='line-height:30px;'>For added security, you are receiving this verification code to verify your identity for BookingWhizz Booking Engine Login Page.</p>" + "<p style='text-align:center;font-size: 14px;font - style: oblique;'>Code:<span style='font-size:24px;font-weight:700;color:#479fd4;background-color:#ffffff70;padding:4px12px;border-radius:8px;border:2px solid #ccc;'>" + randomNumber + "</span></p></div>"; string Subject = "Code Verification";
                                WebMail.SmtpServer = "smtp.gmail.com";
                                WebMail.SmtpPort = 587;
                                WebMail.SmtpUseDefaultCredentials = true;
                                WebMail.EnableSsl = true;
                                WebMail.UserName = "bookingwhizz00@gmail.com";
                                WebMail.Password = "karim@52";
                                WebMail.From = "bookingwhizz00@gmail.com";
                                WebMail.Send(to: toemail, subject: Subject, body: Body, isBodyHtml: true);
                                ViewBag.Status = "Email Sent Successfully.";
                            }
                            //
                            return RedirectToAction("Verfication", "Account");
                        }
                    }
                    else
                    {
                        con.Close();
                        con.Open();
                        SqlDataReader dr2 = cmd.ExecuteReader();
                        if (dr2.HasRows)
                        {
                            while (dr2.Read())
                            {
                                Session["LoginId"] = dr2["LoginId"];
                                //Session["UserTypeId"] = dr2["UserTypeId"];
                                //Session["UserId"] = dr2["UserId"];
                                Session["guid"] = dr2["GlobalId"];
                            }
                        }
                        con.Close();
                        con.Open();
                        Random rnd = new Random();

                        int randomNumber = rnd.Next(102, 13000);

                        SqlCommand cm = new SqlCommand("update Users set ActivationCode=" + randomNumber + "  where UserId=" + Session["UserId"].ToString(), con);
                        cm.ExecuteNonQuery();
                        con.Close();

                        //var guid = Session["guid"].ToString();
                        //var uid = Session["UserId"] + "-" + guid;
                        //Response.Cookies["cookie"].Value = uid;
                        // email 
                        string domainName = Request.Url.Host.ToLower();
                        if (domainName == "bookingengine.ascendant.travel")
                        {
                            string _toEmail = Session["LoginId"].ToString();
                            string _body = "<h1 style='width: 550px;background-color:#312020;color:white;padding: 20px 0;text-align: center;'>	Ascendant Verification Code</h1><div style='width: 509px;background-color: #e2e1e1;padding: 20px;font-size: 14px;'><h5 style='margin-top:-30px;'>Dear Ascendant User,</h5> <p style='line-height:30px;'>For added security, you are receiving this verification code to verify your identity for Ascendant Booking Engine Login Page.</p>" + "<p style='text-align:center;font-size: 14px;font - style: oblique;'>Code:<span style='font-size:24px;font-weight:700;color:#479fd4;background-color:#ffffff70;padding:4px12px;border-radius:8px;border:2px solid #ccc;'>" + randomNumber + "</span></p></div>";
                            string _subject = "Login Verification";
                            WebMail.SmtpServer = "smtp.office365.com";
                            WebMail.SmtpPort = 587;
                            WebMail.SmtpUseDefaultCredentials = true;
                            WebMail.EnableSsl = true;
                            WebMail.UserName = "customer.service@ascendant.travel";
                            WebMail.Password = "Asc@bw$051";
                            WebMail.From = "customer.service@ascendant.travel";
                            WebMail.Send(to: _toEmail, subject: _subject, body: _body, isBodyHtml: true);
                            ViewBag.Status = "Email Sent Successfully.";
                        }
                        else
                        {
                            string toemail = Session["LoginId"].ToString();
                            string Body = "<h1 style='width: 550px;background-color:#312020;color:white;padding: 20px 0;text-align: center;'>	BookingWhizz Verification Code</h1><div style='width: 509px;background-color: #e2e1e1;padding: 20px;font-size: 14px;'><h5 style='margin-top:-30px;'>Dear BookingWhizz User,</h5> <p style='line-height:30px;'>For added security, you are receiving this verification code to verify your identity for BookingWhizz Booking Engine Login Page.</p>" + "<p style='text-align:center;font-size: 14px;font - style: oblique;'>Code:<span style='font-size:24px;font-weight:700;color:#479fd4;background-color:#ffffff70;padding:4px12px;border-radius:8px;border:2px solid #ccc;'>" + randomNumber + "</span></p></div>"; string Subject = "Code Verification";
                            WebMail.SmtpServer = "smtp.gmail.com";
                            WebMail.SmtpPort = 587;
                            WebMail.SmtpUseDefaultCredentials = true;
                            WebMail.EnableSsl = true;
                            WebMail.UserName = "bookingwhizz00@gmail.com";
                            WebMail.Password = "karim@52";
                            WebMail.From = "bookingwhizz00@gmail.com";
                            WebMail.Send(to: toemail, subject: Subject, body: Body, isBodyHtml: true);
                            ViewBag.Status = "Email Sent Successfully.";
                        }
                        //
                        return RedirectToAction("Verfication", "Account");
                    }

                }
                else
                {
                    //TempData["error"] = "Invalid Verfication";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "System Error : Contact Technical Support" + ex;
                return RedirectToAction("Error", "Home");
            }
            return View();
        }
        public string GetCheckIn()
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SP_GetBookingDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProcedureType", "G");
            cmd.Parameters.AddWithValue("@SessionType", "202");
            cmd.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
            //cmd.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
            SqlParameter _results = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
            _results.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(_results);

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            var set = adp.Fill(dt);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }
        public string GetCheckOut()
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SP_GetBookingDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProcedureType", "G");
            cmd.Parameters.AddWithValue("@SessionType", "203");
            cmd.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
            //cmd.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
            SqlParameter _results = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
            _results.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(_results);

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            var set = adp.Fill(dt);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }
        public string GetTopTENSaller()
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SP_BookingEngineDashboard2", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProcedureType", "G");
            cmd.Parameters.AddWithValue("@SessionType", "103");
            cmd.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
            //cmd.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
            SqlParameter _results = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
            _results.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(_results);

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            var set = adp.Fill(dt);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }
        public string GetTopBookings()
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SP_BookingEngineDashboard2", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProcedureType", "G");
            cmd.Parameters.AddWithValue("@SessionType", "101");
            cmd.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
            //cmd.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
            SqlParameter _results = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
            _results.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(_results);

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            var set = adp.Fill(dt);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        } 
        public string GetBookingSource()
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SP_BookingEngineDashboard2", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProcedureType", "G");
            cmd.Parameters.AddWithValue("@SessionType", "102");
            cmd.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
            //cmd.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
            SqlParameter _results = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
            _results.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(_results);

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            var set = adp.Fill(dt);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }
        public ActionResult AddNewAccommodations()// Add_New_Accommodations
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            try
            {
                if (Session["LoginType"] == null)
                {
                    return RedirectToAction("Login", "Account");
                } 
                //Currency
                string _status_Currency = "";
                DataSet _ds_Currency = new DataSet();
                SqlCommand _cmd_Currency = new SqlCommand("SP_Accommodations", con);
                _cmd_Currency.CommandType = CommandType.StoredProcedure;
                _cmd_Currency.Parameters.AddWithValue("@ProcedureType", "G");
                _cmd_Currency.Parameters.AddWithValue("@SessionType", "107");
                SqlParameter _results_Currency = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                _results_Currency.Direction = ParameterDirection.Output;
                _cmd_Currency.Parameters.Add(_results_Currency);

                SqlDataAdapter _sda_Currency = new SqlDataAdapter(_cmd_Currency);
                con.Open();
                _cmd_Currency.ExecuteNonQuery();
                _status_Currency = _results_Currency.Value.ToString();
                _sda_Currency.Fill(_ds_Currency);
                 
                //Accommodation Type
                string _status_AccommodationType = "";
                DataSet _ds_AccommodationType = new DataSet();
                SqlCommand _cmd_AccommodationType = new SqlCommand("SP_Accommodations", con);
                _cmd_AccommodationType.CommandType = CommandType.StoredProcedure;
                _cmd_AccommodationType.Parameters.AddWithValue("@ProcedureType", "G");
                _cmd_AccommodationType.Parameters.AddWithValue("@SessionType", "112");
                _cmd_AccommodationType.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                SqlParameter _results_AccommodationType = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                _results_AccommodationType.Direction = ParameterDirection.Output;
                _cmd_AccommodationType.Parameters.Add(_results_AccommodationType);

                SqlDataAdapter _adp_AccommodationType = new SqlDataAdapter(_cmd_AccommodationType);
                _cmd_AccommodationType.ExecuteNonQuery();
                _status_AccommodationType = _results_AccommodationType.Value.ToString();
                _adp_AccommodationType.Fill(_ds_AccommodationType);
                //
                //Country
                string _status_Country = "";
                DataSet _ds_Country = new DataSet();
                SqlCommand _cmd_Country = new SqlCommand("SP_Accommodations", con);
                _cmd_Country.CommandType = CommandType.StoredProcedure;
                _cmd_Country.Parameters.AddWithValue("@ProcedureType", "G");
                _cmd_Country.Parameters.AddWithValue("@SessionType", "103");
                _cmd_Country.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                SqlParameter _results_Country = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                _results_Country.Direction = ParameterDirection.Output;
                _cmd_Country.Parameters.Add(_results_Country);

                SqlDataAdapter _sda_Country = new SqlDataAdapter(_cmd_Country);
                _cmd_Country.ExecuteNonQuery();
                _status_Country = _results_Country.Value.ToString();
                _sda_Country.Fill(_ds_Country);
                

                //TimeZone
                string _status_TimeZone = "";
                DataSet _ds_TimeZone = new DataSet();
                SqlCommand _cmd_TimeZone = new SqlCommand("SP_Accommodations", con);
                _cmd_TimeZone.CommandType = CommandType.StoredProcedure;
                _cmd_TimeZone.Parameters.AddWithValue("@ProcedureType", "G");
                _cmd_TimeZone.Parameters.AddWithValue("@SessionType", "113");
                SqlParameter _results_TimeZone = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                _results_TimeZone.Direction = ParameterDirection.Output;
                _cmd_TimeZone.Parameters.Add(_results_TimeZone);

                SqlDataAdapter _sda_TimeZone = new SqlDataAdapter(_cmd_TimeZone);
                _cmd_TimeZone.ExecuteNonQuery();
                _status_TimeZone = _results_TimeZone.Value.ToString();
                _sda_TimeZone.Fill(_ds_TimeZone);
                //
                DataSet[] _ds_Array = new DataSet[] { _ds_AccommodationType, _ds_Country, _ds_TimeZone, _ds_Currency };
                con.Close();

                return View(_ds_Array);
            }
            catch (Exception ex)
            {
             TempData["Error"] = "System Error : Contact Technical Support";
                return RedirectToAction("Error", "Home");
            }
        }
        //public string SelectedCountry(int MultiLanguageId)
        //{
        //    DataTable dt = new DataTable();
        //    string qurey3 = "SP_Accommodations_Test";
        //    SqlCommand cmd3 = new SqlCommand(qurey3, con);
        //    cmd3.Parameters.AddWithValue("@ProcedureType", "G");
        //    cmd3.Parameters.AddWithValue("@SessionType", "4");
        //    cmd3.Parameters.AddWithValue("@p_MultiLanguageId", "1");
        //    cmd3.CommandType = CommandType.StoredProcedure;
        //    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
        //    var set = da3.Fill(dt);
        //    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> row;
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        row = new Dictionary<string, object>();
        //        foreach (DataColumn col in dt.Columns)
        //        {
        //            row.Add(col.ColumnName, dr[col]);
        //        }
        //        rows.Add(row);
        //    }
        //    return serializer.Serialize(rows);
        //}
        public string SelectedCity(int _countryId)
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            DataTable dt = new DataTable();
            string qurey3 = "SP_Accommodations";
            SqlCommand cmd3 = new SqlCommand(qurey3, con);
            cmd3.Parameters.AddWithValue("@ProcedureType", "G");
            cmd3.Parameters.AddWithValue("@SessionType", "105");
            cmd3.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
            cmd3.Parameters.AddWithValue("@p_CountryId", _countryId);
            cmd3.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            var set = da3.Fill(dt);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }
        [HttpPost]
        public ActionResult AddNewAccommodations(_AddAccommodation _add_AC)// GEt  Accommodations
        {
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                string _status = "";
                SqlCommand _cmd_AddAccommodation = new SqlCommand("SP_Accommodations", con);
                _cmd_AddAccommodation.CommandType = CommandType.StoredProcedure;
                _cmd_AddAccommodation.Parameters.AddWithValue("@ProcedureType", "I");
                _cmd_AddAccommodation.Parameters.AddWithValue("@SessionType", "1");
                _cmd_AddAccommodation.Parameters.AddWithValue("@p_MultiLanguageId", "1");
                _cmd_AddAccommodation.Parameters.AddWithValue("@p_OwnerId", Session["OwnerId"]);
                _cmd_AddAccommodation.Parameters.AddWithValue("@p_AccommodationTypeId", _add_AC._accommodationTypeId);
                _cmd_AddAccommodation.Parameters.AddWithValue("@p_PropertyCategory", _add_AC._propertyCategory);
                _cmd_AddAccommodation.Parameters.AddWithValue("@p_AddressId", _add_AC._addressId); 
                _cmd_AddAccommodation.Parameters.AddWithValue("@p_CurrencyId", _add_AC._currencyId); 
                _cmd_AddAccommodation.Parameters.AddWithValue("@p_AccommodationName", _add_AC._accommodationName);
                _cmd_AddAccommodation.Parameters.AddWithValue("@p_TimeZoneId", _add_AC._timeZoneId);
                _cmd_AddAccommodation.Parameters.AddWithValue("@p_contactNo", _add_AC._contactNo);
                _cmd_AddAccommodation.Parameters.AddWithValue("@p_EmailId", _add_AC._emailId);
                _cmd_AddAccommodation.Parameters.AddWithValue("@p_PostCode", _add_AC._postCode);
                _cmd_AddAccommodation.Parameters.AddWithValue("@p_Latitude", _add_AC._latitude);
                _cmd_AddAccommodation.Parameters.AddWithValue("@p_Longitude", _add_AC._longitude);
                _cmd_AddAccommodation.Parameters.AddWithValue("@p_WebSearch", _add_AC._webSearch);
                _cmd_AddAccommodation.Parameters.AddWithValue("@p_HomeShopping", _add_AC._HomeShopping);
                SqlParameter _results_AddAccommodation = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                _results_AddAccommodation.Direction = ParameterDirection.Output;
                _cmd_AddAccommodation.Parameters.Add(_results_AddAccommodation);

                con.Open();
                _cmd_AddAccommodation.ExecuteNonQuery();
                _status = _results_AddAccommodation.Value.ToString();
                 
                if (_status == "")
                {
                    TempData["Error"] = "Server Error : Contact Technical Support";
                }
                else
                {

                    if (Session["WebSearch"] != null)
                    {
                        if (int.Parse(Session["WebSearch"].ToString()) == 1)
                        {
                            HttpWebRequest request = WebRequest.Create("http://roomph.pk/admin/home/fetch-accommodations/" + Session["AccommodationId"]) as HttpWebRequest;
                            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                        }
                    }
                    TempData["okey"] = "Accommodation Created!"; 

                    //DataTable _dt_GetAccommodationDDL = new DataTable();
                    //SqlCommand _cmd_GetAccommodationDDL = new SqlCommand("SP_Accommodations_Test", con);
                    //_cmd_GetAccommodationDDL.CommandType = CommandType.StoredProcedure;
                    //_cmd_GetAccommodationDDL.Parameters.AddWithValue("@ProcedureType", "G");
                    //_cmd_GetAccommodationDDL.Parameters.AddWithValue("@SessionType", "202");
                    //_cmd_GetAccommodationDDL.Parameters.AddWithValue("@p_MultiLanguageId", "1");
                    //_cmd_GetAccommodationDDL.Parameters.AddWithValue("@p_UserId", Session["UserId"]);
                    //SqlParameter _results_GetAccommodationDDL = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    //_results_GetAccommodationDDL.Direction = ParameterDirection.Output;
                    //_cmd_GetAccommodationDDL.Parameters.Add(_results_GetAccommodationDDL);
                    //SqlDataAdapter _sda_GetAccommodationDDL = new SqlDataAdapter(_cmd_GetAccommodationDDL);
                    //_sda_GetAccommodationDDL.Fill(_dt_GetAccommodationDDL);
                    //Session["SaveDt"] = _dt_GetAccommodationDDL;

                    con.Close();
                    //
                    //int _userTypeId = int.Parse(Session["UserTypeId"].ToString());
                    //if (_userTypeId == 5)
                    //{
                    //    con.Open();
                    //    _cmd_GetAccommodationDDL.ExecuteNonQuery();
                    //    SqlDataReader _sdr_GetAccommodationDDL = _cmd_GetAccommodationDDL.ExecuteReader();
                    //    if (_sdr_GetAccommodationDDL.HasRows)
                    //    {
                    //        while (_sdr_GetAccommodationDDL.Read())
                    //        {
                    //            Session["WebSearch"] = _add_AC._webSearch;
                    //            Session["AccommodationId"] = _sdr_GetAccommodationDDL["AccommodationId"];
                    //            Session["AccommodationName"] = _sdr_GetAccommodationDDL["AccommodationName"];
                    //        }
                    //    }
                    //    con.Close();
                    //}
                    Session["SaveDt"] = null; 
                }
                con.Close();
                return RedirectToAction("AddNewAccommodations", "Home");
            }
            catch (Exception ex)
            {
             TempData["Error"] = "System Error : Contact Technical Support";

                TempData["ErrorPage"] = "Add Accommodations";
                return RedirectToAction("Error", "Home");
            }
        }
        //public ActionResult GetAccommodations()
        //{
        //    if (Session["LoginType"] == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    try
        //    {
        //        string Status6 = "";
        //        DataSet ds6 = new DataSet();
        //        SqlCommand cmd6 = new SqlCommand("SP_Accommodations_Test", con);
        //        cmd6.CommandType = CommandType.StoredProcedure;
        //        cmd6.Parameters.AddWithValue("@ProcedureType", "G");
        //        cmd6.Parameters.AddWithValue("@SessionType", "1");
        //        cmd6.Parameters.AddWithValue("@p_MultiLanguageId", "1");
        //        cmd6.Parameters.AddWithValue("@p_UserTypeId", Session["UserTypeId"]);
        //        cmd6.Parameters.AddWithValue("@p_UserId", Session["UserId"]);
        //        SqlParameter _results6 = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
        //        _results6.Direction = ParameterDirection.Output;
        //        cmd6.Parameters.Add(_results6);

        //        SqlDataAdapter adp6 = new SqlDataAdapter(cmd6);
        //        con.Open();
        //        cmd6.ExecuteNonQuery();
        //        Status6 = _results6.Value.ToString();
        //        adp6.Fill(ds6);
        //        con.Close();
        //        return View(ds6);
        //    }
        //    catch (Exception ex)
        //    {
        //     TempData["Error"] = "System Error : Contact Technical Support";
        //        return RedirectToAction("Error", "Home");
        //    } 
        //}//Get Accommodations
        public ActionResult UpdateAccommodations()// Edit_Accommodations
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["AccommodationId"] == null || Session["AccommodationId"].ToString() == "")
            {
                TempData["Error"] = "System Error : Accommodation Not Found !";
                return RedirectToAction("AddNewAccommodations", "Home");
            }
            try
            {
                string _status_GABId = ""; //Get Accommodation by Id
                DataSet _ds_GABId = new DataSet();
                SqlCommand _cmd_GABId = new SqlCommand("SP_Accommodations", con);
                _cmd_GABId.CommandType = CommandType.StoredProcedure;
                _cmd_GABId.Parameters.AddWithValue("@ProcedureType", "G");
                _cmd_GABId.Parameters.AddWithValue("@SessionType", "1");
                _cmd_GABId.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                _cmd_GABId.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                _cmd_GABId.Parameters.AddWithValue("@p_UserTypeId", Session["UserTypeId"]);
                _cmd_GABId.Parameters.AddWithValue("@p_UserId", Session["UserId"]);
                SqlParameter _results_GABId = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                _results_GABId.Direction = ParameterDirection.Output;
                _cmd_GABId.Parameters.Add(_results_GABId);

                SqlDataAdapter _sda_GABId = new SqlDataAdapter(_cmd_GABId);
                con.Open();
                _cmd_GABId.ExecuteNonQuery();
                _status_GABId = _results_GABId.Value.ToString();
                _sda_GABId.Fill(_ds_GABId);

                //Currency
                string _status_Currency = ""; // Get All Currency
                DataSet _ds_Currency = new DataSet();
                SqlCommand _cmd_Currency = new SqlCommand("SP_Accommodations", con);
                _cmd_Currency.CommandType = CommandType.StoredProcedure;
                _cmd_Currency.Parameters.AddWithValue("@ProcedureType", "G");
                _cmd_Currency.Parameters.AddWithValue("@SessionType", "107");
                SqlParameter _results_Currency = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                _results_Currency.Direction = ParameterDirection.Output;
                _cmd_Currency.Parameters.Add(_results_Currency);

                SqlDataAdapter _sda_Currency = new SqlDataAdapter(_cmd_Currency);

                _cmd_Currency.ExecuteNonQuery();
                _status_Currency = _results_Currency.Value.ToString();
                _sda_Currency.Fill(_ds_Currency);

                //ExtraFee
                string _status_ExtraFee = "";
                DataSet _ds_ExtraFee = new DataSet();
                SqlCommand _cmd_ExtraFee = new SqlCommand("SP_Accommodations", con);
                _cmd_ExtraFee.CommandType = CommandType.StoredProcedure;
                _cmd_ExtraFee.Parameters.AddWithValue("@ProcedureType", "G");
                _cmd_ExtraFee.Parameters.AddWithValue("@SessionType", "110");
                _cmd_ExtraFee.Parameters.AddWithValue("@p_UserTypeId", Session["UserTypeId"]);
                SqlParameter _results_ExtraFee = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                _results_ExtraFee.Direction = ParameterDirection.Output;
                _cmd_ExtraFee.Parameters.Add(_results_ExtraFee);

                SqlDataAdapter _sda_ExtraFee = new SqlDataAdapter(_cmd_ExtraFee);

                _cmd_ExtraFee.ExecuteNonQuery();
                _status_ExtraFee = _results_ExtraFee.Value.ToString();
                _sda_ExtraFee.Fill(_ds_ExtraFee);
                //
                DataSet[] _ds_Array = new DataSet[] { _ds_GABId, _ds_Currency, _ds_ExtraFee };
                con.Close();

                return View(_ds_Array);
            }
            catch (Exception ex)
            {
             TempData["Error"] = "System Error : Contact Technical Support";
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public ActionResult UpdateAccommodations(_UpdateAccommodation _update_AC)// Edit_Accommodations
        {
            var _PHPAPI = "";
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["AccommodationId"] == null || Session["AccommodationId"] == "")
            {
                TempData["Error"] = "System Error : Accommodation Not Found !";
                return RedirectToAction("AddNewAccommodations", "Home");
            }
            try
            {
                if(_update_AC._webSearch==false)
                {

                    Session["WebSearch"] = 0;
                }
                else
                { 
                    Session["WebSearch"] = 1;
                }
                string _status = "";
                DataSet _ds_update_AC = new DataSet();
                SqlCommand _cmd_update_AC = new SqlCommand("SP_Accommodations", con);
                _cmd_update_AC.CommandType = CommandType.StoredProcedure;
                _cmd_update_AC.Parameters.AddWithValue("@ProcedureType", "M");
                _cmd_update_AC.Parameters.AddWithValue("@SessionType", "1");
                _cmd_update_AC.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                _cmd_update_AC.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                _cmd_update_AC.Parameters.AddWithValue("@p_CurrencyId", _update_AC._currencyId);
                _cmd_update_AC.Parameters.AddWithValue("@p_ExtraFeeTypeId", _update_AC._extraFeeTypeId);
                _cmd_update_AC.Parameters.AddWithValue("@p_PropertyCategory", _update_AC._propertyCategory); 
                _cmd_update_AC.Parameters.AddWithValue("@p_contactNo", _update_AC._contactNo);
                _cmd_update_AC.Parameters.AddWithValue("@p_EmailId", _update_AC._emailId);
                _cmd_update_AC.Parameters.AddWithValue("@p_PostCode", _update_AC._postCode);
                _cmd_update_AC.Parameters.AddWithValue("@p_Latitude", _update_AC._latitude);
                _cmd_update_AC.Parameters.AddWithValue("@p_Longitude", _update_AC._longitude);
                _cmd_update_AC.Parameters.AddWithValue("@p_WebSearch", Session["WebSearch"]);
                _cmd_update_AC.Parameters.AddWithValue("@p_HomeShopping", _update_AC._HomeShopping);
                SqlParameter _results_update_AC = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                _results_update_AC.Direction = ParameterDirection.Output;
                _cmd_update_AC.Parameters.Add(_results_update_AC);
                 
                con.Open();
                _cmd_update_AC.ExecuteNonQuery();
                _status = _results_update_AC.Value.ToString(); 
                if (_status == ""|| _status== "Failure")
                {
                    TempData["Error"] = "Server Error : Contact Technical Support";
                }
                else
                {
                   
                    if (Session["WebSearch"] != null)
                    {
                        if (int.Parse(Session["WebSearch"].ToString()) == 1)
                        {
                            HttpWebRequest request = WebRequest.Create("http://roomph.pk/admin/home/fetch-accommodations/" + Session["AccommodationId"]) as HttpWebRequest;
                            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                            _PHPAPI= response.StatusDescription;
                        }
                    }
                    TempData["okey"] = "Accommodation Update Successfully!";
                }
                con.Close(); 
                return RedirectToAction("UpdateAccommodations", "Home");
            }
            catch (Exception ex)
            {
             TempData["Error"] = "System Error : Contact Technical Support";
                TempData["ErrorPage"] = "Update Accommodation"+ _PHPAPI;
                
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult GetAllUsers()//Get All Users Assigning Unassigning Accommodations 
        {
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            { 
                string _status = "";
                DataSet _ds_GAUser = new DataSet(); // Get All User
                SqlCommand _cmd_GAUser = new SqlCommand("SP_Admin", con);
                _cmd_GAUser.CommandType = CommandType.StoredProcedure;
                _cmd_GAUser.Parameters.AddWithValue("@ProcedureType", "G");
                _cmd_GAUser.Parameters.AddWithValue("@SessionType", "3");
                _cmd_GAUser.Parameters.AddWithValue("@p_UserTypeId", Session["UserTypeId"]);
                SqlParameter _results_GAUser = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                _results_GAUser.Direction = ParameterDirection.Output;
                _cmd_GAUser.Parameters.Add(_results_GAUser);

                SqlDataAdapter _sda_GAUser = new SqlDataAdapter(_cmd_GAUser);
                con.Open();
                _cmd_GAUser.ExecuteNonQuery();
                _status = _results_GAUser.Value.ToString();
                var _set = _sda_GAUser.Fill(_ds_GAUser);
                return View(_ds_GAUser);
            }
            catch (Exception _ex)
            {
                con.Close();
                TempData["Error"] = "System Error : Contact Technical Support"+_ex;
                TempData["ErrorPage"] = "GetAllUsers Home";
                
                return RedirectToAction("Error", "Home");
            }
        }
        //public ActionResult UnMappingAccommodations(int A_UserId)//UnMappingAccommodations
        //{
        //    if (Session["LoginType"] == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    Session["Muid"] = A_UserId;
        //    try
        //    {
        //        string Status6 = "";
        //        DataSet ds6 = new DataSet();
        //        SqlCommand cmd6 = new SqlCommand("SP_Accommodations_Test", con);
        //        cmd6.CommandType = CommandType.StoredProcedure;
        //        cmd6.Parameters.AddWithValue("@ProcedureType", "G");
        //        cmd6.Parameters.AddWithValue("@SessionType", "201");
        //        cmd6.Parameters.AddWithValue("@p_MultiLanguageId", "1");
        //        cmd6.Parameters.AddWithValue("@p_UserId", A_UserId);
        //        SqlParameter _results6 = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
        //        _results6.Direction = ParameterDirection.Output;
        //        cmd6.Parameters.Add(_results6);

        //        SqlDataAdapter adp6 = new SqlDataAdapter(cmd6);
        //        con.Open();
        //        cmd6.ExecuteNonQuery();
        //        Status6 = _results6.Value.ToString();
        //        adp6.Fill(ds6);
        //        //SqlDataReader dr = cmd6.ExecuteReader();
        //        //if (dr.HasRows)
        //        //{
        //        //    while (dr.Read())
        //        //    {
        //        //        Session["CurrentUser"] = dr["UserName"];
        //        //    }
        //        //}
        //        //var a=Session["CurrentUser"];
        //        con.Close();
        //        return View(ds6);
        //    }
        //    catch (Exception ex)
        //    {

        //     TempData["Error"] = "System Error : Contact Technical Support";
        //        return RedirectToAction("Error", "Home");
        //    } 
        //}
        //public ActionResult AddNewAccommodations()    //All Map Accommodation to Current User
        //{
        //    if (Session["LoginType"] == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    try
        //    {


        //        string Status6 = "";
        //        DataSet ds6 = new DataSet();
        //        SqlCommand cmd6 = new SqlCommand("SP_Accommodations_Test", con);
        //        cmd6.CommandType = CommandType.StoredProcedure;
        //        cmd6.Parameters.AddWithValue("@ProcedureType", "G");
        //        cmd6.Parameters.AddWithValue("@SessionType", "202");
        //        cmd6.Parameters.AddWithValue("@p_UserId", Session["UserId"]);
        //        cmd6.Parameters.AddWithValue("@p_MultiLanguageId", "1");
        //        SqlParameter _results6 = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
        //        _results6.Direction = ParameterDirection.Output;
        //        cmd6.Parameters.Add(_results6);

        //        SqlDataAdapter adp6 = new SqlDataAdapter(cmd6);
        //        con.Open();
        //        cmd6.ExecuteNonQuery();
        //        Status6 = _results6.Value.ToString();
        //        adp6.Fill(ds6);
        //        con.Close();
        //        return View(ds6);
        //    }
        //    catch (Exception ex)
        //    { 
        //     TempData["Error"] = "System Error : Contact Technical Support";
        //        return RedirectToAction("Error", "Home");
        //    } 
        //}
        public ActionResult AllMappingAccommodations(int _userId)//All Mapping Accommodations
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {  
                string _status_GUBId = "";// Get User By Id
                DataSet _ds_GUBId = new DataSet();
                SqlCommand _cmd_GUBId = new SqlCommand("SP_Accommodations", con);
                _cmd_GUBId.CommandType = CommandType.StoredProcedure;
                _cmd_GUBId.Parameters.AddWithValue("@ProcedureType", "G");
                _cmd_GUBId.Parameters.AddWithValue("@SessionType", "202");
                _cmd_GUBId.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                _cmd_GUBId.Parameters.AddWithValue("@p_UserId", _userId);
                SqlParameter _results_GUBId = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                _results_GUBId.Direction = ParameterDirection.Output;
                _cmd_GUBId.Parameters.Add(_results_GUBId);

                SqlDataAdapter _sda_GUBId = new SqlDataAdapter(_cmd_GUBId);
                con.Open();
                _cmd_GUBId.ExecuteNonQuery();
                _status_GUBId = _results_GUBId.Value.ToString();
                _sda_GUBId.Fill(_ds_GUBId);
                con.Close();
                return View(_ds_GUBId);
            }
            catch (Exception ex)
            {
             TempData["Error"] = "System Error : Contact Technical Support";

                TempData["ErrorPage"] = "AllMappingAccommodations Home";
                return RedirectToAction("Error", "Home");
            }
        }
        public string Load_Accommodation(string _userId)
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            string Status6 = "";
            DataTable dt = new DataTable();
            SqlCommand cmd6 = new SqlCommand("SP_Accommodations", con);
            cmd6.CommandType = CommandType.StoredProcedure;
            cmd6.Parameters.AddWithValue("@ProcedureType", "G");
            cmd6.Parameters.AddWithValue("@SessionType", "201");
            cmd6.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
            cmd6.Parameters.AddWithValue("@p_UserId", Session["UserId"]);
            cmd6.Parameters.AddWithValue("@p_Assign_UserId", _userId);
            SqlDataAdapter da3 = new SqlDataAdapter(cmd6);
            var set = da3.Fill(dt);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }// Find Accommodation User Id Wise --  Uid Same is UserId
        public ActionResult Add_MapAccommodation(string _userId, string[] _accommodationId)
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            try
            { 
                int x = _accommodationId.Count() - 1;
                for (int i = 0; i <= x; i++)
                {

                    string _status_MappedAC = "";

                    DataSet _ds_MappedAC = new DataSet();
                    SqlCommand _cmd_MappedAC = new SqlCommand("SP_Accommodations", con);
                    _cmd_MappedAC.CommandType = CommandType.StoredProcedure;
                    _cmd_MappedAC.Parameters.AddWithValue("@ProcedureType", "I");
                    _cmd_MappedAC.Parameters.AddWithValue("@SessionType", "201");
                    _cmd_MappedAC.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    _cmd_MappedAC.Parameters.AddWithValue("@p_OwnerId", Session["OwnerId"]);
                    _cmd_MappedAC.Parameters.AddWithValue("@p_UserId", _userId);
                    _cmd_MappedAC.Parameters.AddWithValue("@p_AccommodationId", _accommodationId[i]);
                    SqlParameter _results_MappedAC = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_MappedAC.Direction = ParameterDirection.Output;
                    _cmd_MappedAC.Parameters.Add(_results_MappedAC);
                     
                    con.Open();
                    _cmd_MappedAC.ExecuteNonQuery();
                    con.Close();
                    _status_MappedAC = _results_MappedAC.Value.ToString(); 
                    if (_status_MappedAC == "")
                    {
                        TempData["Error"] = "Server Error : Contact Technical Support";
                    }
                    else
                    {

                        TempData["okey"] = "Accommodation Mapped Successfully!";
                    }
                   
                }

                return RedirectToAction("AllMappingAccommodations", "Home", new { @_userId = _userId });
            }
            catch (Exception ex)
            {
             TempData["Error"] = "System Error : Contact Technical Support";

                TempData["ErrorPage"] = "Add_MapAccommodation Home";
            }
            return View();
        } // Map hotel to User
        public ActionResult RemoveMappingAccommodations(int _userId, int _accommodationId)//Removing Mapping hotel
        { 
            try
            {
                //Session["dtRemoveMappingAccommodations"]
                string Status6 = "";
                DataSet ds6 = new DataSet();
                SqlCommand cmd6 = new SqlCommand("SP_Accommodations", con);
                cmd6.CommandType = CommandType.StoredProcedure;
                cmd6.Parameters.AddWithValue("@ProcedureType", "D");
                cmd6.Parameters.AddWithValue("@SessionType", "201");
                cmd6.Parameters.AddWithValue("@p_UserId", _userId);
                cmd6.Parameters.AddWithValue("@p_AccommodationId", _accommodationId);
                SqlParameter _results6 = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                _results6.Direction = ParameterDirection.Output;
                cmd6.Parameters.Add(_results6);
                 
                con.Open();
                cmd6.ExecuteNonQuery();
                Status6 = _results6.Value.ToString(); 
                con.Close();

                TempData["okey"] = "Accommodation Assign Removed Successfully!";

                return RedirectToAction("AllMappingAccommodations", "Home", new { @_userId = _userId });
            }

            catch (Exception ex)
            {
             TempData["Error"] = "System Error : Contact Technical Support";
                TempData["ErrorPage"] = "RemoveMappingAccommodations"; 
                return RedirectToAction("AllMappingAccommodations", "Home", new { @_userId = _userId });
            }

        }
        //public JsonResult SreachAccommodation(int AccmommdationId)
        //{
        //    string AccmommdationName = "";

        //    string Status6 = "";
        //        DataSet ds6 = new DataSet();
        //        SqlCommand cmd6 = new SqlCommand("SP_Accommodations_Test", con);
        //        cmd6.CommandType = CommandType.StoredProcedure;
        //        cmd6.Parameters.AddWithValue("@ProcedureType", "G");
        //        cmd6.Parameters.AddWithValue("@SessionType", "202");
        //        cmd6.Parameters.AddWithValue("@p_MultiLanguageId", "1");
        //        cmd6.Parameters.AddWithValue("@p_UserId", Session["UserId"]);
        //        cmd6.Parameters.AddWithValue("@p_AccommodationId", AccmommdationId);
        //        SqlParameter _results6 = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
        //        _results6.Direction = ParameterDirection.Output;
        //        cmd6.Parameters.Add(_results6);

        //        SqlDataAdapter adp6 = new SqlDataAdapter(cmd6);
        //        con.Open();
        //        cmd6.ExecuteNonQuery();
        //        Status6 = _results6.Value.ToString();
        //        adp6.Fill(ds6);
        //        con.Close();
        //    con.Open();
        //    SqlDataReader dr = cmd6.ExecuteReader();
        //    if (dr.HasRows)
        //    {
        //        while (dr.Read())
        //        {
        //           // AccmommdationId = dr["AccommodationId"];
        //           AccmommdationName = dr["AccommodationName"].ToString();
        //        }
        //    }
        //    con.Close();
        //    var ass = Session["OwnerId"];
        //    return Json(AccmommdationName, JsonRequestBehavior.AllowGet);
        //} 
        //public ActionResult MappingAccommodations(int uid,string AccommodationId)//Insert MappingAccommodations
        //{
        //    if (Session["LoginType"] == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    try
        //    {      // Session["dtAsginAccommodation"] 
        //        string Status6 = "";

        //        DataSet ds6 = new DataSet();
        //        SqlCommand cmd6 = new SqlCommand("SP_Accommodations_Test", con);
        //        cmd6.CommandType = CommandType.StoredProcedure;
        //        cmd6.Parameters.AddWithValue("@ProcedureType", "I");
        //        cmd6.Parameters.AddWithValue("@SessionType", "201");
        //        cmd6.Parameters.AddWithValue("@p_MultiLanguageId", "1");
        //        cmd6.Parameters.AddWithValue("@p_OwnerId", Session["OwnerId"]);
        //        cmd6.Parameters.AddWithValue("@p_UserId", uid);
        //        cmd6.Parameters.AddWithValue("@p_AccommodationId", AccommodationId);
        //        SqlParameter _results6 = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
        //        _results6.Direction = ParameterDirection.Output;
        //        cmd6.Parameters.Add(_results6);

        //        SqlDataAdapter adp6 = new SqlDataAdapter(cmd6);
        //        con.Open();
        //        cmd6.ExecuteNonQuery();
        //        Status6 = _results6.Value.ToString();
        //        adp6.Fill(ds6);
        //        if (Status6 == "")
        //        {
        //            TempData["Error"] = "Server Error : Contact Technical Support";
        //        }
        //        else {

        //            TempData["okey"] = "Map Accommodations";
        //        }
        //        con.Close();
        //        return RedirectToAction("AllMappingAccommodations", "Home", new { @uid = uid });
        //    }


        //    catch (Exception ex)
        //    {
        //     TempData["Error"] = "System Error : Contact Technical Support";
        //        return RedirectToAction("Error", "Home");
        //    }

        //} 
        //public ActionResult RemoveMappingAccommodations(int UserId, int AccommodationId)//Removing Mapping Accommodations
        //{
        //    var uid = UserId;
        //    if (Session["LoginType"] == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    try
        //    {
        //        //Session["dtRemoveMappingAccommodations"]
        //        string Status6 = "";
        //        DataSet ds6 = new DataSet();
        //        SqlCommand cmd6 = new SqlCommand("SP_Accommodations_Test", con);
        //        cmd6.CommandType = CommandType.StoredProcedure;
        //        cmd6.Parameters.AddWithValue("@ProcedureType", "D");
        //        cmd6.Parameters.AddWithValue("@SessionType", "201");
        //        cmd6.Parameters.AddWithValue("@p_UserId", uid);
        //        cmd6.Parameters.AddWithValue("@p_AccommodationId", AccommodationId);
        //        SqlParameter _results6 = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
        //        _results6.Direction = ParameterDirection.Output;
        //        cmd6.Parameters.Add(_results6);

        //        SqlDataAdapter adp6 = new SqlDataAdapter(cmd6);
        //        con.Open();
        //        cmd6.ExecuteNonQuery();
        //        Status6 = _results6.Value.ToString();
        //        adp6.Fill(ds6);
        //        con.Close();

        //        TempData["okey"] = "Removed Assign Accommodation";

        //        return RedirectToAction("AllMappingAccommodations", "Home", new { @uid = uid });
        //    }

        //    catch (Exception ex)
        //    {
        //     TempData["Error"] = "System Error : Contact Technical Support";
        //        return RedirectToAction("AllMappingAccommodations", "Home", new { @uid = uid });
        //    }

        //}



        //public JsonResult AllAssignAccommodations(int UserId)//Insert All MappingAccommodations
        //{
        //    string result = ""; 
        //    var uid = UserId;
        //    if (Session["LoginType"] == null)
        //    {
        //        return null;
        //    }
        //    try
        //    {far
         
        //        string Status6 = "";

        //        DataSet ds6 = new DataSet();
        //        SqlCommand cmd6 = new SqlCommand("SP_Accommodations_Test", con);
        //        cmd6.CommandType = CommandType.StoredProcedure;
        //        cmd6.Parameters.AddWithValue("@ProcedureType", "I");
        //        cmd6.Parameters.AddWithValue("@SessionType", "202");
        //        cmd6.Parameters.AddWithValue("@p_MultiLanguageId", "1");
        //        cmd6.Parameters.AddWithValue("@p_Assign_UserId", uid);
        //        cmd6.Parameters.AddWithValue("@p_OwnerId", Session["OwnerId"]);
        //        cmd6.Parameters.AddWithValue("@p_UserId", Session["OwnerId"]);
        //        SqlParameter _results6 = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
        //        _results6.Direction = ParameterDirection.Output;
        //        cmd6.Parameters.Add(_results6);

        //        SqlDataAdapter adp6 = new SqlDataAdapter(cmd6);
        //        con.Open();
        //        cmd6.ExecuteNonQuery();
        //        Status6 = _results6.Value.ToString();
        //        adp6.Fill(ds6);
        //        con.Close();
        //        TempData["new"] = "Assigned All Accommodations";
        //        result = "Assigned All Accommodations";

        //        return Json(JsonRequestBehavior.AllowGet, result);
        //    }


        //    catch (Exception ex)
        //    {
        //     TempData["Error"] = "System Error : Contact Technical Support"; 
        //        return Json(JsonRequestBehavior.AllowGet, result);
        //    }
        //   // return View();
        //    //return RedirectToAction("AllMappingAccommodations", "Home", new { @uid = uid });
        //}
        public ActionResult SelectMappedAccommodations()//Drop Down Fill/ Partial View
        {
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var LanguageId = Session["LanguageId"];// Session["LanguageId"];
               
                    string _status = "";
                    DataSet _ds_GADDL = new DataSet(); // Get All User
                    SqlCommand _cmd_GADDL = new SqlCommand("SP_Accommodations", con);
                    _cmd_GADDL.CommandType = CommandType.StoredProcedure;
                    _cmd_GADDL.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_GADDL.Parameters.AddWithValue("@SessionType", "202");
                    _cmd_GADDL.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    _cmd_GADDL.Parameters.AddWithValue("@p_UserId", Session["UserId"]);
                    SqlParameter _results_GAUser = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_GAUser.Direction = ParameterDirection.Output;
                    _cmd_GADDL.Parameters.Add(_results_GAUser);

                    SqlDataAdapter _sda_GAUser = new SqlDataAdapter(_cmd_GADDL);
                    con.Open();
                    _cmd_GADDL.ExecuteNonQuery();
                    _status = _results_GAUser.Value.ToString();
                    var _set = _sda_GAUser.Fill(_ds_GADDL);
                    con.Close();
                    return PartialView(_ds_GADDL);
          
            }
            catch (Exception ex)
            {
             TempData["Error"] = "System Error : Contact Technical Support";
                return RedirectToAction("Error", "Home");
            }
        }
        DataTable SaveDt;
        public string GetDDLhotels()
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            SaveDt = (DataTable)Session["SaveDt"];
            if (SaveDt != null && SaveDt.Rows.Count > 0)
            {
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in SaveDt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in SaveDt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                return serializer.Serialize(rows);
            }
            else
            {
                string Status6 = "";
                DataTable dt = new DataTable();
                SqlCommand cmd6 = new SqlCommand("SP_Accommodations", con);
                cmd6.CommandType = CommandType.StoredProcedure;
                cmd6.Parameters.AddWithValue("@ProcedureType", "G");
                cmd6.Parameters.AddWithValue("@SessionType", "202");
                cmd6.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                cmd6.Parameters.AddWithValue("@p_UserId", Session["UserId"]);
                SqlParameter _results6 = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                _results6.Direction = ParameterDirection.Output;
                cmd6.Parameters.Add(_results6);

                SqlDataAdapter adp6 = new SqlDataAdapter(cmd6);
                adp6.Fill(dt);
                Session["SaveDt"] = dt;
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }

                return serializer.Serialize(rows);
            }

        }
        public ActionResult ViewSelectAddNewAccommodations(int _accommodationId = 0)//Select Accommodation Is view//
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (_accommodationId == null || _accommodationId == 0)
            {
                return RedirectToAction("AddNewAccommodations");
            }
            else
            {

                if (Session["LoginType"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                try
                {
                    var UserTypeId = Session["UserTypeId"];
                    var UserId = Session["UserId"];
                    string _status_SelectAC = "";
                    SqlCommand _cmd_SelectAC = new SqlCommand("SP_Accommodations", con);
                    _cmd_SelectAC.CommandType = CommandType.StoredProcedure;
                    _cmd_SelectAC.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_SelectAC.Parameters.AddWithValue("@SessionType", "1");
                    _cmd_SelectAC.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    _cmd_SelectAC.Parameters.AddWithValue("@p_AccommodationId", _accommodationId);
                    _cmd_SelectAC.Parameters.AddWithValue("@p_UserTypeId", Session["UserTypeId"]);
                    _cmd_SelectAC.Parameters.AddWithValue("@p_UserId", Session["UserId"]);
                    SqlParameter _results_SelectAC = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_SelectAC.Direction = ParameterDirection.Output;
                    _cmd_SelectAC.Parameters.Add(_results_SelectAC);

                    SqlDataAdapter _sda_SelectAC = new SqlDataAdapter(_cmd_SelectAC);
                    con.Open();
                    _cmd_SelectAC.ExecuteNonQuery();
                    _status_SelectAC = _results_SelectAC.Value.ToString();
                    DataSet dt = new DataSet();
                    _sda_SelectAC.Fill(dt);
                    con.Close(); 

                    con.Open();
                    SqlDataReader _sdr_SelectAC = _cmd_SelectAC.ExecuteReader(); 
                    if (_sdr_SelectAC.HasRows)
                    {
                        while (_sdr_SelectAC.Read())
                        {
                            Session["AccommodationId"] = _sdr_SelectAC["AccommodationId"];
                            Session["AccommodationName"] = _sdr_SelectAC["AccommodationName"];
                        }
                    }
                    con.Close();
                    //return View();
                    var asdfsdf = Session["AccommodationId"];
                    var asdsd = Session["AccommodationName"];
                    return Redirect(Request.UrlReferrer.ToString());
                    //return Redirect(Request.UrlReferrer.ToString());
                }
                catch (Exception ex)
                {
                 TempData["Error"] = "System Error : Contact Technical Support";

                    TempData["ErrorPage"] = "ViewSelectAddNewAccommodations Home";
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        //  ---                         ------------------------------------------   //
        public ActionResult AccommodationDetails()// Edit_Accommodations
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["AccommodationId"] == null || Session["AccommodationId"].ToString() == "")
            {
                TempData["Error"] = "System Error : Accommodation Not Found !";
                return RedirectToAction("AddNewAccommodations", "Home");
            }
            else
            {
                try
                {
                    string _status = "";
                    DataSet _ds_ACDetails = new DataSet();
                    SqlCommand _cmd_ACDetails = new SqlCommand("SP_Accommodations", con);
                    _cmd_ACDetails.CommandType = CommandType.StoredProcedure;
                    _cmd_ACDetails.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_ACDetails.Parameters.AddWithValue("@SessionType", "1");
                    _cmd_ACDetails.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    _cmd_ACDetails.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                    _cmd_ACDetails.Parameters.AddWithValue("@p_UserTypeId", Session["UserTypeId"]);
                    _cmd_ACDetails.Parameters.AddWithValue("@p_UserId", Session["UserId"]);
                    SqlParameter _results_ACDetails = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_ACDetails.Direction = ParameterDirection.Output;
                    _cmd_ACDetails.Parameters.Add(_results_ACDetails);

                    SqlDataAdapter _sda_ACDetails = new SqlDataAdapter(_cmd_ACDetails);
                    con.Open();
                    _cmd_ACDetails.ExecuteNonQuery();
                    _status = _results_ACDetails.Value.ToString();
                    _sda_ACDetails.Fill(_ds_ACDetails);
                    /// Booking Media
                    string _status_BookingMedia = "";
                    DataSet _ds_BookingMedia = new DataSet();
                    SqlCommand _cmd_BookingMedia = new SqlCommand("SP_Accommodations", con);
                    _cmd_BookingMedia.CommandType = CommandType.StoredProcedure;
                    _cmd_BookingMedia.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_BookingMedia.Parameters.AddWithValue("@SessionType", "111");
                    SqlParameter _results_BookingMedia = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_BookingMedia.Direction = ParameterDirection.Output;
                    _cmd_BookingMedia.Parameters.Add(_results_BookingMedia);

                    SqlDataAdapter _sda_BookingMedia = new SqlDataAdapter(_cmd_BookingMedia);
                    _cmd_BookingMedia.ExecuteNonQuery();
                    _status_BookingMedia = _results_BookingMedia.Value.ToString();
                    _sda_BookingMedia.Fill(_ds_BookingMedia);

                    //Country
                    string _status_Country = "";
                    DataSet _ds_Country = new DataSet();
                    SqlCommand _cmd_Country = new SqlCommand("SP_Accommodations", con);
                    _cmd_Country.CommandType = CommandType.StoredProcedure;
                    _cmd_Country.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_Country.Parameters.AddWithValue("@SessionType", "103");
                    _cmd_Country.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    SqlParameter _results_Country = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_Country.Direction = ParameterDirection.Output;
                    _cmd_Country.Parameters.Add(_results_Country);

                    SqlDataAdapter _sda_Country = new SqlDataAdapter(_cmd_Country);
                    _cmd_Country.ExecuteNonQuery();
                    _status_Country = _results_Country.Value.ToString();
                    _sda_Country.Fill(_ds_Country);
                    //
                    //Accommodation Type
                    string _status_AccommodationType = "";
                    DataSet _ds_AccommodationType = new DataSet();
                    SqlCommand _cmd_AccommodationType = new SqlCommand("SP_Accommodations", con);
                    _cmd_AccommodationType.CommandType = CommandType.StoredProcedure;
                    _cmd_AccommodationType.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_AccommodationType.Parameters.AddWithValue("@SessionType", "112");
                    _cmd_AccommodationType.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    SqlParameter _results_AccommodationType = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_AccommodationType.Direction = ParameterDirection.Output;
                    _cmd_AccommodationType.Parameters.Add(_results_AccommodationType);

                    SqlDataAdapter _sda_AccommodationType = new SqlDataAdapter(_cmd_AccommodationType);
                    _cmd_AccommodationType.ExecuteNonQuery();
                    _status_AccommodationType = _results_AccommodationType.Value.ToString();
                    _sda_AccommodationType.Fill(_ds_AccommodationType);
                    //
                    con.Close();
                    DataSet[] _ds_Array = new DataSet[] { _ds_ACDetails, _ds_BookingMedia, _ds_Country, _ds_AccommodationType };
                    return View(_ds_Array);
                }
                catch (Exception ex)
                {
                 TempData["Error"] = "System Error : Contact Technical Support";

                    TempData["ErrorPage"] = "AccommodationDetails Home";
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        [HttpPost]
        public ActionResult AccommodationDetails(HttpPostedFileBase Files, _AccommodationDetails _AC_Details)// Edit_Accommodation's
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["AccommodationId"] == null || Session["AccommodationId"].ToString() == "")
            {
                TempData["Error"] = "System Error : Accommodation Not Found !";
                return RedirectToAction("AddNewAccommodations", "Home");
            }
            else
            {
                try
                { 
                    string dbpath = null; 
                    if (Files != null)
                    {
                        var fileName = Path.GetFileName(Session["AccommodationId"] + ".jpeg");

                        var path = Path.Combine(Server.MapPath("../Image/"), fileName);
                        Files.SaveAs(path);
                        dbpath = "/Image/" + fileName;
                        string filepathtosave = path + fileName;
                    }

                    string _status_AC_Details = "";
                    DataSet _ds_AC_Details = new DataSet();
                    SqlCommand _cmd_AC_Details = new SqlCommand("SP_Accommodations", con);
                    _cmd_AC_Details.CommandType = CommandType.StoredProcedure;
                    _cmd_AC_Details.Parameters.AddWithValue("@ProcedureType", "M");
                    _cmd_AC_Details.Parameters.AddWithValue("@SessionType", "1");
                    _cmd_AC_Details.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    _cmd_AC_Details.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]); 
                    _cmd_AC_Details.Parameters.AddWithValue("@p_AccommodationTypeId", _AC_Details._accommodationTypeId);
                    _cmd_AC_Details.Parameters.AddWithValue("@p_Rating", _AC_Details._rating);
                    _cmd_AC_Details.Parameters.AddWithValue("@p_Website", _AC_Details._website);
                    _cmd_AC_Details.Parameters.AddWithValue("@p_EmailId", _AC_Details._emailId);
                    _cmd_AC_Details.Parameters.AddWithValue("@p_TotalRooms", _AC_Details._totalRooms);
                    _cmd_AC_Details.Parameters.AddWithValue("@p_contactNo", _AC_Details._contactNo);
                    _cmd_AC_Details.Parameters.AddWithValue("@p_FaxNo", _AC_Details._faxNo);
                    _cmd_AC_Details.Parameters.AddWithValue("@p_Address", _AC_Details._address);
                    _cmd_AC_Details.Parameters.AddWithValue("@p_BookingMediaId", _AC_Details._bookingMediaId);
                    _cmd_AC_Details.Parameters.AddWithValue("@p_AddressId", _AC_Details._addressId); 
                    _cmd_AC_Details.Parameters.AddWithValue("@p_BookingMediaEmail", _AC_Details._bookingMediaEmail);
                    _cmd_AC_Details.Parameters.AddWithValue("@p_BookingMediaFax", _AC_Details._bookingMediaFax);
                    _cmd_AC_Details.Parameters.AddWithValue("@p_GeneralDescription", _AC_Details._generalDescription);
                    _cmd_AC_Details.Parameters.AddWithValue("@p_AreaOfInterest", _AC_Details._areaOfInterest);  
                    _cmd_AC_Details.Parameters.AddWithValue("@p_AccommdationLogo", dbpath);

                    SqlParameter _results_AC_Details = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_AC_Details.Direction = ParameterDirection.Output;
                    _cmd_AC_Details.Parameters.Add(_results_AC_Details);
                     
                    con.Open();
                    _cmd_AC_Details.ExecuteNonQuery();
                    _status_AC_Details = _results_AC_Details.Value.ToString(); 
                    if (_status_AC_Details == "" || _status_AC_Details == "Failure")
                    {
                        TempData["Error"] = "Server Error : Contact Technical Support";
                    }
                    else
                    {
                        TempData["okey"] = "Accommodation Update Successfully!"; 
                        if(Session["WebSearch"]!=null)
                        {
                            if (int.Parse(Session["WebSearch"].ToString()) == 1)
                            {
                                HttpWebRequest request = WebRequest.Create("http://roomph.pk/admin/home/fetch-accommodations/" + Session["AccommodationId"]) as HttpWebRequest;
                                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                                Stream stream = response.GetResponseStream();
                            }
                        }
                       
                    }
                    con.Close();

                    return RedirectToAction("AccommodationDetails", "Home");
                }
                catch (Exception ex)
                {
                 TempData["Error"] = "System Error : Contact Technical Support";

                    TempData["ErrorPage"] = "Update AccommodationDetails Home";
                    return RedirectToAction("Error", "Home");

                }
            }
        }
        public ActionResult CreateWebsite()
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            var _accommodationId = Session["AccommodationId"];
            string _Domain = null;
            string _status = null;

            string _url = @"http://b2bhotels.net/cms_update/index.php?id=" + _accommodationId + "&createdomain=1&templateid=" + "123" + "&primarycolor=" + "123" + "&secondarycolor=" + "123" + "&fontcolor=" + "123";


            string text = new WebClient().DownloadString(_url);
            var wclients = JsonConvert.DeserializeObject<dynamic>(text);

            var _data = wclients.domain_name;
            _Domain = _data;
            var _WebLink = "http://www." + _Domain;
            DataSet _ds_Addwebsite = new DataSet();
            SqlCommand _cmd_Addwebsite = new SqlCommand("SP_Accommodations", con);
            _cmd_Addwebsite.CommandType = CommandType.StoredProcedure;
            _cmd_Addwebsite.Parameters.AddWithValue("@ProcedureType", "M");
            _cmd_Addwebsite.Parameters.AddWithValue("@SessionType", "1");
            _cmd_Addwebsite.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
            _cmd_Addwebsite.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);

            _cmd_Addwebsite.Parameters.AddWithValue("@p_Website", _WebLink);
            SqlParameter _results_Addwebsite = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
            _results_Addwebsite.Direction = ParameterDirection.Output;
            _cmd_Addwebsite.Parameters.Add(_results_Addwebsite);

            con.Open();
            _cmd_Addwebsite.ExecuteNonQuery();
            _status = _results_Addwebsite.Value.ToString();
            if (_status == "")
            {
                TempData["Error"] = "Server Error : Contact Technical Support";
            }
            else
            {
                TempData["okey"] = "Website Create to Successfully!";
            }
            con.Close();
            return RedirectToAction("AccommodationDetails", "Home");
        }
        public ActionResult AdditionalDetails()
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["AccommodationId"] == null || Session["AccommodationId"] == "")
            {
                TempData["Error"] = "System Error : Accommodation Not Found !";
                return RedirectToAction("AddNewAccommodations", "Home");
            }
            else
            {
                try
                {
                    string _status = "";
                    DataSet _ds_AdditionalDetails = new DataSet();
                    SqlCommand cmd_AdditionalDetails = new SqlCommand("SP_Accommodations", con);
                    cmd_AdditionalDetails.CommandType = CommandType.StoredProcedure;
                    cmd_AdditionalDetails.Parameters.AddWithValue("@ProcedureType", "G");
                    cmd_AdditionalDetails.Parameters.AddWithValue("@SessionType", "1");
                    cmd_AdditionalDetails.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    cmd_AdditionalDetails.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                    cmd_AdditionalDetails.Parameters.AddWithValue("@p_UserTypeId", Session["UserTypeId"]);
                    cmd_AdditionalDetails.Parameters.AddWithValue("@p_UserId", Session["UserId"]);
                    SqlParameter _results_AdditionalDetails = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_AdditionalDetails.Direction = ParameterDirection.Output;
                    cmd_AdditionalDetails.Parameters.Add(_results_AdditionalDetails);

                    SqlDataAdapter _sda_AdditionalDetails = new SqlDataAdapter(cmd_AdditionalDetails);
                    con.Open();
                    cmd_AdditionalDetails.ExecuteNonQuery();
                    _status = _results_AdditionalDetails.Value.ToString();
                    _sda_AdditionalDetails.Fill(_ds_AdditionalDetails);

                    return View(_ds_AdditionalDetails);
                }
                catch (Exception ex)
                {
                 TempData["Error"] = "System Error : Contact Technical Support";
                    TempData["ErrorPage"] = "Additional Details";
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        [HttpPost]
        public ActionResult AdditionalDetails(_AdditionalDetails _Addit_Details)// Edit_Accommodation's
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["AccommodationId"] == null || Session["AccommodationId"] == "")
            {
                TempData["Error"] = "System Error : Accommodation Not Found !";
                return RedirectToAction("AddNewAccommodations", "Home");
            }
            else
            {
                try
                {

                    string _status = "";
                    DataSet _ds_AdditionalDetail = new DataSet();
                    SqlCommand _cmd_AdditionalDetail = new SqlCommand("SP_Accommodations", con);
                    _cmd_AdditionalDetail.CommandType = CommandType.StoredProcedure;
                    _cmd_AdditionalDetail.Parameters.AddWithValue("@ProcedureType", "M");
                    _cmd_AdditionalDetail.Parameters.AddWithValue("@SessionType", "1");
                    _cmd_AdditionalDetail.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    _cmd_AdditionalDetail.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                    _cmd_AdditionalDetail.Parameters.AddWithValue("@p_CheckInFrom", _Addit_Details._checkInFrom);
                    _cmd_AdditionalDetail.Parameters.AddWithValue("@p_CheckOutFrom", _Addit_Details._checkOutFrom);
                    _cmd_AdditionalDetail.Parameters.AddWithValue("@p_CheckInTo", _Addit_Details._checkInTo);
                    _cmd_AdditionalDetail.Parameters.AddWithValue("@p_CheckOutTo", _Addit_Details._checkOutTo);
                    _cmd_AdditionalDetail.Parameters.AddWithValue("@p_KeyCollectionType", _Addit_Details._keyCollectionType);
                    _cmd_AdditionalDetail.Parameters.AddWithValue("@p_KeyCollectionComments", _Addit_Details._keyCollectionComments);
                    _cmd_AdditionalDetail.Parameters.AddWithValue("@p_GroupReservation", _Addit_Details._groupReservation);
                    //
                    _cmd_AdditionalDetail.Parameters.AddWithValue("@p_GroupReservationNoOfRooms", _Addit_Details._groupReservationNoOfRooms);
                    _cmd_AdditionalDetail.Parameters.AddWithValue("@p_PetsAllowed", _Addit_Details._petsAllowed);
                    _cmd_AdditionalDetail.Parameters.AddWithValue("@p_PetsAllowedComments", _Addit_Details._petsAllowedComments);
                    SqlParameter _results_AdditionalDetail = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_AdditionalDetail.Direction = ParameterDirection.Output;
                    _cmd_AdditionalDetail.Parameters.Add(_results_AdditionalDetail);
                     
                    con.Open();
                    _cmd_AdditionalDetail.ExecuteNonQuery();
                    _status = _results_AdditionalDetail.Value.ToString(); 
                    if (_status == "")
                    {
                        TempData["Error"] = "Server Error : Contact Technical Support";
                    }
                    else
                    {
                        TempData["okey"] = " Additional Details Update Successfully!";
                        if (Session["WebSearch"] != null)
                        {
                            if (int.Parse(Session["WebSearch"].ToString()) == 1)
                            {
                                HttpWebRequest request = WebRequest.Create("http://roomph.pk/admin/home/fetch-accommodations/" + Session["AccommodationId"]) as HttpWebRequest;
                                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                                Stream stream = response.GetResponseStream();
                            }
                        }
                    }
                    con.Close();

                    return RedirectToAction("AdditionalDetails", "Home");
                }
                catch (Exception ex)
                {
                 TempData["Error"] = "System Error : Contact Technical Support";
                    TempData["ErrorPage"] = "Additional Details Update";
                    return RedirectToAction("Error", "Home");
                }
            }
        } 
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult AccommodationsImages()
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["AccommodationId"] == null || Session["AccommodationId"] == "")
            {
                TempData["Error"] = "System Error : Accommodation Not Found !";
                return RedirectToAction("AddNewAccommodations", "Home");
            }
            else
            {
                try
                {
                    string _status_ACImage = "";
                    string _nextImageId = "";
                    DataSet _ds_ACImage = new DataSet();
                    SqlCommand _cmd_ACImage = new SqlCommand("SP_Accommodations", con);
                    _cmd_ACImage.CommandType = CommandType.StoredProcedure;
                    _cmd_ACImage.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_ACImage.Parameters.AddWithValue("@SessionType", "301");
                    _cmd_ACImage.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    _cmd_ACImage.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                    SqlParameter _results_ACImage = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_ACImage.Direction = ParameterDirection.Output;
                    _cmd_ACImage.Parameters.Add(_results_ACImage);
                    SqlParameter MaxImageId = new SqlParameter("@p_MaxImageId_Return", SqlDbType.VarChar, 500);
                    MaxImageId.Direction = ParameterDirection.Output;
                    _cmd_ACImage.Parameters.Add(MaxImageId);

                    SqlDataAdapter _sda_ACImage = new SqlDataAdapter(_cmd_ACImage);
                    con.Open();
                    _cmd_ACImage.ExecuteNonQuery();
                    _nextImageId = MaxImageId.Value.ToString();
                    _status_ACImage = _results_ACImage.Value.ToString();
                    _sda_ACImage.Fill(_ds_ACImage);
                    Session["_nextImageId"] = _nextImageId;
                    return View(_ds_ACImage);
                }
                catch (Exception ex)
                {
                 TempData["Error"] = "System Error : Contact Technical Support";
                    TempData["ErrorPage"] = "Accommodation Images";
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        [HttpPost]
        [OutputCache(Duration = 10, VaryByParam = "none")]
        public ActionResult AccommodationsImages(HttpPostedFileBase[] Files)
        { 
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["AccommodationId"] == null || Session["AccommodationId"] == "")
            {
                TempData["Error"] = "System Error : Accommodation Not Found !";
                return RedirectToAction("AddNewAccommodations", "Home");
            }
            else
            {
                try
                {
                    var _accommodationId = Session["AccommodationId"];
                    var _nextImageId = Session["_nextImageId"];

                    var _fileCount = Files.Count();
                    if (_fileCount > 1)
                    {
                        con.Open();
                        string _status_AcImage = "";
                        Directory.CreateDirectory(Server.MapPath("../Image/AccomodationImages/" + Session["AccommodationId"]));
                        foreach (HttpPostedFileBase files in Files)
                        {
                            //var fileSize = files.ContentLength;
                            //if ((fileSize / 1048576.0) > 1)
                            //{
                            //    TempData["LogoError"] = "Accommodation Image size recommended : 1MB.";
                            //    // image is too large
                            //}
                            //else
                            //{
                            //WebImage img = new WebImage(files.InputStream);
                            //if (img.Width > 1300)
                            //{ 

                            var fileName = Path.GetFileName(Session["AccommodationId"] + "_" + Session["_nextImageId"] + ".jpeg");
                            var path = Path.Combine(Server.MapPath("../Image/AccomodationImages/" + Session["AccommodationId"]), fileName);
                           // files.SaveAs(path);
                            var _dbParameters_path = "/Image/AccomodationImages/" + Session["AccommodationId"] + "/" + fileName;
                            string filepathtosave = path + fileName;

                            //
                            Image image = Image.FromStream(files.InputStream, true, false);

                            //Size can be change according to your requirement 
                            float thumbWidth = 1024F;
                            float thumbHeight = 653F;
                            //calculate  image  size
                            if (image.Width > image.Height)
                            {
                                thumbHeight = ((float)image.Height / image.Width) * thumbWidth;
                            }
                            else
                            {
                                thumbWidth = ((float)image.Width / image.Height) * thumbHeight;
                            }

                            int actualthumbWidth = Convert.ToInt32(Math.Floor(thumbWidth));
                            int actualthumbHeight = Convert.ToInt32(Math.Floor(thumbHeight));
                            var thumbnailBitmap = new Bitmap(actualthumbWidth, actualthumbHeight);
                            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            var imageRectangle = new Rectangle(0, 0, actualthumbWidth, actualthumbHeight);
                            thumbnailGraph.DrawImage(image, imageRectangle);
                            var ms = new MemoryStream();
                            thumbnailBitmap.Save(path, ImageFormat.Jpeg);
                            ms.Position = 0;
                            GC.Collect();
                            thumbnailGraph.Dispose();
                            thumbnailBitmap.Dispose();
                            image.Dispose();
                            //
                            //if (img.Width > 1300)
                            //{
                            //img.Resize(1000, 1000);
                            ////}
                            //img.Save(path, "jpeg");

                            //Temp
                            var fileName_Temp = Path.GetFileName("Temp_" + Session["AccommodationId"] + "_" + Session["_nextImageId"] + ".jpeg");
                            var path_Temp = Path.Combine(Server.MapPath("~/Image/AccomodationImages/" + Session["AccommodationId"]), fileName_Temp);
                           // files.SaveAs(path_Temp);
                            var _dbParameters_pathTemp = "/Image/AccomodationImages/" + Session["AccommodationId"] + "/" + fileName_Temp;
                            string filepathtosave_Temp = path_Temp + fileName_Temp;

                            //
                            Image image_Temp = Image.FromStream(files.InputStream, true, false);

                            //Size can be change according to your requirement 
                            float thumbWidth_Temp = 1024F;
                            float thumbHeight_Temp = 653F;
                            //calculate  image  size
                            if (image_Temp.Width > image_Temp.Height)
                            {
                                thumbHeight_Temp = ((float)image_Temp.Height / image_Temp.Width) * thumbWidth_Temp;
                            }
                            else
                            {
                                thumbWidth_Temp = ((float)image_Temp.Width / image_Temp.Height) * thumbHeight_Temp;
                            }

                            int actualthumbWidth_Temp = Convert.ToInt32(Math.Floor(thumbWidth_Temp));
                            int actualthumbHeight_Temp = Convert.ToInt32(Math.Floor(thumbHeight_Temp));
                            var thumbnailBitmap_Temp = new Bitmap(actualthumbWidth_Temp, actualthumbHeight_Temp);
                            var thumbnailGraph_Temp = Graphics.FromImage(thumbnailBitmap_Temp);
                            thumbnailGraph_Temp.CompositingQuality = CompositingQuality.HighQuality;
                            thumbnailGraph_Temp.SmoothingMode = SmoothingMode.HighQuality;
                            thumbnailGraph_Temp.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            var imageRectangle_Temp = new Rectangle(0, 0, actualthumbWidth_Temp, actualthumbHeight_Temp);
                            thumbnailGraph_Temp.DrawImage(image_Temp, imageRectangle_Temp);
                            var ms_Temp = new MemoryStream();
                            thumbnailBitmap_Temp.Save(path_Temp, ImageFormat.Jpeg);
                            ms_Temp.Position = 0;
                            GC.Collect();
                            thumbnailGraph_Temp.Dispose();
                            thumbnailBitmap_Temp.Dispose();
                            image_Temp.Dispose();
                            //
                            //if (img.Width > 700)
                            //{
                            //img.Resize(300, 300);
                            ////}
                            //img.Save(path_Temp, "jpeg");
                            //

                            SqlCommand _cmd_AcImage = new SqlCommand("SP_Accommodations", con);
                            _cmd_AcImage.CommandType = CommandType.StoredProcedure;
                            _cmd_AcImage.Parameters.AddWithValue("@ProcedureType", "I");
                            _cmd_AcImage.Parameters.AddWithValue("@SessionType", "301");
                            _cmd_AcImage.Parameters.AddWithValue("@p_OwnerId", Session["OwnerId"]);
                            _cmd_AcImage.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);

                            _cmd_AcImage.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                            _cmd_AcImage.Parameters.AddWithValue("@p_ImageURL", _dbParameters_path);
                            _cmd_AcImage.Parameters.AddWithValue("@p_TempImageURL", _dbParameters_pathTemp);
                            SqlParameter _result_AcImage = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                            _result_AcImage.Direction = ParameterDirection.Output;
                            _cmd_AcImage.Parameters.Add(_result_AcImage);

                            _cmd_AcImage.ExecuteNonQuery();
                            _status_AcImage = _result_AcImage.Value.ToString();

                            Session["_nextImageId"] = Convert.ToInt16(Session["_nextImageId"]) + 1;
                            if (Session["WebSearch"] != null)
                            {
                                if (int.Parse(Session["WebSearch"].ToString()) == 1)
                                {
                                    HttpWebRequest request = WebRequest.Create("http://roomph.pk/admin/home/fetch-accommodations/" + Session["AccommodationId"]) as HttpWebRequest;
                                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                                    Stream stream = response.GetResponseStream();
                                }
                            }
                            //}
                            //else
                            //{
                            //    TempData["LogoErrorMulti"] = "Accommodation Image size recommended : greater than 1300 weight.";
                            //}
                        }

                        // }
                        con.Close();
                        //if(TempData["LogoErrorMulti"]== "Accommodation Image size recommended : greater than 1300 weight.")
                        //{ 
                        //    return RedirectToAction("AccommodationsImages");
                        //}
                        //if (status == "")
                        //{
                        //    TempData["Error"] = "Server Error : Contact Technical Support";
                        //}
                        //else
                        //{
                        //    TempData["okey"] = "Image Upload Successfully!";
                        //    if (int.Parse(Session["WebSearch"].ToString()) == 1)
                        //    {
                        //        HttpWebRequest request = WebRequest.Create("http://roomph.pk/admin/home/fetch-accommodations/" + Session["AccommodationId"]) as HttpWebRequest;
                        //        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                        //        Stream stream = response.GetResponseStream();
                        //    }
                        //}
                        return RedirectToAction("AccommodationsImages");
                    }
                    else
                    {
                        con.Open();
                        string _status_AcImage = "";
                        Directory.CreateDirectory(Server.MapPath("~/Image/AccomodationImages/" + Session["AccommodationId"]));
                        foreach (HttpPostedFileBase files in Files)
                        {


                            //var fileSize = files.ContentLength;
                            //if ((fileSize / 1048576.0) > 1)
                            //{
                            //    TempData["LogoError"] = "Accommodation Image size recommended : 1MB.";
                            //    // image is too large
                            //}
                            //else
                            //{
                            //WebImage img = new WebImage(files.InputStream);
                            //if (img.Width > 1300)
                            //{
                            string fileName = Path.GetFileName(Session["AccommodationId"] + "_" + Session["_nextImageId"] + ".jpg");
                            var path = Path.Combine(Server.MapPath("~/Image/AccomodationImages/" + Session["AccommodationId"]), fileName);
                            //files.SaveAs(path);
                            

                            Image image = Image.FromStream(files.InputStream, true, false);
                             
                                //Size can be change according to your requirement 
                                float thumbWidth = 1024F;
                                float thumbHeight = 653F;
                                //calculate  image  size
                                if (image.Width > image.Height)
                                {
                                    thumbHeight = ((float)image.Height / image.Width) * thumbWidth;
                                }
                                else
                                {
                                    thumbWidth = ((float)image.Width / image.Height) * thumbHeight;
                                }

                                int actualthumbWidth = Convert.ToInt32(Math.Floor(thumbWidth));
                                int actualthumbHeight = Convert.ToInt32(Math.Floor(thumbHeight));
                                var thumbnailBitmap = new Bitmap(actualthumbWidth, actualthumbHeight);
                                var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                                thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                                thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                                thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                var imageRectangle = new Rectangle(0, 0, actualthumbWidth, actualthumbHeight);
                                thumbnailGraph.DrawImage(image, imageRectangle);
                                var ms = new MemoryStream();
                                thumbnailBitmap.Save(path, ImageFormat.Jpeg);
                                ms.Position = 0;
                                GC.Collect();
                                thumbnailGraph.Dispose();
                                thumbnailBitmap.Dispose();
                                image.Dispose();

                                //string ph_path = "~/Image/AccomodationImages/" + Session["AccommodationId"] + "/" + fileName;
                                //Bitmap bitmap = (Bitmap)Image.FromFile(path);
                                //Bitmap newBitmap = new Bitmap(bitmap);
                                //newBitmap.SetResolution(300, 300); 
                                //newBitmap.Save("D:\\New Laptop\\Booking Engine Upload Live\\BookingWhizzAdmins\\BookingWhizzAdmins\\Image\\AccomodationImages\\8853\\8853_1.jpg", ImageFormat.Jpeg);

                                var _dbParameters_path = "/Image/AccomodationImages/" + Session["AccommodationId"] + "/" + fileName;
                                string filepathtosave = path + fileName;
                                //if (img.Width > 1300)
                                //{
                                //img.Resize(1000, 1000);
                                ////}
                                //img.Save(path, "jpeg");

                                //Temp
                                var fileName_Temp = Path.GetFileName("Temp_" + Session["AccommodationId"] + "_" + Session["_nextImageId"] + ".jpeg");
                                var path_Temp = Path.Combine(Server.MapPath("~/Image/AccomodationImages/" + Session["AccommodationId"]), fileName_Temp);
                                //files.SaveAs(path_Temp);
                                var _dbParameters_pathTemp = "/Image/AccomodationImages/" + Session["AccommodationId"] + "/" + fileName_Temp;
                                string filepathtosave_Temp = path_Temp + fileName_Temp;

                            Image image_Temp = Image.FromStream(files.InputStream, true, false);

                                //Size can be change according to your requirement 
                            float thumbWidth_Temp = 1024F;
                            float thumbHeight_Temp = 653F;
                            //calculate  image  size
                            if (image_Temp.Width > image_Temp.Height)
                            {
                                thumbHeight_Temp = ((float)image_Temp.Height / image_Temp.Width) * thumbWidth_Temp;
                            }
                            else
                            {
                                thumbWidth_Temp = ((float)image_Temp.Width / image_Temp.Height) * thumbHeight_Temp;
                            }

                            int actualthumbWidth_Temp = Convert.ToInt32(Math.Floor(thumbWidth_Temp));
                            int actualthumbHeight_Temp = Convert.ToInt32(Math.Floor(thumbHeight_Temp));
                            var thumbnailBitmap_Temp = new Bitmap(actualthumbWidth_Temp, actualthumbHeight_Temp);
                            var thumbnailGraph_Temp = Graphics.FromImage(thumbnailBitmap_Temp);
                            thumbnailGraph_Temp.CompositingQuality = CompositingQuality.HighQuality;
                            thumbnailGraph_Temp.SmoothingMode = SmoothingMode.HighQuality;
                            thumbnailGraph_Temp.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            var imageRectangle_Temp = new Rectangle(0, 0, actualthumbWidth_Temp, actualthumbHeight_Temp);
                            thumbnailGraph_Temp.DrawImage(image_Temp, imageRectangle_Temp);
                            var ms_Temp = new MemoryStream();
                            thumbnailBitmap_Temp.Save(path_Temp, ImageFormat.Jpeg);
                            ms_Temp.Position = 0;
                            GC.Collect();
                            thumbnailGraph_Temp.Dispose();
                            thumbnailBitmap_Temp.Dispose();
                            image_Temp.Dispose();
                            //if (img.Width > 700)
                            //{
                            //img.Resize(300, 300);
                            ////}
                            //img.Save(path_Temp, "jpeg");
                            //
                            SqlCommand _cmd_AcImage = new SqlCommand("SP_Accommodations", con);
                                _cmd_AcImage.CommandType = CommandType.StoredProcedure;
                                _cmd_AcImage.Parameters.AddWithValue("@ProcedureType", "I");
                                _cmd_AcImage.Parameters.AddWithValue("@SessionType", "301");
                                _cmd_AcImage.Parameters.AddWithValue("@p_OwnerId", Session["OwnerId"]);
                                _cmd_AcImage.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);

                                _cmd_AcImage.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                                _cmd_AcImage.Parameters.AddWithValue("@p_ImageURL", _dbParameters_path);
                                _cmd_AcImage.Parameters.AddWithValue("@p_TempImageURL", _dbParameters_pathTemp);
                                SqlParameter _result_AcImage = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                                _result_AcImage.Direction = ParameterDirection.Output;
                                _cmd_AcImage.Parameters.Add(_result_AcImage);


                                _cmd_AcImage.ExecuteNonQuery();
                                _status_AcImage = _result_AcImage.Value.ToString();

                                TempData["okey"] = "Images Upload Successfully!";
                                //if (Session["WebSearch"] != null)
                                //{
                                //    if (int.Parse(Session["WebSearch"].ToString()) == 1)
                                //    {
                                //        HttpWebRequest request = WebRequest.Create("http://roomph.pk/admin/home/fetch-accommodations/" + Session["AccommodationId"]) as HttpWebRequest;
                                //        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                                //        Stream stream = response.GetResponseStream();
                                //    }
                                //}
                                //} 
                                //else
                                //{
                                //    TempData["LogoErrorMulti"] = "Accommodation Image size recommended : greater than 1300 weight.";
                                //}
                                con.Close();
                                //}
                                 
                        }
                        return RedirectToAction("AccommodationsImages", "Home");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "System Error : Contact Technical Support";
                    TempData["ErrorPage"] = "Accommodations Images";
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        public ActionResult AccommodationsDescription(int _imageId, string _description)
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["AccommodationId"] == null || Session["AccommodationId"] == "")
            {
                TempData["Error"] = "System Error : Accommodation Not Found !";
                return RedirectToAction("AddNewAccommodations", "Home");
            }
            else
            {
                try
                {

                    string _status_AcImageDesc = ""; 
                    SqlCommand _cmd_AcImageDesc = new SqlCommand("SP_Accommodations", con);
                    _cmd_AcImageDesc.CommandType = CommandType.StoredProcedure;
                    _cmd_AcImageDesc.Parameters.AddWithValue("@ProcedureType", "M");
                    _cmd_AcImageDesc.Parameters.AddWithValue("@SessionType", "301");
                    _cmd_AcImageDesc.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    _cmd_AcImageDesc.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                    _cmd_AcImageDesc.Parameters.AddWithValue("@p_ImageId", _imageId);
                    _cmd_AcImageDesc.Parameters.AddWithValue("@p_ImageDescription", _description);

                    SqlParameter _results_AcDesc = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_AcDesc.Direction = ParameterDirection.Output;
                    _cmd_AcImageDesc.Parameters.Add(_results_AcDesc);
                     
                    con.Open();
                    _cmd_AcImageDesc.ExecuteNonQuery();
                    _status_AcImageDesc = _results_AcDesc.Value.ToString(); 
                    if (_status_AcImageDesc == "")
                    {
                        TempData["Error"] = "Server Error : Contact Technical Support";
                    }
                    else
                    {
                        TempData["okey"] = "Update Image Description Successfully!";
                        if (Session["WebSearch"] != null)
                        {
                            if (int.Parse(Session["WebSearch"].ToString()) == 1)
                            {
                                HttpWebRequest request = WebRequest.Create("http://roomph.pk/admin/home/fetch-accommodations/" + Session["AccommodationId"]) as HttpWebRequest;
                                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                                Stream stream = response.GetResponseStream();
                            }
                        }
                    }
                    con.Close();
                    ////
                    //return RedirectToAction("ManageAccommodationImages");
                    return Json(new { message = "Description has been update" + _description, _imageId = _imageId });
                }
                catch (Exception ex)
                {
                 TempData["Error"] = "System Error : Contact Technical Support";

                    TempData["ErrorPage"] = "Accommodations Images Descrip";
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        public ActionResult SortingAccommodationImages(string _imageId)
        {

            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            string[] _AllimagesId = _imageId.Split(',');

            int y = 1;
            DataTable tblSortAccommodationImages = (DataTable)Session["tblSortAccommodationImages"];
            tblSortAccommodationImages = new DataTable("tblSortAccommodationImages"); // Declare as Global.
            tblSortAccommodationImages.Columns.Add("MultiLanguageId", typeof(string));
            tblSortAccommodationImages.Columns.Add("AccommodationId", typeof(string));
            tblSortAccommodationImages.Columns.Add("ImageId", typeof(string));
            tblSortAccommodationImages.Columns.Add("SortId", typeof(string));
            for (int i = 0; i < _AllimagesId.Length; i++)
            {

                int dtRows = tblSortAccommodationImages.Rows.Count;

                if (dtRows == 0)
                {
                    DataRow dr = tblSortAccommodationImages.NewRow();
                    dr["MultiLanguageId"] = LanguageId;
                    dr["AccommodationId"] = Session["AccommodationId"];
                    dr["ImageId"] = _AllimagesId[i];
                    dr["SortId"] = y;
                    tblSortAccommodationImages.Rows.Add(dr);
                    y++;
                }
                else
                {
                    DataRow dr = tblSortAccommodationImages.NewRow();
                    dr["MultiLanguageId"] = LanguageId;
                    dr["AccommodationId"] = Session["AccommodationId"];
                    dr["ImageId"] = _AllimagesId[i];
                    dr["SortId"] = y;
                    tblSortAccommodationImages.Rows.Add(dr);
                    y++;
                }
            }
            Session["tblSortAccommodationImages"] = tblSortAccommodationImages;

            //
            SqlCommand _cmd_SortImage = new SqlCommand("SP_Accommodations", con);
            _cmd_SortImage.CommandType = CommandType.StoredProcedure;

            _cmd_SortImage.Parameters.AddWithValue("@ProcedureType ", "M");
            _cmd_SortImage.Parameters.AddWithValue("@SessionType", 302);
            _cmd_SortImage.Parameters.AddWithValue("@p_dataset_sortid_upd", Session["tblSortAccommodationImages"]);
            SqlParameter Msg_out = new SqlParameter("@MessageOut", SqlDbType.NVarChar, 100);
            Msg_out.Direction = ParameterDirection.Output;
            _cmd_SortImage.Parameters.Add(Msg_out);
            con.Open();
            _cmd_SortImage.ExecuteNonQuery();

            string _status_SortImage = _cmd_SortImage.Parameters["@MessageOut"].Value.ToString();

            if (_status_SortImage.Equals("Success"))
            {
                Session["tblSortAccommodationImages"] = null;
                TempData["okey"] = "Image Sorting Update Successfully!";
                if (Session["WebSearch"] != null)
                {
                    if (int.Parse(Session["WebSearch"].ToString()) == 1)
                    {
                        HttpWebRequest request = WebRequest.Create("http://roomph.pk/admin/home/fetch-accommodations/" + Session["AccommodationId"]) as HttpWebRequest;
                        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                        Stream stream = response.GetResponseStream();
                    }
                }
            }
            //
            return RedirectToAction("AccommodationsImages");
        }
        public ActionResult RemoveManageAccommodationImages(int _imageId)
        {

            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["AccommodationId"] == null || Session["AccommodationId"] == "")
            {
                TempData["Error"] = "System Error : Accommodation Not Found !";
                return RedirectToAction("AddNewAccommodations", "Home");
            }
            else
            {
                try
                {
                    string FileName = Session["AccommodationId"] + "_" + _imageId + ".jpeg";
                    string Paths = Server.MapPath("~/Image/AccomodationImages/" + Session["AccommodationId"] + "/" + FileName);

                    FileInfo file = new FileInfo(Paths);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    //Temp
                    string FileName_Temp = "Temp_" + Session["AccommodationId"] + "_" + _imageId + ".jpeg";
                    string Paths_Temp = Server.MapPath("~/Image/AccomodationImages/" + Session["AccommodationId"] + "/" + FileName_Temp);

                    FileInfo file_Temp = new FileInfo(Paths_Temp);
                    if (file_Temp.Exists)
                    {
                        file_Temp.Delete();
                    }
                    //
                    string _status_RemoveAcImage = "";
                    DataSet ds_RemoveAcImage = new DataSet();
                    SqlCommand _cmd_RemoveAcImage = new SqlCommand("SP_Accommodations", con);
                    _cmd_RemoveAcImage.CommandType = CommandType.StoredProcedure;
                    _cmd_RemoveAcImage.Parameters.AddWithValue("@ProcedureType", "D");
                    _cmd_RemoveAcImage.Parameters.AddWithValue("@SessionType", "301");
                    _cmd_RemoveAcImage.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    _cmd_RemoveAcImage.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                    _cmd_RemoveAcImage.Parameters.AddWithValue("@p_ImageId", _imageId);
                    SqlParameter _result = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _result.Direction = ParameterDirection.Output;
                    _cmd_RemoveAcImage.Parameters.Add(_result); 
                    con.Open();
                    _cmd_RemoveAcImage.ExecuteNonQuery();
                    _status_RemoveAcImage = _result.Value.ToString(); 
                    con.Close();
                    if (_status_RemoveAcImage == "")
                    {
                        TempData["Error"] = "Server Error : Contact Technical Support";
                    }
                    else
                    {
                        TempData["okey"] = " Accommodations Image Removed Successfully!";
                        if (Session["WebSearch"] != null)
                        {
                            if (int.Parse(Session["WebSearch"].ToString()) == 1)
                            {
                                HttpWebRequest request = WebRequest.Create("http://roomph.pk/admin/home/fetch-accommodations/" + Session["AccommodationId"]) as HttpWebRequest;
                                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                                Stream stream = response.GetResponseStream();
                            }
                        }
                    }
                    return RedirectToAction("AccommodationsImages");
                }
                catch (Exception ex)
                {
                 TempData["Error"] = "System Error : Contact Technical Support";

                    TempData["ErrorPage"] = "Accommodations Images Removed";
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        public ActionResult ManageLocation()
        {

            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["AccommodationId"] == null || Session["AccommodationId"] == "")
            {
                TempData["Error"] = "System Error : Accommodation Not Found !";
                return RedirectToAction("AddNewAccommodations", "Home");
            }
            else
            {
                try
                {
                    string _status_AcLocation = "";
                    DataSet _ds_AcLocation = new DataSet();
                    SqlCommand _cmd_AcLocation = new SqlCommand("SP_Accommodations", con);
                    _cmd_AcLocation.CommandType = CommandType.StoredProcedure;
                    _cmd_AcLocation.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_AcLocation.Parameters.AddWithValue("@SessionType", "1");
                    _cmd_AcLocation.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    _cmd_AcLocation.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                    _cmd_AcLocation.Parameters.AddWithValue("@p_UserTypeId", Session["UserTypeId"]);
                    _cmd_AcLocation.Parameters.AddWithValue("@p_UserId", Session["UserId"]);
                    SqlParameter _results_AcLocation = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_AcLocation.Direction = ParameterDirection.Output;
                    _cmd_AcLocation.Parameters.Add(_results_AcLocation);

                    SqlDataAdapter _sda_AcLocation = new SqlDataAdapter(_cmd_AcLocation);
                    con.Open();
                    _cmd_AcLocation.ExecuteNonQuery();
                    _status_AcLocation = _results_AcLocation.Value.ToString();
                    _sda_AcLocation.Fill(_ds_AcLocation);
                    string markers = "[";
                    con.Close();
                    con.Open();
                    using (SqlDataReader _sdr_AcLocation = _cmd_AcLocation.ExecuteReader())
                    {
                        while (_sdr_AcLocation.Read())
                        {
                            markers += "{";

                            markers += string.Format("'lat': '{0}',", _sdr_AcLocation["Latitude"]);
                            markers += string.Format("'lng': '{0}',", _sdr_AcLocation["Longitude"]);
                            markers += "},";
                        }
                    }
                    con.Close();
                    markers += "];";
                    ViewBag.Markers = markers; 
                    return View(_ds_AcLocation);
                }
                catch (Exception ex)
                {
                 TempData["Error"] = "System Error : Contact Technical Support"; 
                    TempData["ErrorPage"] = "Accommodations Location";
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        [HttpPost]
        public ActionResult ManageLocation(_ManageLocation _ACLocation)
        { 
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["AccommodationId"] == null || Session["AccommodationId"] == "")
            {
                TempData["Error"] = "System Error : Accommodation Not Found !";
                return RedirectToAction("AddNewAccommodations", "Home");
            }
            else
            {
                try
                {

                    string _status_AcLocation = "";
                    DataSet _ds_AcLocation = new DataSet();
                    SqlCommand _cmd_AcLocation = new SqlCommand("SP_Accommodations", con);
                    _cmd_AcLocation.CommandType = CommandType.StoredProcedure;
                    _cmd_AcLocation.Parameters.AddWithValue("@ProcedureType", "M");
                    _cmd_AcLocation.Parameters.AddWithValue("@SessionType", "1");
                    _cmd_AcLocation.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    _cmd_AcLocation.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                    _cmd_AcLocation.Parameters.AddWithValue("@p_Latitude", _ACLocation._latitude);
                    _cmd_AcLocation.Parameters.AddWithValue("@p_Longitude", _ACLocation._longitude);
                    _cmd_AcLocation.Parameters.AddWithValue("@p_LocationDescription", _ACLocation._locationDescription);
                    _cmd_AcLocation.Parameters.AddWithValue("@p_NearestTube", _ACLocation._nearestTube);
                    _cmd_AcLocation.Parameters.AddWithValue("@p_NearestAirport", _ACLocation._nearestAirport);
                    _cmd_AcLocation.Parameters.AddWithValue("@p_HowToGetToTheHotel", _ACLocation._howToGetToTheHotel);
                    _cmd_AcLocation.Parameters.AddWithValue("@p_DirectionsProperty", _ACLocation._directionsProperty);
                    _cmd_AcLocation.Parameters.AddWithValue("@p_NearbyAreas", _ACLocation._nearbyAreas);
                    SqlParameter _results_AcLocation = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_AcLocation.Direction = ParameterDirection.Output;
                    _cmd_AcLocation.Parameters.Add(_results_AcLocation);
                     
                    con.Open();
                    _cmd_AcLocation.ExecuteNonQuery();
                    _status_AcLocation = _results_AcLocation.Value.ToString(); 
                    if (_status_AcLocation == "")
                    {
                        TempData["Error"] = "Server Error : Contact Technical Support";
                    }
                    else
                    { 
                        TempData["okey"] = " Accommodation Location Update Successfully!";
                        if (Session["WebSearch"] != null)
                        {
                            if (int.Parse(Session["WebSearch"].ToString()) == 1)
                            {
                                HttpWebRequest request = WebRequest.Create("http://roomph.pk/admin/home/fetch-accommodations/" + Session["AccommodationId"]) as HttpWebRequest;
                                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                                Stream stream = response.GetResponseStream();
                            }
                        }
                    }
                    con.Close(); 
                    return RedirectToAction("ManageLocation", "Home");
                }
                catch (Exception ex)
                {
                 TempData["Error"] = "System Error : Contact Technical Support";

                    TempData["ErrorPage"] = "Accommodations Location Update";
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        public ActionResult AccommodationFacilities()
        {
            var LanguageId =Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["AccommodationId"] == null || Session["AccommodationId"].ToString() == "")
            {
                TempData["Error"] = "System Error : Accommodation Not Found !";
                return RedirectToAction("AddNewAccommodations", "Home");
            }
            else
            {
                try
                {
                    string _status_AcFacilities = "";
                    DataSet _ds_AcFacilities = new DataSet();
                    SqlCommand _cmd_AcFacilities = new SqlCommand("SP_FacilitiesManagement", con);
                    _cmd_AcFacilities.CommandType = CommandType.StoredProcedure;
                    _cmd_AcFacilities.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_AcFacilities.Parameters.AddWithValue("@SessionType", "101");
                    _cmd_AcFacilities.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    SqlParameter _results1 = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results1.Direction = ParameterDirection.Output;
                    _cmd_AcFacilities.Parameters.Add(_results1);

                    SqlDataAdapter adp1 = new SqlDataAdapter(_cmd_AcFacilities);
                    con.Open();
                    _cmd_AcFacilities.ExecuteNonQuery();
                    _status_AcFacilities = _results1.Value.ToString();
                    adp1.Fill(_ds_AcFacilities);

                    return View(_ds_AcFacilities);
                }
                catch (Exception ex)
                {
                    con.Close();
                 TempData["Error"] = "System Error : Contact Technical Support";

                    TempData["ErrorPage"] = "AccommodationFacilities";
                    return RedirectToAction("Error", "Home");
                }
            }

        }
        public string GetAccommodationFacilities(String _GroupId)
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return null;
            }
            if (Session["AccommodationId"] == null)
            {
                return null;
            }
            else
            {
                try
                {
                    string _status = string.Empty;
                    DataSet _ds_GAF = new DataSet();
                    SqlCommand _cmd_GAF = new SqlCommand("SP_FacilitiesManagement", con);
                    _cmd_GAF.CommandType = CommandType.StoredProcedure;
                    _cmd_GAF.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_GAF.Parameters.AddWithValue("@SessionType", "103");
                    _cmd_GAF.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    _cmd_GAF.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                    _cmd_GAF.Parameters.AddWithValue("@p_AFGroupId", _GroupId);
                    _cmd_GAF.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter _sda_GAF = new SqlDataAdapter(_cmd_GAF);
                    _sda_GAF.Fill(_ds_GAF);
                    
                    string _splitValue = null;

                    int x = _ds_GAF.Tables[0].Rows.Count;
                   
                    for (int y = 0; y < x;)
                    {
                        if (_splitValue == null)
                        {
                            _splitValue = _ds_GAF.Tables[0].Rows[y]["FacilityId"].ToString() + "_" + _ds_GAF.Tables[0].Rows[y]["IsChecked"].ToString() + "_" + _ds_GAF.Tables[0].Rows[y]["FacilityName"].ToString() + "_" + _ds_GAF.Tables[0].Rows[y]["Comments"].ToString();
                        }
                        else
                        {
                            _splitValue += "," + _ds_GAF.Tables[0].Rows[y]["FacilityId"].ToString() + "_" + _ds_GAF.Tables[0].Rows[y]["IsChecked"].ToString() + "_" + _ds_GAF.Tables[0].Rows[y]["FacilityName"].ToString() + "_" + _ds_GAF.Tables[0].Rows[y]["Comments"].ToString();
                        }
                        y++;
                    }

                    con.Close();
                    return _splitValue;
                }
                catch (Exception ex)
                {
                 TempData["Error"] = "System Error : Contact Technical Support";

                    TempData["ErrorPage"] = "Accommodation Facilities method";
                    return null;
                }
            }
        }
        [HttpPost]
        public ActionResult AccommodationFacilitiesUpdate(int[] _facilityID, string[] _comments, int[] _groupIDs, string[] _isChecked, string[] _grpFacId)
        {
            var LanguageId = 1;//Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["AccommodationId"] == null || Session["AccommodationId"] == "")
            {
                TempData["Error"] = "System Error : Accommodation Not Found !";
                return RedirectToAction("AddNewAccommodations", "Home");
            }
            else
            {
                try
                {
                    DataSet _ds_AcFacilities = new DataSet();
                    string _status_AcFacilities = "";
                    string _status_AcFacilities_Add = "";
                    SqlCommand _cmd_AcFacilities = new SqlCommand("SP_FacilitiesManagement", con);
                    _cmd_AcFacilities.CommandType = CommandType.StoredProcedure;
                    _cmd_AcFacilities.Parameters.AddWithValue("@ProcedureType", "M");
                    _cmd_AcFacilities.Parameters.AddWithValue("@SessionType", "103");
                    _cmd_AcFacilities.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    _cmd_AcFacilities.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                    SqlParameter _results_AcFacilities = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_AcFacilities.Direction = ParameterDirection.Output;
                    _cmd_AcFacilities.Parameters.Add(_results_AcFacilities);
                    con.Open();
                    _cmd_AcFacilities.ExecuteNonQuery();
                    _status_AcFacilities = _results_AcFacilities.Value.ToString();
                    /////////////---------------//////////////
                 
                    if(_isChecked!=null)
                    {
                        foreach (var _item in _isChecked)
                        {
                            int pos = Array.IndexOf(_grpFacId, _item);
                            if (pos > -1)
                            {
                                int _FacId = _facilityID[pos];
                                int _GroupId = _groupIDs[pos];
                                string _FacComments = _comments[pos].ToString();

                                SqlCommand _cmd_AcFacilities_Add = new SqlCommand("SP_FacilitiesManagement", con);
                                _cmd_AcFacilities_Add.CommandType = CommandType.StoredProcedure;
                                _cmd_AcFacilities_Add.Parameters.AddWithValue("@ProcedureType", "I");
                                _cmd_AcFacilities_Add.Parameters.AddWithValue("@SessionType", "103");
                                _cmd_AcFacilities_Add.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                                _cmd_AcFacilities_Add.Parameters.AddWithValue("@p_OwnerId", Session["OwnerId"]);
                                _cmd_AcFacilities_Add.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                                _cmd_AcFacilities_Add.Parameters.AddWithValue("@p_AFGroupId", _GroupId);
                                _cmd_AcFacilities_Add.Parameters.AddWithValue("@p_AFacilityId", _FacId);
                                _cmd_AcFacilities_Add.Parameters.AddWithValue("@p_Comments", _FacComments);
                                SqlParameter _results_AcFacilities_Add = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                                _results_AcFacilities_Add.Direction = ParameterDirection.Output;
                                _cmd_AcFacilities_Add.Parameters.Add(_results_AcFacilities_Add);

                                _cmd_AcFacilities_Add.ExecuteNonQuery();
                                _status_AcFacilities_Add = _results_AcFacilities_Add.Value.ToString();
                            }
                        }
                        if (_status_AcFacilities_Add == "")
                        {
                            TempData["Error"] = "Server Error : Contact Technical Support";
                        }
                        TempData["okey"] = " Accommodation Facilities & Comments Update Successfully!";
                    }
                    else
                    { 
                        TempData["Error"] = "Select the Facilities !";
                    }
                    return RedirectToAction("AccommodationFacilities", "Home");
                }
                catch (Exception ex)
                { 
                    con.Close();
                 TempData["Error"] = "System Error : Contact Technical Support";

                    TempData["ErrorPage"] = "Accommodation Facilities Update";
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        public string AccommodationGroupFacilities(string _AFGroupName = "N", int _AFGroupId = 'N', string _AFacilityName = "N",string _OtherGroupName="N",string _OtherFacilitiesName="N")
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return null;
            }
            if (Session["AccommodationId"] == null || Session["AccommodationId"] == "")
            {
                return null;
            }
            else
            {
                try
                {
                    string _status_AcGF = "";
                    SqlCommand _cmd_AcGF = new SqlCommand("SP_FacilitiesManagement", con);
                    _cmd_AcGF.CommandType = CommandType.StoredProcedure;

                    if (_AFGroupName == "")
                    {

                        _cmd_AcGF.Parameters.AddWithValue("@ProcedureType", "I");
                        _cmd_AcGF.Parameters.AddWithValue("@SessionType", "102");
                        _cmd_AcGF.Parameters.AddWithValue("@p_OwnerId", Session["OwnerId"]);
                        _cmd_AcGF.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                        _cmd_AcGF.Parameters.AddWithValue("@p_AFGroupId", _AFGroupId);
                        _cmd_AcGF.Parameters.AddWithValue("@p_AFacilityName", _AFacilityName);
                        _cmd_AcGF.Parameters.AddWithValue("@p_ML_FacilityName", _OtherFacilitiesName);
                        TempData["okey"] = "Added New Facilities Successfully!"; 
                    }
                    else
                    {
                        _cmd_AcGF.Parameters.AddWithValue("@ProcedureType", "I");
                        _cmd_AcGF.Parameters.AddWithValue("@SessionType", "101");
                        _cmd_AcGF.Parameters.AddWithValue("@p_OwnerId", Session["OwnerId"]);
                        _cmd_AcGF.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                        _cmd_AcGF.Parameters.AddWithValue("@p_AFGroupName", _AFGroupName);
                        _cmd_AcGF.Parameters.AddWithValue("@p_ML_AFGroupName", _OtherGroupName); 
                        TempData["okey"] = "Added New Group Successfully!";
                    }

                    SqlParameter _results1 = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results1.Direction = ParameterDirection.Output;
                    _cmd_AcGF.Parameters.Add(_results1);

                    SqlDataAdapter adp1 = new SqlDataAdapter(_cmd_AcGF);
                    con.Open();
                    _cmd_AcGF.ExecuteNonQuery();
                    _status_AcGF = _results1.Value.ToString();
                    if (_status_AcGF == "")
                    {
                        TempData["Error"] = "Server Error : Contact Technical Support";
                    }
                    return _status_AcGF;
                }
                catch (Exception ex)
                {
                 TempData["Error"] = "System Error : Contact Technical Support";

                    TempData["ErrorPage"] = "Accommodation Facilities Add";
                    return null;
                }
            }
        }
        public ActionResult AccommodationTaxes()
        {
            var LanguageId = Session["LanguageId"];// Session["LanguageId"];
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["AccommodationId"] == null || Session["AccommodationId"] == "")
            {
                TempData["Error"] = "System Error : Accommodation Not Found !";
                return RedirectToAction("AddNewAccommodations", "Home");
            }
            else
            {
                try
                {
                    //string _status = "";
                    //DataSet _ds_AcTaxes = new DataSet();
                    //SqlCommand _cmd_AcTaxes = new SqlCommand("SP_Accommodations_Test", con);
                    //_cmd_AcTaxes.CommandType = CommandType.StoredProcedure;
                    //_cmd_AcTaxes.Parameters.AddWithValue("@ProcedureType", "G");
                    //_cmd_AcTaxes.Parameters.AddWithValue("@SessionType", "401");
                    //_cmd_AcTaxes.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    ////cmd.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                    //SqlParameter _results_AcTaxes = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    //_results_AcTaxes.Direction = ParameterDirection.Output;
                    //_cmd_AcTaxes.Parameters.Add(_results_AcTaxes);

                    //SqlDataAdapter _adp_AcTaxes = new SqlDataAdapter(_cmd_AcTaxes);
                    //con.Open();
                    //_cmd_AcTaxes.ExecuteNonQuery();
                    //_status = _results_AcTaxes.Value.ToString();
                    //_adp_AcTaxes.Fill(_ds_AcTaxes);
                    //
                    string _status_Tax = "";
                    DataSet _ds_Tax = new DataSet();
                    SqlCommand _cmd_Tax = new SqlCommand("SP_Accommodations", con);
                    _cmd_Tax.CommandType = CommandType.StoredProcedure;
                    _cmd_Tax.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_Tax.Parameters.AddWithValue("@SessionType", "402");
                    _cmd_Tax.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    SqlParameter _results_Tax = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_Tax.Direction = ParameterDirection.Output;
                    _cmd_Tax.Parameters.Add(_results_Tax);
                    
                    SqlDataAdapter _sda_Tax = new SqlDataAdapter(_cmd_Tax);
                    con.Open();
                    _cmd_Tax.ExecuteNonQuery();
                    _status_Tax = _results_Tax.Value.ToString();
                    _sda_Tax.Fill(_ds_Tax);
                    //
                    string _status_TaxType = "";
                    DataSet _ds_TaxType = new DataSet();
                    SqlCommand _cmd_TaxType = new SqlCommand("SP_RatePlansManagement", con);
                    _cmd_TaxType.CommandType = CommandType.StoredProcedure;
                    _cmd_TaxType.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_TaxType.Parameters.AddWithValue("@SessionType", "1");
                    _cmd_TaxType.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    SqlParameter _results_TaxType = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_TaxType.Direction = ParameterDirection.Output;
                    _cmd_TaxType.Parameters.Add(_results_TaxType);

                    SqlDataAdapter _sda_TaxType = new SqlDataAdapter(_cmd_TaxType);
                    _cmd_TaxType.ExecuteNonQuery();
                    _status_TaxType = _results_TaxType.Value.ToString();
                    _sda_TaxType.Fill(_ds_TaxType);
                    //
                    //
                    string _status_taxChargeType = "";
                    DataSet _ds_taxChargeType = new DataSet();
                    SqlCommand _cmd_taxChargeType = new SqlCommand("SP_Accommodations", con);
                    _cmd_taxChargeType.CommandType = CommandType.StoredProcedure;
                    _cmd_taxChargeType.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_taxChargeType.Parameters.AddWithValue("@SessionType", "400");
                    _cmd_taxChargeType.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    SqlParameter _results_taxChargeType = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_taxChargeType.Direction = ParameterDirection.Output;
                    _cmd_taxChargeType.Parameters.Add(_results_taxChargeType);

                    SqlDataAdapter _sda_taxChargeType = new SqlDataAdapter(_cmd_taxChargeType);
                    _cmd_taxChargeType.ExecuteNonQuery();
                    _status_taxChargeType = _results_taxChargeType.Value.ToString();
                    _sda_taxChargeType.Fill(_ds_taxChargeType);
                    //
                    string _status_TaxPriceMode = "";
                    DataSet _ds_TaxPriceMode = new DataSet();
                    SqlCommand _cmd_TaxPriceMode = new SqlCommand("SP_Accommodations", con);
                    _cmd_TaxPriceMode.CommandType = CommandType.StoredProcedure;
                    _cmd_TaxPriceMode.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_TaxPriceMode.Parameters.AddWithValue("@SessionType", "404");
                    _cmd_TaxPriceMode.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                    _cmd_TaxPriceMode.Parameters.AddWithValue("@p_MultiLanguageId", LanguageId);
                    SqlParameter _results_TaxPriceMode = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_TaxPriceMode.Direction = ParameterDirection.Output;
                    _cmd_TaxPriceMode.Parameters.Add(_results_TaxPriceMode);

                    SqlDataAdapter _sda_TaxPriceMode = new SqlDataAdapter(_cmd_TaxPriceMode);
                    _cmd_TaxPriceMode.ExecuteNonQuery();
                    _status_TaxPriceMode = _results_TaxPriceMode.Value.ToString();
                    _sda_TaxPriceMode.Fill(_ds_TaxPriceMode);
                    con.Close();
                    DataSet[] _ds_Array = new DataSet[] { _ds_Tax, _ds_TaxType, _ds_TaxPriceMode, _ds_taxChargeType };
                    return View(_ds_Array);
                }
                catch (Exception ex)
                {
                 TempData["Error"] = "System Error : Contact Technical Support";

                    TempData["ErrorPage"] = "Accommodation Taxes";
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        public ActionResult UpdateTaxes(int[] _taxId, int[] _taxTypeId, int[] _priceModeId, string[] _taxValue, string[] _taxChargeType, string[] _taxSorting)
        {
            if (_taxId == null)
            {
                _taxId = new int[1];
                // TaxId[1] = 0;
            }
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["AccommodationId"] == null)
            {
                return RedirectToAction("AddNewAccommodations");
            }
            else
            {
                try
                {
                    string _taxid_Value = "", _taxtypeid_Value = "", _pricemodeid_Value = "", _tax_Value = "", _tax_Charge = "", _tax_sort = "";
                    for (int i = 0; i < _taxId.Length; i++)
                    {
                        if (_taxId[i] != 0)
                        {
                            if (i == _taxId.Length - 1)
                            {
                                _taxid_Value = _taxid_Value + _taxId[i];
                            }
                            else
                            {
                                _taxid_Value = _taxid_Value + _taxId[i] + ",";
                            }
                        }
                    }

                    for (int j = 0; j < _taxTypeId.Length; j++)
                    {
                        if (_taxTypeId[j] != 0)
                        {

                            if (j == _taxTypeId.Length - 1)
                            {
                                _taxtypeid_Value = _taxtypeid_Value + _taxTypeId[j];
                            }

                            else
                            {
                                _taxtypeid_Value = _taxtypeid_Value + _taxTypeId[j] + ",";
                            }
                        }
                        else
                        {
                            _taxTypeId = _taxTypeId.Where(w => w != _taxTypeId[j]).ToArray();
                            _taxtypeid_Value = String.Join(",", _taxTypeId);
                            break;
                            //_taxtypeid = _taxtypeid + TaxTypeId[j] + ",";
                        }
                    }

                    for (int k = 0; k < _priceModeId.Length; k++)
                    {
                        if (_priceModeId[k] != 0)
                        {
                            if (k == _priceModeId.Length - 1)
                            {
                                _pricemodeid_Value = _pricemodeid_Value + _priceModeId[k];
                            }
                            else
                            {
                                _pricemodeid_Value = _pricemodeid_Value + _priceModeId[k] + ",";
                            }
                        }
                        else
                        {
                            _priceModeId = _priceModeId.Where(w => w != _priceModeId[k]).ToArray();
                            _pricemodeid_Value = String.Join(",", _priceModeId);
                            break;
                        }
                    }

                    for (int l = 0; l < _taxValue.Length; l++)
                    {
                        if (_taxValue[l] != "")
                        {
                            if (l == _taxValue.Length - 1)
                            {
                                _tax_Value = _tax_Value + _taxValue[l];
                            }
                            else
                            {
                                _tax_Value = _tax_Value + _taxValue[l] + ",";
                            }
                        }
                        else
                        {
                            _taxValue = _taxValue.Where(w => w != _taxValue[l]).ToArray();
                            _tax_Value = String.Join(",", _taxValue);
                            break;
                        }
                    }

                    for (int l = 0; l < _taxChargeType.Length; l++)
                    {
                        if (_taxChargeType[l] != "")
                        {
                            if (l == _taxChargeType.Length - 1)
                            {
                                _tax_Charge = _tax_Charge + _taxChargeType[l];
                            }
                            else
                            {
                                _tax_Charge = _tax_Charge + _taxChargeType[l] + ",";
                            }
                        }
                        else
                        {
                            _taxChargeType = _taxChargeType.Where(w => w != _taxChargeType[l]).ToArray();
                            _tax_Charge = String.Join(",", _taxChargeType);
                            break;
                        }
                    }
                     
                    for (int l = 0; l < _taxSorting.Length; l++)
                    {
                        if (_taxSorting[l] != "")
                        {
                            if (l == _taxSorting.Length - 1)
                            {
                                _tax_sort = _tax_sort + _taxSorting[l];
                            }
                            else
                            {
                                _tax_sort = _tax_sort + _taxSorting[l] + ",";
                            }
                        }
                        else
                        {
                            _taxSorting = _taxSorting.Where(w => w != _taxSorting[l]).ToArray();
                            _tax_sort = String.Join(",", _taxSorting);
                            break;
                        }
                    }

                    var LanguageId = Session["LanguageId"];// Session["LanguageId"];
                    string _status_AddTex = "";
                    DataSet _ds_AddTex = new DataSet();
                    SqlCommand _cmd_AddTex = new SqlCommand("SP_Accommodations", con);
                    _cmd_AddTex.CommandType = CommandType.StoredProcedure;
                    _cmd_AddTex.Parameters.AddWithValue("@ProcedureType", "I");
                    _cmd_AddTex.Parameters.AddWithValue("@SessionType", "401");
                    _cmd_AddTex.Parameters.AddWithValue("@p_MultiLanguageId", 1);
                    _cmd_AddTex.Parameters.AddWithValue("@p_AccommodationId", Session["AccommodationId"]);
                    _cmd_AddTex.Parameters.AddWithValue("@p_OwnerId", 1);
                    _cmd_AddTex.Parameters.AddWithValue("@p_TaxId", _taxid_Value);
                    _cmd_AddTex.Parameters.AddWithValue("@p_TaxTypeId", _taxtypeid_Value);
                    _cmd_AddTex.Parameters.AddWithValue("@p_PriceModeId", _pricemodeid_Value);
                    _cmd_AddTex.Parameters.AddWithValue("@p_TaxValue", _tax_Value);
                    _cmd_AddTex.Parameters.AddWithValue("@P_TaxChargeTypeId", _tax_Charge);

                    _cmd_AddTex.Parameters.AddWithValue("@P_TaxSorting", _tax_sort);
                    SqlParameter _results_AddTex = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_AddTex.Direction = ParameterDirection.Output;
                    _cmd_AddTex.Parameters.Add(_results_AddTex);

                    con.Open();
                    _cmd_AddTex.ExecuteNonQuery();
                    _status_AddTex = _results_AddTex.Value.ToString();
                    if (_status_AddTex == "")
                    {
                        TempData["Error"] = "Server Error : Contact Technical Support";
                    }
                    else
                    {
                        TempData["okey"] = " Accommodation Taxes Update Successfully!";
                        //if (Session["WebSearch"] != null)
                        //{
                        //    if (int.Parse(Session["WebSearch"].ToString()) == 1)
                        //    {
                        //        HttpWebRequest request = WebRequest.Create("http://roomph.pk/admin/home/fetch-accommodations/" + Session["AccommodationId"]) as HttpWebRequest;
                        //        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                        //        Stream stream = response.GetResponseStream();
                        //    }
                        //}
                    }
                    con.Close();

                    return RedirectToAction("AccommodationTaxes");
                }
                catch (Exception ex)
                {
                 TempData["Error"] = "System Error : Contact Technical Support";

                    TempData["ErrorPage"] = "Accommodation Taxes Update";
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        public ActionResult AccommodationFacilitiesRemoveGroup(int GroupIds)
        {
            return RedirectToAction("AccommodationFacilities");
        }
        public ActionResult AccommodationFacilitiesRemove(int GroupIdR, int FacilitiesIdR)
        {
            return RedirectToAction("AccommodationFacilities");
        }
        public ActionResult Error()
        {
            if (Session["LoginId"] == null || Session["LoginId"] == "" || Session["OwnerId"] == "" || Session["OwnerId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        } 
        public class Post
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Link { get; set; }
            public IList<string> Categories { get; set; }
        }
        public ActionResult Test()
        {
            var abc = new JObject[2];


            string[] arr = new string[3];
            arr[0] = "123";
            arr[1] = "890";
            arr[2] = "789";

            string[] farooq = new string[3];


            string[] assign = new string[2];

            var fr = new[]
                            {
                              new {Ali= "p.Categories", farooq1 = "farooq1010101", RajputArray = new JArray { arr }},
                            };

            //assign[0]=






            List<Post> posts = new List<Post>
            {
             new Post
               {
                   Title = "Episode VII",
                   Description = "Episode VII production",
                   Categories = new List<string>
                   {
                       "episode-vii",
                       "movie"
                   },
                   Link = "episode-vii-production.aspx"
               }
            };
            JObject o = JObject.FromObject(new
            {
                channel = new
                {
                    title = "Star Wars",
                    link = "http://www.starwars.com",
                    description = new
                    {
                        title = "Title",
                        description = "p.Description",
                        link = "p.Link",
                        category = new
                        {
                            fr
                        },
                        item =
                        from p in posts
                        orderby p.Title
                        select new
                        {
                            title = p.Title,
                            description = p.Description,
                            link = p.Link,
                            category = p.Categories
                        }
                    }
                }
            });
            return View();
        }

        public ActionResult HotelReport(string AddressId="",string WebSearch="",string HomeShopping="", string OwnerId = "")
        { 
            if (Session["LoginType"] == null)
            {
                return RedirectToAction("Login", "Account");
            } 
            else
            {
                try
                {
                    string _status_HotelReport = "";
                    DataSet _ds_HotelReport = new DataSet();
                    SqlCommand _cmd_HotelReport = new SqlCommand("SP_Accommodations", con);
                    _cmd_HotelReport.CommandType = CommandType.StoredProcedure;
                    _cmd_HotelReport.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_HotelReport.Parameters.AddWithValue("@SessionType", "700"); 
                    _cmd_HotelReport.Parameters.AddWithValue("@p_AddressId", AddressId);
                    _cmd_HotelReport.Parameters.AddWithValue("@p_WebSearch", WebSearch);
                    _cmd_HotelReport.Parameters.AddWithValue("@p_HomeShopping", HomeShopping);
                    _cmd_HotelReport.Parameters.AddWithValue("@p_OwnerId", OwnerId);
                    SqlParameter _results_HotelReport = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_HotelReport.Direction = ParameterDirection.Output;
                    _cmd_HotelReport.Parameters.Add(_results_HotelReport);

                    SqlDataAdapter _sda_AcLocation = new SqlDataAdapter(_cmd_HotelReport);
                    con.Open();
                    _cmd_HotelReport.ExecuteNonQuery();
                    _status_HotelReport = _results_HotelReport.Value.ToString();
                    _sda_AcLocation.Fill(_ds_HotelReport); 


                    string _status_Country = "";
                    DataSet _ds_Country = new DataSet();
                    SqlCommand _cmd_Country = new SqlCommand("SP_Accommodations", con);
                    _cmd_Country.CommandType = CommandType.StoredProcedure;
                    _cmd_Country.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_Country.Parameters.AddWithValue("@SessionType", "103");
                    _cmd_Country.Parameters.AddWithValue("@p_MultiLanguageId", 1);
                    SqlParameter _results_Country = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_Country.Direction = ParameterDirection.Output;
                    _cmd_Country.Parameters.Add(_results_Country);

                    SqlDataAdapter _sda_Country = new SqlDataAdapter(_cmd_Country);
                    _cmd_Country.ExecuteNonQuery();
                    _status_Country = _results_Country.Value.ToString();
                    _sda_Country.Fill(_ds_Country);

                    //
                    string _status_User = "";
                    DataSet _ds_User = new DataSet();
                    SqlCommand _cmd_User = new SqlCommand("SP_Admin", con);
                    _cmd_User.CommandType = CommandType.StoredProcedure;
                    _cmd_User.Parameters.AddWithValue("@ProcedureType", "G");
                    _cmd_User.Parameters.AddWithValue("@SessionType", "3");
                    _cmd_User.Parameters.AddWithValue("@p_UserTypeId", Session["UserTypeId"]);
                    SqlParameter _results_User = new SqlParameter("@MessageOut", SqlDbType.VarChar, 500);
                    _results_User.Direction = ParameterDirection.Output;
                    _cmd_User.Parameters.Add(_results_User);

                    SqlDataAdapter _sda_User = new SqlDataAdapter(_cmd_User);
                    _cmd_User.ExecuteNonQuery();
                    _status_User = _results_User.Value.ToString();
                    _sda_User.Fill(_ds_User);
                    con.Close();
                    DataSet[] _ds_Array = new DataSet[] { _ds_HotelReport, _ds_Country, _ds_User };

                    return View(_ds_Array);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "System Error : Contact Technical Support";
                    TempData["ErrorPage"] = "Accommodations Location";
                    return RedirectToAction("Error", "Home");
                }
            }
        }
    }
}