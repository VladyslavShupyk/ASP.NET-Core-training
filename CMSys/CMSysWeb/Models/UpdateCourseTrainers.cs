using CMSys.Core.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSysWeb.Models
{
    public class UpdateCourseTrainers
    {
        public Course Course { get; set; }
        public IEnumerable<Trainer> Trainers {get; set;}
    }
}
