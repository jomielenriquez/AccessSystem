using AccessSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessSystem.Repository
{
    public class ComputerRepository
    {
        public static IEnumerable<TBL_COMPUTER> GetComputers()
        {
            AccessSystemDBEntities entities = new AccessSystemDBEntities();

            var computer = from entComputer in entities.TBL_COMPUTER select entComputer;

            return (IEnumerable<TBL_COMPUTER>)computer;
        }

        public static TBL_COMPUTER GetComputerInfo(Guid? computerID)
        {
            AccessSystemDBEntities entities = new AccessSystemDBEntities();

            var computer = (from entComputer in entities.TBL_COMPUTER.Where(com => com.COMPUTERID == computerID) select entComputer).FirstOrDefault();

            return (TBL_COMPUTER)computer;
        }
    }
}