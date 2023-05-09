using AccessSystem.Model;
using AccessSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccessSystem.Controllers
{
    public class ComputerController : Controller
    {
        static string ErrorMessage = string.Empty;
        static TBL_COMPUTER holdComputer = new TBL_COMPUTER();
        public ActionResult ListScreen()
        {
            try
            {
                // reset error Message and hold data when going to list screen
                ErrorMessage = string.Empty;
                holdComputer = new TBL_COMPUTER();

                if (string.IsNullOrEmpty(this.Session["ADMINID"].ToString()))
                {
                    return RedirectToAction("Login");
                }
                string adminAccountID = this.Session["ADMINID"].ToString();

                AdminModel adminModel = new AdminModel();
                adminModel.AccountInfo = AdminUserRepository.GetCurrentUserInfo(new Guid(adminAccountID));
                ViewBag.Firstname = adminModel.AccountInfo.FIRSTNAME;

                adminModel.ComputerList = ComputerRepository.GetComputers();

                return View(adminModel);
            }
            catch
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult EditScreen(Guid? computerID)
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

                if (computerID == null)
                {
                    ViewBag.EditScreenHeader = "Add Computer";
                    //new
                    adminModel.SelectedComputer = null;
                }
                else
                {
                    ViewBag.EditScreenHeader = "Edit Computer";
                    //Update
                    adminModel.SelectedComputer = ComputerRepository.GetComputerInfo(computerID);
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    adminModel.ErrorMessage = ErrorMessage;
                    adminModel.SelectedComputer = holdComputer;
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
            string message = data.Delete(new TBL_COMPUTER(), adminModel.AreChecked, "COMPUTERID");

            return RedirectToAction("ListScreen");
        }

        [HttpPost]
        public ActionResult Update(TBL_COMPUTER SelectedComputer)
        {
            var result = string.Empty;
            Data data = new Data();
            if (SelectedComputer.COMPUTERID == new Guid())
            {
                //Save

                result = data.Save(SelectedComputer, new List<string> { "COMPUTERID" }, "COMPUTERID");
            }
            else
            {
                //Update
                TBL_COMPUTER Computer = SelectedComputer;
                TBL_COMPUTER filter = new TBL_COMPUTER();
                filter.COMPUTERID = Computer.COMPUTERID;
                Computer.COMPUTERID = new Guid();
                result = data.Update(Computer, filter, new Guid(this.Session["ADMINID"].ToString()));
            }
            if (result != "Success")
            {
                Guid x;
                if (!Guid.TryParse(result, out x)) // check if return value is not UID
                {
                    ErrorMessage = result;
                    holdComputer = SelectedComputer as TBL_COMPUTER;
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            ErrorMessage = string.Empty;
            holdComputer = new TBL_COMPUTER();
            return RedirectToAction("ListScreen");
        }
    }
}