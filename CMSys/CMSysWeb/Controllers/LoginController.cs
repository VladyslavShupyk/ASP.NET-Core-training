using CMSys.Core.Entities.Membership;
using CMSys.Core.Repositories.Membership;
using CMSys.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CMSysWeb.Controllers
{
    public class LoginController : Controller
    {
        UnitOfWorkOptions unitOfWorkOptions;
        UnitOfWork unitOfWork;
        public LoginController(IConfiguration configuration)
        {
            unitOfWorkOptions = new UnitOfWorkOptions()
            {
                ConnectionString = configuration.GetConnectionString("CMSys")
            };
            unitOfWork = new UnitOfWork(unitOfWorkOptions);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logining(string email, string pass)
        {
            if(email == null || pass == null)
                return RedirectToAction("Login");
            IUserRepository userRepository = unitOfWork.UserRepository;
            User user = userRepository.FindByEmail(email);
            if(user != null)
            {
                if(user.VerifyPassword(pass))
                {
                    var claims = user.GetClaims();
                    var identity = new ClaimsIdentity(claims, "Basic");
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync("Cookies", principal);
                    return RedirectToRoute(new { controller = "Courses", action = "Courses" });
                }
            }
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Logoff()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login");
        }
    }
}
