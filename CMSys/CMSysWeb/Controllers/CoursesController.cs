using CMSys.Common.Paging;
using CMSys.Core.Entities.Catalog;
using CMSys.Core.Entities.Membership;
using CMSys.Core.Repositories.Catalog;
using CMSys.Core.Repositories.Membership;
using CMSys.Infrastructure;
using CMSysWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSysWeb.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        const int ITEMS_PER_PAGE = 5;
        UnitOfWorkOptions unitOfWorkOptions;
        UnitOfWork unitOfWork;
        public CoursesController(IConfiguration configuration)
        {
            unitOfWorkOptions = new UnitOfWorkOptions()
            {
                ConnectionString = configuration.GetConnectionString("CMSys")
            };
            unitOfWork = new UnitOfWork(unitOfWorkOptions);
        }
        [Route("/courses")]
        [Route("/courses/page/{page:int}/{groupId?}/{typeId?}")]
        public IActionResult Courses(int page = 1, string groupId = null, string typeId = null)
        {
            if (groupId == "null" || groupId == String.Empty)
                groupId = null;
            if(typeId == "null" || typeId == String.Empty)
                typeId = null;
            ICourseRepository courseRep = unitOfWork.CourseRepository;
            IEnumerable<Course> courses = courseRep.All();
            PageInfo pageInfo = new PageInfo(page, ITEMS_PER_PAGE);
            PagedList<Course> coursesPageList = courseRep.GetPagedList(pageInfo);
            if (groupId != null && typeId != null)
            {
                coursesPageList = courseRep.GetPagedList(pageInfo, predicate => predicate.CourseTypeId == new Guid(typeId) && predicate.CourseGroupId == new Guid(groupId));
            }
            else if(groupId != null && typeId == null)
            {
                coursesPageList = courseRep.GetPagedList(pageInfo, predicate => predicate.CourseGroupId == new Guid(groupId));
            }
            else if(groupId == null && typeId != null)
            {
                coursesPageList = courseRep.GetPagedList(pageInfo, predicate => predicate.CourseTypeId == new Guid(typeId));
            }
            ICourseTypeRepository courseTypeRep = unitOfWork.CourseTypeRepository;
            IEnumerable<CourseType> courseTypes = courseTypeRep.All();
            ICourseGroupRepository courseGroupRep = unitOfWork.CourseGroupRepository;
            IEnumerable<CourseGroup> courseGroups = courseGroupRep.All();
            CoursesTypesGroups ctg = new CoursesTypesGroups() 
            { 
                CoursesPagedList = coursesPageList,
                GroupIdFiler = groupId,
                TypeIdFiler = typeId,
                CoursesGroups = courseGroups, 
                CoursesTypes = courseTypes 
            };
            return View(ctg);
        }
        [Route("/admin/courses")]
        public IActionResult CoursesList()
        {
            if(User.IsInRole("Admin"))
            {
                ICourseRepository courseRep = unitOfWork.CourseRepository;
                IEnumerable<Course> courses = courseRep.All();
                ICourseTypeRepository courseTypeRep = unitOfWork.CourseTypeRepository;
                IEnumerable<CourseType> courseTypes = courseTypeRep.All();
                ICourseGroupRepository courseGroupRep = unitOfWork.CourseGroupRepository;
                IEnumerable<CourseGroup> courseGroups = courseGroupRep.All();
                CoursesTypesGroups ctg = new CoursesTypesGroups()
                {
                    Courses = courses,
                    CoursesGroups = courseGroups,
                    CoursesTypes = courseTypes
                };
                return View(ctg);
            }
            return RedirectToRoute(new { controller = "Error", action = "ErrorPage" });
        }
        [HttpGet]
        [Route("/admin/courses/create")]
        public IActionResult CreateCourse()
        {
            if (User.IsInRole("Admin"))
            {
                ICourseRepository courseRep = unitOfWork.CourseRepository;
                IEnumerable<Course> courses = courseRep.All();
                ICourseTypeRepository courseTypeRep = unitOfWork.CourseTypeRepository;
                IEnumerable<CourseType> courseTypes = courseTypeRep.All();
                ICourseGroupRepository courseGroupRep = unitOfWork.CourseGroupRepository;
                IEnumerable<CourseGroup> courseGroups = courseGroupRep.All();
                CoursesTypesGroups ctg = new CoursesTypesGroups()
                {
                    Courses = courses,
                    CoursesGroups = courseGroups,
                    CoursesTypes = courseTypes
                };
                return View(ctg);
            }
            return RedirectToRoute(new { controller = "Error", action = "ErrorPage" });
        }
        [HttpGet]
        [Route("/admin/courses/update/{id}")]
        public IActionResult UpdateCourse(string id)
        {
            if (User.IsInRole("Admin"))
            {
                Guid courseGuid = new Guid(id);
                ICourseRepository courseRep = unitOfWork.CourseRepository;
                Course course = courseRep.Find(courseGuid);
                ICourseTypeRepository courseTypeRep = unitOfWork.CourseTypeRepository;
                IEnumerable<CourseType> courseTypes = courseTypeRep.All();
                ICourseGroupRepository courseGroupRep = unitOfWork.CourseGroupRepository;
                IEnumerable<CourseGroup> courseGroups = courseGroupRep.All();
                CourseUpdate courseUpdate = new CourseUpdate()
                {
                    Course = course,
                    CoursesGroups = courseGroups,
                    CoursesTypes = courseTypes
                };
                return View(courseUpdate);
            }
            return RedirectToRoute(new { controller = "Error", action = "ErrorPage" });
        }
        [HttpGet]
        [Route("/admin/courses/trainers/{id}")]
        public IActionResult UpdateCourseTrainers(string id)
        {
            if (User.IsInRole("Admin"))
            {
                Guid courseGuid = new Guid(id);
                ICourseRepository courseRep = unitOfWork.CourseRepository;
                Course course = courseRep.Find(courseGuid);
                ITrainerRepository trainerRep = unitOfWork.TrainerRepository;
                IEnumerable<Trainer> trainers = trainerRep.All();
                UpdateCourseTrainers updateCourseTrainers = new Models.UpdateCourseTrainers()
                {
                    Course = course,
                    Trainers = trainers
                };
                return View(updateCourseTrainers);
            }
            return RedirectToRoute(new { controller = "Error", action = "ErrorPage" });
        }
        [Route ("/admin/coursegroups")]
        public IActionResult CourseGroups()
        {
            if (User.IsInRole("Admin"))
            {
                ICourseGroupRepository courseGroupRepository = unitOfWork.CourseGroupRepository;
                IEnumerable<CourseGroup> courseGroups = courseGroupRepository.All();
                return View(courseGroups);
            }
            return RedirectToRoute(new { controller = "Error", action = "ErrorPage" });
        }

        [HttpGet]
        [Route("/courses/{id}")]
        public IActionResult SearchCourse(string id)
        {
            Guid guid = new Guid(id);
            ICourseRepository courseRep = unitOfWork.CourseRepository;
            Course course = courseRep.Find(guid);
            return View(course);
        }
        
        [HttpPost]
        public JsonResult Create(string name,bool isNew,string groupId, string typeId, int order, string description)
        {
            Guid guidGroup = new Guid(groupId);
            Guid guidType = new Guid(typeId);
            ICourseRepository courseRep = unitOfWork.CourseRepository;
            ICourseGroupRepository courseGroupRep = unitOfWork.CourseGroupRepository;
            ICourseTypeRepository courseTypeRep = unitOfWork.CourseTypeRepository;
            CourseGroup courseGroup = courseGroupRep.Find(guidGroup);
            CourseType courseType = courseTypeRep.Find(guidType);
            courseRep.Add(new Course()
            {
                Id = new Guid(),
                CourseGroup = courseGroup,
                CourseType = courseType,
                IsNew = isNew,
                CourseGroupId = guidGroup,
                CourseTypeId = guidType,
                Description = description,
                Name = name,
                VisualOrder = order
            });
            unitOfWork.Commit();
            RedirectModel redirect = new RedirectModel() { RedirectUrl = "/admin/courses" };
            return Json(redirect);
        }
        [HttpPost]
        public JsonResult Delete(string id)
        {
            Guid guidCourse = new Guid(id);
            ICourseRepository courseRep = unitOfWork.CourseRepository;
            Course course = courseRep.Find(guidCourse);
            courseRep.Remove(course);
            unitOfWork.Commit();
            RedirectModel redirect = new RedirectModel() { RedirectUrl = "/admin/courses" };
            return Json(redirect);
        }
        [HttpPost]
        public JsonResult Update(string id, string name, bool isNew, string groupId, string typeId, int order, string description)
        {
            Guid CourseId = new Guid(id);
            Guid courseGroupId = new Guid(groupId);
            Guid courseTypeId = new Guid(typeId);
            ICourseRepository courseRep = unitOfWork.CourseRepository;
            Course course = courseRep.Find(CourseId);
            ICourseGroupRepository courseGroupRep = unitOfWork.CourseGroupRepository;
            CourseGroup courseGroup = courseGroupRep.Find(courseGroupId);
            ICourseTypeRepository courseTypeRep = unitOfWork.CourseTypeRepository;
            CourseType courseType = courseTypeRep.Find(courseTypeId);
            course.IsNew = isNew;
            course.Name = name;
            course.VisualOrder = order;
            course.CourseGroupId = courseGroupId;
            course.CourseTypeId = courseTypeId;
            course.Description = description;
            course.CourseType = courseType;
            course.CourseGroup = courseGroup;
            unitOfWork.Commit();
            RedirectModel redirect = new RedirectModel() { RedirectUrl = "/admin/courses" };
            return Json(redirect);
        }
        [HttpPost]
        public JsonResult DeleteCourseTrainer(string courseId, string trainerId)
        {
            Guid courseIdGuid = new Guid(courseId);
            Guid trainerIdGuid = new Guid(trainerId);
            ICourseRepository courseRep = unitOfWork.CourseRepository;
            Course course = courseRep.Find(courseIdGuid);
            ICourseTrainerRepository courseTrainerRepository = unitOfWork.CourseTrainerRepository;
            CourseTrainer courseTrainer = courseTrainerRepository.Find(courseIdGuid, trainerIdGuid);
            course.Trainers.Remove(courseTrainer);
            unitOfWork.Commit();
            RedirectModel redirect = new RedirectModel() { RedirectUrl = "/admin/courses/trainers/" + courseId};
            return Json(redirect);
        }

        [HttpPost]
        public JsonResult AddCourseTrainer(string courseId, string trainerId)
        {
            Guid courseIdGuid = new Guid(courseId);
            Guid trainerIdGuid = new Guid(trainerId);
            ICourseRepository courseRep = unitOfWork.CourseRepository;
            Course course = courseRep.Find(courseIdGuid);
            ITrainerRepository trainerRepository = unitOfWork.TrainerRepository;
            Trainer trainer = trainerRepository.Find(trainerIdGuid);
            CourseTrainer courseTrainer = new CourseTrainer() { CourseId = courseIdGuid, TrainerId = trainerIdGuid, Trainer = trainer, VisualOrder = trainer.VisualOrder };
            course.Trainers.Add(courseTrainer);
            unitOfWork.Commit();
            RedirectModel redirect = new RedirectModel() { RedirectUrl = "/admin/courses/trainers/" + courseId };
            return Json(redirect);
        }
        [HttpPost]
        public JsonResult CreateCourseGroup(string name, int order, string description)
        {
            ICourseGroupRepository courseGroupRepository = unitOfWork.CourseGroupRepository;
            IEnumerable<CourseGroup> courseGroups = courseGroupRepository.All();
            CreateRequest request = new CreateRequest();
            foreach(var group in courseGroups)
            {
                if(group.Name == name)
                {
                    request.Status = "Group with this name already created.";
                    return Json(request);
                }
            }
            CourseGroup courseGroup = new CourseGroup()
            {
                Id = new Guid(),
                Name = name,
                VisualOrder = order,
                Description = description
            };
            courseGroupRepository.Add(courseGroup);
            unitOfWork.Commit();
            IEnumerable<CourseGroup> responseCourseGroups = courseGroupRepository.All();
            request.Status = "Success";
            request.CourseGroups = responseCourseGroups;
            return Json(request);
        }
        [HttpPost]
        public JsonResult DeleteCourseGroup(string id)
        {
            Guid courseGroupGuid = new Guid(id);
            ICourseGroupRepository courseGroupRepository = unitOfWork.CourseGroupRepository;
            CourseGroup courseGroup = courseGroupRepository.Find(courseGroupGuid);

            ICourseRepository courseRepository = unitOfWork.CourseRepository;
            IEnumerable<Course> courses = courseRepository.All();
            DeleteRequest deleteRequest = new DeleteRequest();
            foreach(var course in courses)
            {
                if(course.CourseGroupId == courseGroup.Id)
                {
                    deleteRequest.Status = "Group have dependent items.";
                    return Json(deleteRequest);
                }
            }
            courseGroupRepository.Remove(courseGroup);
            unitOfWork.Commit();
            IEnumerable<CourseGroup> responseCourseGroups = courseGroupRepository.All();
            deleteRequest.Status = "Success";
            deleteRequest.CourseGroups = responseCourseGroups;
            return Json(deleteRequest);
        }
        [HttpPost]
        public JsonResult EditCourseGroup(string id)
        {
            Guid courseGroupGuid = new Guid(id);
            ICourseGroupRepository courseGroupRepository = unitOfWork.CourseGroupRepository;
            CourseGroup courseGroup = courseGroupRepository.Find(courseGroupGuid);
            return Json(courseGroup);
        }
        [HttpPost]
        public JsonResult UpdateCourseGroup(string id, string name, int order, string description)
        {
            Guid groupGuid = new Guid(id);
            ICourseGroupRepository courseGroupRepository = unitOfWork.CourseGroupRepository;
            IEnumerable<CourseGroup> courseGroups = courseGroupRepository.All();
            UpdateStatusCourseGroups status = new UpdateStatusCourseGroups();
            foreach(var group in courseGroups)
            {
                if(group.Name == name && group.Id != groupGuid)
                {
                    status.Status = "Some group already have this name.";
                    return Json(status);
                }
            }
            CourseGroup courseGroup = courseGroupRepository.Find(groupGuid);
            courseGroup.Name = name;
            courseGroup.VisualOrder = order;
            courseGroup.Description = description;
            unitOfWork.Commit();
            IEnumerable<CourseGroup> responseCourseGroups = courseGroupRepository.All();
            status.Status = "Success";
            status.CourseGroups = responseCourseGroups;
            return Json(status);
        }
    }
}
