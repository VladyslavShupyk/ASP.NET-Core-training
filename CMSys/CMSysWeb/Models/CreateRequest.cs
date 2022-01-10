using CMSys.Core.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSysWeb.Models
{
    public class CreateRequest
    {
        public IEnumerable<CourseGroup> CourseGroups { get; set; }
        public IEnumerable<TrainerGroup> TrainerGroups { get; set; }
        public string Status { get; set; }
    }
}
