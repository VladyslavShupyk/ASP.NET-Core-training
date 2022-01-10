using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSysWeb.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error/")]
        public IActionResult ErrorPage()
        {
            return View();
        }
    }
}
