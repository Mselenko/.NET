using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment5.Controllers
{
    public class MediaItemController : Controller
    {
        private Manager m = new Manager();

        public ActionResult Index()
        {
            return View("Index", "Home");
        }

        // GET: mediaItem/Details/5
        [Route("media/{id}")]
        public ActionResult Details(int? id)
        {
            var o = m.MediaGetById(id.GetValueOrDefault());

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                return File(o.Content, o.ContentType);
            }
        }
    }
}
