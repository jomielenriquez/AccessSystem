using AccessSystem.Model;
using AccessSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace AccessSystem.Controllers
{
    public class AdminUserController : Controller
    {
        static string ErrorMessage = string.Empty;
        static TBL_ADMINUSER holdAdminUser = new TBL_ADMINUSER();
        // GET: AdminUser
        public ActionResult ListScreen()
        {
            try
            {
                // reset error Message and hold data when going to list screen
                ErrorMessage = string.Empty;
                holdAdminUser = new TBL_ADMINUSER();

                if (string.IsNullOrEmpty(this.Session["ADMINID"].ToString()))
                {
                    return RedirectToAction("Login");
                }
                string adminAccountID = this.Session["ADMINID"].ToString();

                AdminModel adminModel = new AdminModel();
                adminModel.AccountInfo = AdminUserRepository.GetCurrentUserInfo(new Guid(adminAccountID));
                ViewBag.Firstname = adminModel.AccountInfo.FIRSTNAME;

                adminModel.AccountInfoList = AdminUserRepository.GetAllAdminUsers();

                return View(adminModel);
            }
            catch {
                return RedirectToAction("Login");
            }
        }

        public ActionResult EditScreen(Guid? AdminUserID)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Session["ADMINID"].ToString()))
                {
                    return RedirectToAction("Login");
                }
                string adminAccountID = this.Session["ADMINID"].ToString();

                AdminModel adminModel = new AdminModel();
                adminModel.AccountInfo = AdminUserRepository.GetCurrentUserInfo(new Guid(adminAccountID));
                ViewBag.Firstname = adminModel.AccountInfo.FIRSTNAME;

                if(AdminUserID == null)
                {
                    ViewBag.EditScreenHeader = "Add Admin User";
                    //new
                    adminModel.SelectedAccountInfo = null;
                }
                else
                {
                    ViewBag.EditScreenHeader = "Edit Admin User";
                    //Update
                    adminModel.SelectedAccountInfo = AdminUserRepository.GetAdminUser(AdminUserID);
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    adminModel.ErrorMessage = ErrorMessage;
                    adminModel.SelectedAccountInfo = holdAdminUser;
                }

                return View(adminModel);
            }
            catch
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult Delete(AdminModel adminModel)
        {
            Data data = new Data();
            string message = data.Delete(new TBL_ADMINUSER(), adminModel.AreChecked, "ADMINUSERID");

            return RedirectToAction("ListScreen");
        }

        [HttpPost]
        public ActionResult Update(TBL_ADMINUSER SelectedAccountInfo)
        {
            var result = string.Empty;
            Data data = new Data();
            if(SelectedAccountInfo.ADMINUSERID == new Guid())
            {
                //Save
                SelectedAccountInfo.CREATEDBY = this.Session["ADMINID"].ToString();
                // If new user, default username and password will be the firstname of the user
                SelectedAccountInfo.USERNAME = SelectedAccountInfo.FIRSTNAME.ToString().ToLower();
                SelectedAccountInfo.PASSWORD = SelectedAccountInfo.FIRSTNAME.ToString().ToLower();

                result = data.Save(SelectedAccountInfo, new List<string> { "ADMINUSERID" }, "ADMINUSERID");
            }
            else
            {
                //Update
                TBL_ADMINUSER AdminUser = SelectedAccountInfo;
                TBL_ADMINUSER filter = new TBL_ADMINUSER();
                filter.ADMINUSERID = AdminUser.ADMINUSERID;
                AdminUser.ADMINUSERID = new Guid();
                result = data.Update(AdminUser, filter, new Guid(this.Session["ADMINID"].ToString()));
            }
            if(result != "Success")
            {
                Guid x;
                if (!Guid.TryParse(result, out x)) // check if return value is not UID
                {
                    ErrorMessage = result;
                    holdAdminUser = SelectedAccountInfo as TBL_ADMINUSER;
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            ErrorMessage = string.Empty;
            holdAdminUser = new TBL_ADMINUSER();
            return RedirectToAction("ListScreen");
        }
    }
}