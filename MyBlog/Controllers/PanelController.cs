using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    [Authorize]
    public class PanelController:Controller
    {
        public PanelController()
        {

        }
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
