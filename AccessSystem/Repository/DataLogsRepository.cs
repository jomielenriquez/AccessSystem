using AccessSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessSystem.Repository
{
    public class DataLogsRepository
    {
        public static IEnumerable<TBL_LOG> GetAllLogs()
        {
            AccessSystemDBEntities entities = new AccessSystemDBEntities();

            var Logs = from entLogs in entities.TBL_LOG select entLogs;

            return (IEnumerable<TBL_LOG>)Logs;
        }

        public static TBL_LOG GetLog(Guid? Logid)
        {
            AccessSystemDBEntities entities = new AccessSystemDBEntities();

            var Logs = (from entLogs in entities.TBL_LOG.Where(log => log.LOGID == Logid) select entLogs).FirstOrDefault();

            return (TBL_LOG)Logs;
        }
    }
}