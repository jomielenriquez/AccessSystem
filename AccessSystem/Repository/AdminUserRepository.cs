using AccessSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccessSystem.Repository
{
    public class AdminUserRepository
    {
        public static TBL_ADMINUSER GetCurrentUserInfo(Guid adminAccountID)
        {
            AccessSystemDBEntities entities = new AccessSystemDBEntities();

            var account = (from entitiesAccount in entities.TBL_ADMINUSER.Where(acc => acc.ADMINUSERID == adminAccountID) select entitiesAccount).FirstOrDefault();

            return (TBL_ADMINUSER)account;
        }

        public static IEnumerable<TBL_ADMINUSER> GetAllAdminUsers()
        {
            AccessSystemDBEntities entities = new AccessSystemDBEntities();

            var account = from entitiesAccount in entities.TBL_ADMINUSER select entitiesAccount;

            return (IEnumerable<TBL_ADMINUSER>)account;
        }

        public static TBL_ADMINUSER GetAdminUser(Guid? AdminUserID)
        {
            AccessSystemDBEntities entities = new AccessSystemDBEntities();

            var account = (from entitiesAccount in entities.TBL_ADMINUSER.Where(acc => acc.ADMINUSERID == AdminUserID) select entitiesAccount).FirstOrDefault();

            return (TBL_ADMINUSER)account;
        }
    }
}