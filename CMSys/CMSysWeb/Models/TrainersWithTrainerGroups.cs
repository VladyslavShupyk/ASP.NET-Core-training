using CMSys.Core.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSysWeb.Models
{
    public class TrainersWithTrainerGroups
    {
        public IEnumerable<Trainer> Trainers { get; set; }
        public IEnumerable<TrainerGroup> TrainerGroups { get; set; }
    }
}
