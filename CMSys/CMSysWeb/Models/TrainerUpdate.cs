using CMSys.Core.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSysWeb.Models
{
    public class TrainerUpdate
    {
        public IEnumerable<TrainerGroup> TrainerGroups { get; set; }
        public Trainer Trainer { get; set; }
    }
}
