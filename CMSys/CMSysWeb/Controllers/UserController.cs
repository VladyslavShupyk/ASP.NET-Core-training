using Microsoft.AspNetCore.Mvc;
using CMSys.Core.Repositories.Membership;
using CMSys.Infrastructure;
using System.Collections.Generic;
using CMSys.Core.Entities.Membership;
using System;
using CMSys.Common.Paging;
using CMSysWeb.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace CMSysWeb.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        const int ITEMS_PER_PAGE = 10;
        UnitOfWorkOptions unitOfWorkOptions;
        UnitOfWork unitOfWork;
        public UserController(IConfiguration configuration)
        {

            unitOfWorkOptions = new UnitOfWorkOptions()
            {
                ConnectionString = configuration.GetConnectionString("CMSys")
            };
            unitOfWork = new UnitOfWork(unitOfWorkOptions);
        }
        [HttpGet]
        [Route("admin/users")]
        [Route("admin/users/page/{number:int}/{search?}")]
        public IActionResult ShowUsers(int number = 1, string search = null)
        {
            if (User.IsInRole("Admin"))
            {
                IUserRepository userRepository = unitOfWork.UserRepository;
                PageInfo pageInfo = new PageInfo(number, ITEMS_PER_PAGE);
                PagedList<User> pageUsers = userRepository.GetPagedList(pageInfo,search);
                SearchUserModel searchUserModel = new SearchUserModel { Users = pageUsers, Search = search };
                return View(searchUserModel);
            }
            return RedirectToRoute(new { controller = "Courses", action = "Courses" });
        }
        [Route ("admin/users/{id}")]
        public IActionResult ShowUserInfo(string id)
        {
            if (User.IsInRole("Admin"))
            {
                Guid userGuid = new Guid(id);
                IUserRepository userRepository = unitOfWork.UserRepository;
                User user = userRepository.Find(userGuid);
                return View(user);
            }
            return RedirectToRoute(new { controller = "Courses", action = "Courses" });
        }
        [Route("admin/users/update/{id}")]
        public IActionResult UpdateUser(string id)
        {
            if (User.IsInRole("Admin"))
            {
                Guid userGuid = new Guid(id);
                IUserRepository userRepository = unitOfWork.UserRepository;
                User user = userRepository.Find(userGuid);
                IRoleRepository roleRepository = unitOfWork.RoleRepository;
                IEnumerable<Role> roles = roleRepository.All();
                UpdateUserRoles updateUserRoles = new UpdateUserRoles() { Roles = roles, User = user };
                return View(updateUserRoles);
            }
            return RedirectToRoute(new { controller = "Courses", action = "Courses" });
        }
        [HttpPost]
        public JsonResult AddRole(string id, string roleId)
        {
            Guid userGuid = new Guid(id);
            Guid roleGuid = new Guid(roleId);
            IUserRepository userRepository = unitOfWork.UserRepository;
            User user = userRepository.Find(userGuid);
            IRoleRepository roleRepository = unitOfWork.RoleRepository;
            Role role = roleRepository.Find(roleGuid);
            user.Roles.Add(role);
            IEnumerable<Role> roles = roleRepository.All();
            List<RoleForResponse> allRoles = new List<RoleForResponse>();
            foreach (var item in roles)
            {
                RoleForResponse roleForResponse = new RoleForResponse() { Id = item.Id, Name = item.Name };
                allRoles.Add(roleForResponse);
            }
            List<RoleForResponse> userRoles = new List<RoleForResponse>();
            foreach (var userRole in user.Roles)
            {
                RoleForResponse roleForResponse = new RoleForResponse() { Id = userRole.Id, Name = userRole.Name };
                userRoles.Add(roleForResponse);
            }
            List<RoleForResponse> finalRoles = new List<RoleForResponse>();
            foreach (var item in allRoles)
            {
                if (!item.Check(userRoles))
                    finalRoles.Add(new RoleForResponse() { Name = item.Name, Id = item.Id });
            }
            //all good
            List<RoleForResponse> roleFors = new List<RoleForResponse>();
            foreach(var rolee in user.Roles)
            {
                RoleForResponse roleForResponse = new RoleForResponse() { Id = rolee.Id, Name = rolee.Name };
                roleFors.Add(roleForResponse);
            }
            RoleResponse response = new RoleResponse() { UserRoles = roleFors , Roles = finalRoles };
            unitOfWork.Commit();
            return Json(response);
        }
        [HttpPost]
        public JsonResult DeleteRole(string id, string roleId)
        {
            Guid userGuid = new Guid(id);
            Guid roleGuid = new Guid(roleId);
            IUserRepository userRepository = unitOfWork.UserRepository;
            User user = userRepository.Find(userGuid);
            IRoleRepository roleRepository = unitOfWork.RoleRepository;
            Role role = roleRepository.Find(roleGuid);
            user.Roles.Remove(role);
            IEnumerable<Role> roles = roleRepository.All();
            List<RoleForResponse> allRoles = new List<RoleForResponse>();
            foreach (var item in roles)
            {
                RoleForResponse roleForResponse = new RoleForResponse() { Id = item.Id, Name = item.Name };
                allRoles.Add(roleForResponse);
            }
            List<RoleForResponse> userRoles = new List<RoleForResponse>();
            foreach (var userRole in user.Roles)
            {
                RoleForResponse roleForResponse = new RoleForResponse() { Id = userRole.Id, Name = userRole.Name };
                userRoles.Add(roleForResponse);
            }
            List<RoleForResponse> finalRoles = new List<RoleForResponse>();
            foreach (var item in allRoles)
            {
                if (!item.Check(userRoles))
                    finalRoles.Add(new RoleForResponse() { Name = item.Name, Id = item.Id });
            }

            //all good
            List<RoleForResponse> roleFors = new List<RoleForResponse>();
            foreach (var rolee in user.Roles)
            {
                RoleForResponse roleForResponse = new RoleForResponse() { Id = rolee.Id, Name = rolee.Name };
                roleFors.Add(roleForResponse);
            }
            RoleResponse response = new RoleResponse() { UserRoles = roleFors, Roles = finalRoles };
            unitOfWork.Commit();
            return Json(response);
        }
        [HttpPost]
        public JsonResult ChangePassword(string Id, string newPassword)
        {
            Guid userGuid = new Guid(Id);
            IUserRepository userRepository = unitOfWork.UserRepository;
            User user = userRepository.Find(userGuid);
            user.ChangePassword(newPassword);
            unitOfWork.Commit();
            var response = new { Response = "Success" };
            return Json(response);
        }
    }
}
