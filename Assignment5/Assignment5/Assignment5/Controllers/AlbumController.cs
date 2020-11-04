using Assignment5.Models;
using System.Web.Mvc;

namespace Assignment5.Controllers
{
    public class AlbumController : Controller
    {

        private Manager m = new Manager();
        
        [Authorize]
        
        public ActionResult Index()
        {
            return View(m.GetAllAlbums());
        }

        // GET: Album/Details/5
        public ActionResult Details(int? id)
        {
            var obj = m.AlbumGetById(id.GetValueOrDefault());

            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(obj);
            }
        }

        [Authorize(Roles = "Clerk")]
        [Route("Album/{id}/AddTrack")]
        public ActionResult AddTrack(int? id)
        {
            var obj = m.AlbumGetById(id.GetValueOrDefault());

            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                var form = new TrackAddFormViewModel();
                form.AlbumId = id.GetValueOrDefault();
                form.AlbumName = obj.Name;
                form.GenreList = new SelectList(m.GetAllGenres(), "Name", "Name");

                return View(form);
            }
        }

        [Authorize(Roles = "Clerk")]
        [Route("Album/{id}/AddTrack")]
        [HttpPost]
        public ActionResult AddTrack(TrackAddViewModel newItem)
        {
            if (!ModelState.IsValid)

            {
                var form = new TrackAddFormViewModel();
                form.AlbumId = newItem.AlbumId;
                form.AlbumName = newItem.Name;
                form.GenreList = new SelectList(m.GetAllGenres(), "Name", "Name");
                return View(form);
                //return View(newItem);
            }
            else
            {
                var addedItem = m.AddNewTrack(newItem, newItem.AlbumId);

                if (addedItem == null)
                {
                    return View(newItem);
                }
                else
                {
                    return RedirectToAction("Details", "Track", new { id = addedItem.Id });
                }
            }
        }


    }
}
