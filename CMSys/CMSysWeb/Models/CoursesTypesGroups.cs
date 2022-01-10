using CMSys.Common.Paging;
using CMSys.Core.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSysWeb.Models
{
    public class CoursesTypesGroups
    {
        public string UserName { get; set; }
        public IEnumerable<CourseGroup> CoursesGroups { get; set; }
        public IEnumerable<CourseType> CoursesTypes { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public PagedList<Course> CoursesPagedList { get; set; }
        public string GroupIdFiler { get; set; }
        public string TypeIdFiler { get; set; }
    }
}
