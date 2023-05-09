using AccessSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessSystem.Repository
{
    public class CourseSectionRepository
    {
        public static IEnumerable<TBL_COURSEANDSECTION> GetAllCourseSection()
        {
            AccessSystemDBEntities entities = new AccessSystemDBEntities();

            var courseSection = from entCourseSection in entities.TBL_COURSEANDSECTION select entCourseSection;

            return (IEnumerable<TBL_COURSEANDSECTION>)courseSection;

        }

        public static TBL_COURSEANDSECTION GetAllCourseSectionWithID(Guid? CourseSectionID)
        {
            AccessSystemDBEntities entities = new AccessSystemDBEntities();

            var courseSection = (from entCourseSection in entities.TBL_COURSEANDSECTION.Where(CAS => CAS.COURSESECTIONID == CourseSectionID) select entCourseSection).FirstOrDefault();

            return (TBL_COURSEANDSECTION)courseSection;

        }
    }
}