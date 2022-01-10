using CMSys.Core.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSysWeb.Models
{
    public class UpdateStatusCourseGroups
    {
        public IEnumerable<CourseGroup> CourseGroups { get; set; }
        public string Status { get; set; }
    }
}
