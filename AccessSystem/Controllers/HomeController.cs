using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.WebPages;
using AccessSystem.Model;
using AccessSystem.Repository;
using Newtonsoft.Json;

namespace AccessSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoginCommand(AdminModel adminModel)
        {
            AccessSystemDBEntities dbEntities = new AccessSystemDBEntities();
            var log = (from adminUsers in dbEntities.TBL_ADMINUSER.Where(user => user.USERNAME == adminModel.username && user.PASSWORD == adminModel.password) select adminUsers).FirstOrDefault();
            if (log != null)
            {
                this.Session["ADMINID"] = log.ADMINUSERID.ToString();
                return RedirectToAction("AdminDashboard");
            }

            return View("../Home/Login");
        }
        public ActionResult AdminDashboard()
        {
            if (string.IsNullOrEmpty(this.Session["ADMINID"].ToString()))
            {
                return RedirectToAction("Login");
            }
            string adminAccountID = this.Session["ADMINID"].ToString();

            AdminModel adminModel = new AdminModel();
            adminModel.AccountInfo = AdminUserRepository.GetCurrentUserInfo(new Guid(adminAccountID));
            ViewBag.Firstname = adminModel.AccountInfo.FIRSTNAME;

            return View();
        }

        [HttpPost]
        public ActionResult SaveAccountChanges(TBL_ADMINUSER AccountInfo)
        {
            Data data = new Data();
            TBL_ADMINUSER filter = new TBL_ADMINUSER();
            filter.ADMINUSERID = AccountInfo.ADMINUSERID;
            AccountInfo.ADMINUSERID = new Guid();
            data.Update(AccountInfo,filter,new Guid(this.Session["ADMINID"].ToString()));

            return RedirectToAction("AdminDashboard");
        }

        public ActionResult todo()
        {
            if (string.IsNullOrEmpty(this.Session["ADMINID"].ToString()))
            {
                return RedirectToAction("Login");
            }

            string adminAccountID = this.Session["ADMINID"].ToString();

            AdminModel adminModel = new AdminModel();
            adminModel.AccountInfo = AdminUserRepository.GetCurrentUserInfo(new Guid(adminAccountID));
            ViewBag.Firstname = adminModel.AccountInfo.FIRSTNAME;

            return View();
        }

        public ActionResult AccountSettings()
        {
            if (string.IsNullOrEmpty(this.Session["ADMINID"].ToString()))
            {
                return RedirectToAction("Login");
            }
            string adminAccountID = this.Session["ADMINID"].ToString();

            AdminModel adminModel = new AdminModel();
            adminModel.AccountInfo = AdminUserRepository.GetCurrentUserInfo(new Guid(adminAccountID));
            ViewBag.Firstname = adminModel.AccountInfo.FIRSTNAME;

            return View(adminModel);
        }

        public ActionResult Logout()
        {
            this.Session["ADMINID"] = "";

            return RedirectToAction("Login");
        }

        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Index()
        {
            AdminModel adminModel = new AdminModel();
            adminModel.UserList = UserRepository.GetAllUsers();

            return View(adminModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        [HttpPost]
        public string LOGDATAIN(string NameID, string RFIDCODE)
        {
            return LogRepository.LOGDATAIN(NameID, RFIDCODE);
        }

        [HttpPost]
        public string LOGDATAOUT(string NameID)
        {
            return LogRepository.LOGDATAOUT(NameID);
        }
    }
}