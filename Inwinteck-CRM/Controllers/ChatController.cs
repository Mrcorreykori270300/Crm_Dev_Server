using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inwinteck_CRM.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChatRegister()
        {
            return View();
        }
        public ActionResult ChatLogin()
        {
            return View();
        }

        public ActionResult ChatMessage()  
        {
            return View();
        }
       
    }
}