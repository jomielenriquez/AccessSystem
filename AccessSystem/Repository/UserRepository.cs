using AccessSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessSystem.Repository
{
    public class UserRepository
    {
        public static IEnumerable<TBL_NAMES> GetAllUsers()
        {
            AccessSystemDBEntities entities = new AccessSystemDBEntities();

            var Users = from entUsers in entities.TBL_NAMES select entUsers;

            return (IEnumerable<TBL_NAMES>)Users;
        }

        public static TBL_NAMES GetUser(Guid? NameID)
        {
            AccessSystemDBEntities entities = new AccessSystemDBEntities();

            var Users = (from entUsers in entities.TBL_NAMES.Where(name => name.NAMEID == NameID) select entUsers).FirstOrDefault();

            return (TBL_NAMES)Users;
        }
    }
}