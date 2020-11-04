using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment1.Controllers
{
    public class TracksController : Controller
    {
        private Manager m = new Manager();

        // GET: Tracks
        public ActionResult Index()
        {
            return View(m.TrackGetAll());
        }

        public ActionResult Jazz()
        {
            return View("Index", m.TrackGetAllJazz());
        }

        public ActionResult RogerGlover()
        {
            return View("Index", m.TrackGetAllRogerGlover());
        }

        public ActionResult Longest()
        {
            return View("Index", m.TrackGetAllTop50Longest());
        }

    }
}
