using AccessSystem.Model;
using AccessSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccessSystem.Controllers
{
    public class LogController : Controller
    {
        static string ErrorMessage = string.Empty;
        static TBL_LOG holdlog = new TBL_LOG();
        static bool isNew = false;
        public ActionResult ListScreen()
        {
            try
            {
                // reset error Message and hold data when going to list screen
                ErrorMessage = string.Empty;
                holdlog = new TBL_LOG();

                if (string.IsNullOrEmpty(this.Session["ADMINID"].ToString()))
                {
                    return RedirectToAction("Login");
                }
                string adminAccountID = this.Session["ADMINID"].ToString();

                AdminModel adminModel = new AdminModel();
                adminModel.AccountInfo = AdminUserRepository.GetCurrentUserInfo(new Guid(adminAccountID));
                ViewBag.Firstname = adminModel.AccountInfo.FIRSTNAME;

                adminModel.LogList = DataLogsRepository.GetAllLogs();

                return View(adminModel);
            }
            catch
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult EditScreen(Guid? LogID)
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

                adminModel.UserList = UserRepository.GetAllUsers();
                adminModel.ComputerList = ComputerRepository.GetComputers();

                if (LogID == null)
                {
                    ViewBag.EditScreenHeader = "Add Log";
                    //new
                    adminModel.SelectedLog = null;
                }
                else
                {
                    ViewBag.EditScreenHeader = "Edit Log";
                    //Update
                    adminModel.SelectedLog = DataLogsRepository.GetLog(LogID);
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    adminModel.ErrorMessage = ErrorMessage;
                    adminModel.SelectedLog = holdlog;
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
            string message = data.Delete(new TBL_LOG(), adminModel.AreChecked, "LOGID");

            return RedirectToAction("ListScreen");
        }

        [HttpPost]
        public ActionResult Update(TBL_LOG SelectedLog)
        {
            var result = string.Empty;
            Data data = new Data();
            if (SelectedLog.LOGID == new Guid())
            {
                //Save

                result = data.Save(SelectedLog, new List<string> { "LOGID" }, "LOGID");
                isNew = true;
            }
            else
            {
                //Update
                TBL_LOG log = SelectedLog;
                TBL_LOG filter = new TBL_LOG();
                filter.LOGID = log.LOGID;
                log.LOGID = new Guid();
                result = data.Update(log, filter, new Guid(this.Session["ADMINID"].ToString()));
                isNew = false;
            }
            if (result != "Success")
            {
                Guid x;
                if (!Guid.TryParse(result, out x)) // check if return value is not UID
                {
                    ErrorMessage = result;
                    holdlog = SelectedLog as TBL_LOG;
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            ErrorMessage = string.Empty;
            holdlog = new TBL_LOG();
            if (isNew)
            {
                holdlog = SelectedLog;
                holdlog.NAMEID = new Guid(result);
                //return RedirectToAction("RegisterFaceRFID");
            }
            return RedirectToAction("ListScreen");
        }
    }
}