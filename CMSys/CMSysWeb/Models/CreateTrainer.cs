using CMSys.Core.Entities.Catalog;
using CMSys.Core.Entities.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSysWeb.Models
{
    public class CreateTrainer
    {
        public IEnumerable<Trainer> Trainers { get; set; }
        public IEnumerable<TrainerGroup> TrainerGroups { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
