using AccessSystem.Model;
using AccessSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccessSystem.Controllers
{
    public class CourseSectionController : Controller
    {
        static string ErrorMessage = string.Empty;
        static TBL_COURSEANDSECTION holdCourseSection = new TBL_COURSEANDSECTION();
        public ActionResult ListScreen()
        {
            try
            {
                // reset error Message and hold data when going to list screen
                ErrorMessage = string.Empty;
                holdCourseSection = new TBL_COURSEANDSECTION();

                if (string.IsNullOrEmpty(this.Session["ADMINID"].ToString()))
                {
                    return RedirectToAction("Login");
                }
                string adminAccountID = this.Session["ADMINID"].ToString();

                AdminModel adminModel = new AdminModel();
                adminModel.AccountInfo = AdminUserRepository.GetCurrentUserInfo(new Guid(adminAccountID));
                ViewBag.Firstname = adminModel.AccountInfo.FIRSTNAME;

                adminModel.CourseSectionList = CourseSectionRepository.GetAllCourseSection();

                return View(adminModel);
            }
            catch
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult EditScreen(Guid? CourseSectionID)
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

                if (CourseSectionID == null)
                {
                    ViewBag.EditScreenHeader = "Add Course and Section";
                    //new
                    adminModel.SelectedCourseSection = null;
                }
                else
                {
                    ViewBag.EditScreenHeader = "Edit Course and Section";
                    //Update
                    adminModel.SelectedCourseSection = CourseSectionRepository.GetAllCourseSectionWithID(CourseSectionID);
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    adminModel.ErrorMessage = ErrorMessage;
                    adminModel.SelectedCourseSection = holdCourseSection;
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
            string message = data.Delete(new TBL_COURSEANDSECTION(), adminModel.AreChecked, "COURSESECTIONID");

            return RedirectToAction("ListScreen");
        }

        [HttpPost]
        public ActionResult Update(TBL_COURSEANDSECTION SelectedCourseSection)
        {
            var result = string.Empty;
            Data data = new Data();
            if (SelectedCourseSection.COURSESECTIONID == new Guid())
            {
                //Save

                result = data.Save(SelectedCourseSection, new List<string> { "COURSESECTIONID" }, "COURSESECTIONID");
            }
            else
            {
                //Update
                TBL_COURSEANDSECTION CourseSection = SelectedCourseSection;
                TBL_COURSEANDSECTION filter = new TBL_COURSEANDSECTION();
                filter.COURSESECTIONID = CourseSection.COURSESECTIONID;
                CourseSection.COURSESECTIONID = new Guid();
                result = data.Update(CourseSection, filter, new Guid(this.Session["ADMINID"].ToString()));
            }
            if (result != "Success")
            {
                Guid x;
                if (!Guid.TryParse(result, out x)) // check if return value is not UID
                {
                    ErrorMessage = result;
                    holdCourseSection = SelectedCourseSection as TBL_COURSEANDSECTION;
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            ErrorMessage = string.Empty;
            holdCourseSection = new TBL_COURSEANDSECTION();
            return RedirectToAction("ListScreen");
        }
    }
}