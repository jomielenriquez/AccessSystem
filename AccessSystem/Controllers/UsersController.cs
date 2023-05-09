using AccessSystem.Model;
using AccessSystem.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccessSystem.Controllers
{
    public class UsersController : Controller
    {
        static string ErrorMessage = string.Empty;
        static TBL_NAMES holdUser = new TBL_NAMES();
        static bool isNew = false;
        public ActionResult ListScreen()
        {
            try
            {
                // reset error Message and hold data when going to list screen
                ErrorMessage = string.Empty;
                holdUser = new TBL_NAMES();

                if (string.IsNullOrEmpty(this.Session["ADMINID"].ToString()))
                {
                    return RedirectToAction("Login");
                }
                string adminAccountID = this.Session["ADMINID"].ToString();

                AdminModel adminModel = new AdminModel();
                adminModel.AccountInfo = AdminUserRepository.GetCurrentUserInfo(new Guid(adminAccountID));
                ViewBag.Firstname = adminModel.AccountInfo.FIRSTNAME;

                adminModel.UserList = UserRepository.GetAllUsers();

                return View(adminModel);
            }
            catch
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult EditScreen(Guid? NameID)
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

                adminModel.CourseSectionList = CourseSectionRepository.GetAllCourseSection();

                if (NameID == null)
                {
                    ViewBag.EditScreenHeader = "Add User";
                    //new
                    adminModel.SelectedUser = null;
                }
                else
                {
                    ViewBag.EditScreenHeader = "Edit User";
                    //Update
                    adminModel.SelectedUser = UserRepository.GetUser(NameID);
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    adminModel.ErrorMessage = ErrorMessage;
                    adminModel.SelectedUser = holdUser;
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
            string message = data.Delete(new TBL_NAMES(), adminModel.AreChecked, "NAMEID");

            return RedirectToAction("ListScreen");
        }

        [HttpPost]
        public ActionResult Update(TBL_NAMES SelectedUser)
        {
            var result = string.Empty;
            Data data = new Data();
            if (SelectedUser.NAMEID == new Guid())
            {
                //Save

                result = data.Save(SelectedUser, new List<string> { "NAMEID" }, "NAMEID");
                isNew = true;
            }
            else
            {
                //Update
                TBL_NAMES Name = SelectedUser;
                TBL_NAMES filter = new TBL_NAMES();
                filter.NAMEID = Name.NAMEID;
                Name.NAMEID = new Guid();
                result = data.Update(Name, filter, new Guid(this.Session["ADMINID"].ToString()));
                isNew = false;
            }
            if (result != "Success")
            {
                Guid x;
                if (!Guid.TryParse(result, out x)) // check if return value is not UID
                {
                    ErrorMessage = result;
                    holdUser = SelectedUser as TBL_NAMES;
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            ErrorMessage = string.Empty;
            holdUser = new TBL_NAMES();
            if(isNew)
            {
                holdUser = SelectedUser;
                holdUser.NAMEID = new Guid(result);
                return RedirectToAction("RegisterFaceRFID");
            }
            return RedirectToAction("ListScreen");
        }

        public ActionResult RegisterFaceRFID()
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

                adminModel.CourseSectionList = CourseSectionRepository.GetAllCourseSection();
                adminModel.SelectedUser = holdUser;

                return View(adminModel);
            }
            catch
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public string Capture(string ImageByteText, string FolderName, string captureNumber)
        {
            //byte[] bitmap = hdnimageBytes.Value;
            var t = ImageByteText.Substring(22);  // remove data:image/png;base64,
            byte[] bitmap = Convert.FromBase64String(t);
            using (Image image = Image.FromStream(new MemoryStream(bitmap)))
            {
                createDirectory(FolderName);

                string folderPath = Server.MapPath("~/Scripts/label/" + FolderName + "/");
                string fileName = captureNumber + ".jpg";
                string imagePath = folderPath + fileName;

                image.Save(imagePath, ImageFormat.Jpeg); // Or Png 
            }
            return "Image Saved";
        }

        public void createDirectory(string FolderName)
        {
            string dir = Server.MapPath("~/Scripts/label/" + FolderName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}