using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment5.Controllers
{
    public class AudioController : Controller
    {
        Manager m = new Manager();

        public ActionResult Index()
        {
            return View("index", "home");
        }


        [Route("clip/{id}")]
        public ActionResult Details(int? id)
        {
            var o = m.TrackAudioGetById(id.GetValueOrDefault());

            if (o == null)
            {
                return HttpNotFound();
            }
            else if (o.Audio == null || o.AudioContentType == null)
            {
                return null;
            }
            else
            {
                return File(o.Audio, o.AudioContentType);
            }
        }

    }
}
