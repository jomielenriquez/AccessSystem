using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessSystem.Model
{
    public class AdminModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public TBL_ADMINUSER AccountInfo { get; set; }
        public TBL_ADMINUSER SelectedAccountInfo { get; set; }
        public IEnumerable<TBL_ADMINUSER> AccountInfoList { get; set; }
        public TBL_COURSEANDSECTION SelectedCourseSection { get; set; }
        public IEnumerable<TBL_COURSEANDSECTION> CourseSectionList { get; set; }
        public TBL_NAMES SelectedUser { get; set; }
        public IEnumerable<TBL_NAMES> UserList { get; set; }
        public TBL_COMPUTER SelectedComputer { get; set; }
        public IEnumerable<TBL_COMPUTER> ComputerList { get; set; }
        public TBL_LOG SelectedLog { get; set; }
        public IEnumerable<TBL_LOG> LogList { get; set; }
        public List<string> AreChecked { get; set; }
        public string ErrorMessage { get; set; }
    }
}