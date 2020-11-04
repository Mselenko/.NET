using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment2.Controllers
{
    public class MediaTypeController : Controller
    {
        private Manager m = new Manager();

        // GET: MediaType
        public ActionResult Index()
        {
            return View(m.MediaTypeGetAll());
        }

        // GET: MediaType/Details/5
        public ActionResult Details(int? id)
        {
            var obj = m.MediaTypeGetById(id.GetValueOrDefault());

            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(obj);
            }
        }

    }
}
