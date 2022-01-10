using CMSys.Core.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSysWeb.Models
{
    public class CourseUpdate
    {
        public IEnumerable<CourseGroup> CoursesGroups { get; set; }
        public IEnumerable<CourseType> CoursesTypes { get; set; }
        public Course Course { get; set; }
    }
}
