using Assignment2.Models;
using System.Web.Mvc;

namespace Assignment2.Controllers
{

    public class TrackController : Controller
    {
        private Manager m = new Manager();

        // GET: Track
        public ActionResult Index()
        {
            return View(m.TrackGetAllWithDetail());
        }

        // GET: Track/Details/5
        public ActionResult Details(int? id)
        {
            var obj = m.TrackGetByIdWithDetail(id.GetValueOrDefault());

            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(obj);
            }
        }

        // GET: Track/Create
        public ActionResult Create()
        {

            var form = new TrackAddFormViewModel();

            form.MediaTypeList = new SelectList(m.MediaTypeGetAll(), "MediaTypeId", "Name");

            form.AlbumList = new SelectList(m.AlbumGetAll(), "AlbumId", "Title");

            return View(form);
        }


        // POST: Track/Create
        [HttpPost]
        public ActionResult Create(TrackAddViewModel item)
        {

            if (!ModelState.IsValid)
            {
                return View(item);
            }

            var addedItem = m.TrackAdd(item);

            if (addedItem == null)
            {
                return View(item);
            }
            else
            {
                return RedirectToAction("details", new { id = addedItem.TrackId });
            }
        }
    }
}
