using CMSys.Core.Entities.Catalog;
using CMSys.Core.Repositories.Catalog;
using CMSys.Core.Repositories.Membership;
using CMSys.Core.Entities.Membership;
using CMSys.Infrastructure;
using CMSysWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace CMSysWeb.Controllers
{
    [Authorize]
    public class TrainersController : Controller
    {
        UnitOfWorkOptions unitOfWorkOptions;
        UnitOfWork unitOfWork;
        public TrainersController(IConfiguration configuration)
        {
            unitOfWorkOptions = new UnitOfWorkOptions()
            {
                ConnectionString = configuration.GetConnectionString("CMSys")
            };
            unitOfWork = new UnitOfWork(unitOfWorkOptions);
        }

        [Route("trainers/")]
        public IActionResult ShowTrainers()
        {
            ITrainerRepository trainerRep = unitOfWork.TrainerRepository;
            IEnumerable<Trainer> trainers = trainerRep.All();
            ITrainerGroupRepository trGroup = unitOfWork.TrainerGroupRepository;
            IEnumerable<TrainerGroup> trainerGroups = trGroup.All();
            TrainersWithTrainerGroups trwtg = new TrainersWithTrainerGroups() { Trainers = trainers, TrainerGroups = trainerGroups };
            return View(trwtg);
        }
        [Route("admin/trainers")]
        public IActionResult TrainerList()
        {
            if(User.IsInRole("Admin"))
            {
                ITrainerRepository trainerRepository = unitOfWork.TrainerRepository;
                IEnumerable<Trainer> trainers = trainerRepository.All();
                return View(trainers);
            }
            return RedirectToRoute(new { controller = "Error", action = "ErrorPage" });
        }
        [Route("/admin/trainers/create")]
        public IActionResult CreateTrainer()
        {
            if (User.IsInRole("Admin"))
            {
                ITrainerGroupRepository trainerGroupRepository = unitOfWork.TrainerGroupRepository;
                IEnumerable<TrainerGroup> trainerGroups = trainerGroupRepository.All();
                IUserRepository userRepository = unitOfWork.UserRepository;
                IEnumerable<User> users = userRepository.All();
                ITrainerRepository trainerRepository = unitOfWork.TrainerRepository;
                IEnumerable<Trainer> trainers = trainerRepository.All();
                List<User> usersTrainers = new List<User>();
                foreach (var user in trainers)
                    usersTrainers.Add(user.User);
                List<User> userForModel = new List<User>();
                foreach (var user in users)
                {
                    if (!usersTrainers.Contains(user))
                        userForModel.Add(user);
                }
                CreateTrainer createTrainer = new CreateTrainer() { TrainerGroups = trainerGroups, Users = userForModel, Trainers = trainers };
                return View(createTrainer);
            }
            return RedirectToRoute(new { controller = "Error", action = "ErrorPage" });
        }
        [Route("/admin/trainers/update/{id}")]
        public IActionResult UpdateTrainer(string id)
        {
            if (User.IsInRole("Admin"))
            {
                Guid trainerGuid = new Guid(id);
                ITrainerRepository courseRep = unitOfWork.TrainerRepository;
                Trainer trainer = courseRep.Find(trainerGuid);
                ITrainerGroupRepository trainerGroupRep = unitOfWork.TrainerGroupRepository;
                IEnumerable<TrainerGroup> trainerGroups = trainerGroupRep.All();
                TrainerUpdate trainerUpdate = new TrainerUpdate() { Trainer = trainer, TrainerGroups = trainerGroups };
                return View(trainerUpdate);
            }
            return RedirectToRoute(new { controller = "Error", action = "ErrorPage" });
        }
        [Route("/admin/trainergroups")]
        public IActionResult TrainerGroups()
        {
            if (User.IsInRole("Admin"))
            {
                ITrainerGroupRepository trainerGroupRepository = unitOfWork.TrainerGroupRepository;
                IEnumerable<TrainerGroup> trainerGroups = trainerGroupRepository.All();
                return View(trainerGroups);
            }
            return RedirectToRoute(new { controller = "Error", action = "ErrorPage" });
        }
        [HttpPost]
        public JsonResult CreateTrainerGroup(string name, int order, string description)
        {
            ITrainerGroupRepository trainerGroupRepository = unitOfWork.TrainerGroupRepository;
            IEnumerable<TrainerGroup> trainerGroups = trainerGroupRepository.All();
            CreateRequest request = new CreateRequest();
            foreach (var group in trainerGroups)
            {
                if (group.Name == name)
                {
                    request.Status = "Group with this name already created.";
                    return Json(request);
                }
            }
            TrainerGroup trainerGroup = new TrainerGroup()
            {
                Id = new Guid(),
                Name = name,
                VisualOrder = order,
                Description = description
            };
            trainerGroupRepository.Add(trainerGroup);
            unitOfWork.Commit();
            IEnumerable<TrainerGroup> responseTrainerGroups = trainerGroupRepository.All();
            request.Status = "Success";
            request.TrainerGroups = responseTrainerGroups;
            return Json(request);
        }
        [HttpPost]
        public JsonResult DeleteTrainerGroup(string id)
        {
            Guid trainerGroupGuid = new Guid(id);
            ITrainerGroupRepository trainerGroupRepository = unitOfWork.TrainerGroupRepository;
            TrainerGroup trainerGroup = trainerGroupRepository.Find(trainerGroupGuid);

            ITrainerRepository trainerRepository = unitOfWork.TrainerRepository;
            IEnumerable<Trainer> trainers = trainerRepository.All();
            DeleteRequest deleteRequest = new DeleteRequest();
            foreach (var trainer in trainers)
            {
                if (trainer.TrainerGroupId == trainerGroup.Id)
                {
                    deleteRequest.Status = "Group have dependent items.";
                    return Json(deleteRequest);
                }
            }
            trainerGroupRepository.Remove(trainerGroup);
            unitOfWork.Commit();
            IEnumerable<TrainerGroup> responseTrainerGroups = trainerGroupRepository.All();
            deleteRequest.Status = "Success";
            deleteRequest.TrainerGroups = responseTrainerGroups;
            return Json(deleteRequest);
        }
        [HttpPost]
        public JsonResult EditTrainerGroup(string id)
        {
            Guid trainerGroupGuid = new Guid(id);
            ITrainerGroupRepository trainerGroupRepository = unitOfWork.TrainerGroupRepository;
            TrainerGroup trainerGroup = trainerGroupRepository.Find(trainerGroupGuid);
            return Json(trainerGroup);
        }
        [HttpPost]
        public JsonResult UpdateTrainerGroup(string id, string name, int order, string description)
        {
            Guid groupGuid = new Guid(id);
            ITrainerGroupRepository trainerGroupRepository = unitOfWork.TrainerGroupRepository;
            IEnumerable<TrainerGroup> trainerGroups = trainerGroupRepository.All();
            UpdateStatusTrainerGroups status = new UpdateStatusTrainerGroups();
            foreach (var group in trainerGroups)
            {
                if (group.Name == name && group.Id != groupGuid)
                {
                    status.Status = "Some group already have this name.";
                    return Json(status);
                }
            }
            TrainerGroup trainerGroup = trainerGroupRepository.Find(groupGuid);
            trainerGroup.Name = name;
            trainerGroup.VisualOrder = order;
            trainerGroup.Description = description;
            unitOfWork.Commit();
            IEnumerable<TrainerGroup> responseTrainerGroups = trainerGroupRepository.All();
            status.Status = "Success";
            status.TrainerGroups = responseTrainerGroups;
            return Json(status);
        }
        [HttpPost]
        public JsonResult SearchTrainer(string id)
        {
            Guid guid = new Guid(id);
            ITrainerRepository trainerRep = unitOfWork.TrainerRepository;
            Trainer trainer = trainerRep.Find(guid);
            return Json(trainer);
        }
        [HttpPost]
        public JsonResult Create(string userId, string groupId, int order, string description)
        {
            Guid userGuid = new Guid(userId);
            Guid groupGuid = new Guid(groupId);
            IUserRepository userRepository = unitOfWork.UserRepository;
            User user = userRepository.Find(userGuid);
            ITrainerGroupRepository trainerGroupRepository = unitOfWork.TrainerGroupRepository;
            TrainerGroup trainerGroup = trainerGroupRepository.Find(groupGuid);
            Trainer trainer = new Trainer() 
            { 
                Id = new Guid(), 
                Description = description,
                VisualOrder = order, 
                User = user ,
                TrainerGroup = trainerGroup, 
                TrainerGroupId = groupGuid
            };
            ITrainerRepository trainerRepository = unitOfWork.TrainerRepository;
            trainerRepository.Add(trainer);
            unitOfWork.Commit();
            RedirectModel redirect = new RedirectModel() { RedirectUrl = "/admin/trainers" };
            return Json(redirect);
        }
        [HttpPost]
        public JsonResult Update(string trainerId, string groupId, int order, string description)
        {
            Guid trainerGuid = new Guid(trainerId);
            Guid groupGuid = new Guid(groupId);
            ITrainerGroupRepository trainerGroupRep = unitOfWork.TrainerGroupRepository;
            TrainerGroup trainerGroup = trainerGroupRep.Find(groupGuid);
            ITrainerRepository trainerRep = unitOfWork.TrainerRepository;
            Trainer trainer = trainerRep.Find(trainerGuid);
            trainer.TrainerGroup = trainerGroup;
            trainer.TrainerGroupId = groupGuid;
            trainer.VisualOrder = order;
            trainer.Description = description;
            unitOfWork.Commit();
            RedirectModel redirect = new RedirectModel() { RedirectUrl = "/admin/trainers" };
            return Json(redirect);
        }
        [HttpPost]
        public JsonResult Delete(string id)
        {
            Guid trainerGuid = new Guid(id);
            ITrainerRepository trainerRepository = unitOfWork.TrainerRepository;
            Trainer trainer = trainerRepository.Find(trainerGuid);
            trainerRepository.Remove(trainer);
            unitOfWork.Commit();
            RedirectModel redirect = new RedirectModel() { RedirectUrl = "/admin/trainers" };
            return Json(redirect);
        }
    }
}
