using Assignment5.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Assignment5.Controllers
{
    public class ArtistController : Controller
    {
        private Manager m = new Manager();
    
        [Authorize]

        public ActionResult Index()
        {
            return View(m.GetAllArtists());
        }

        // GET: Artist/Details/5
        public ActionResult Details(int? id)
        {
            var obj = m.ArtistGetById(id.GetValueOrDefault());

            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(obj);
            }
        }


        // GET: Artist/Create
       [Authorize(Roles = "Executive")]
        public ActionResult Create()
        {
            var form = new ArtistAddFormViewModel();
            form.GenreList = new SelectList(m.GetAllGenres(), "Name", "Name");

            return View(form);
        }

        // POST: Artist/Create
        [Authorize(Roles = "Executive")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(ArtistAddViewModel newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            var addedItem = m.AddNewArtist(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("Details", new { id = addedItem.Id });
            }
        }


        [Authorize(Roles = "Coordinator")]
        [Route("Artist/{id}/AddAlbum")]
        public ActionResult AddAlbum(int? id)
        {
            var obj = m.ArtistGetById(id.GetValueOrDefault());

            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                var form = new AlbumAddFormViewModel();
                var selectedValues = new List<int> { obj.Id };

                form.ArtistList = new MultiSelectList
                    (items: m.GetAllArtists(),
                    dataValueField: "Id",
                    dataTextField: "Name",
                    selectedValues: selectedValues);

                form.TrackList = new MultiSelectList
                    (items: m.TrackGetAllByArtistId(obj.Id),
                    dataValueField: "Id",
                    dataTextField: "Name");

                form.ArtistName = obj.Name;
                form.GenreList = new SelectList(m.GetAllGenres(), "Name", "Name");


                return View(form);
            }
        }


        [Authorize(Roles = "Coordinator")]
        [Route("Artist/{id}/AddAlbum")]
        [HttpPost, ValidateInput(false)]
        public ActionResult AddAlbum(AlbumAddViewModel newItem)
        {

            if (!ModelState.IsValid)
            {
                return View(newItem);
            }


            var addedItem = m.AlbumAdd(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("Details", "Album", new { id = addedItem.Id });
            }
        }


        [Route("Artist/{id}/MediaItemAdd")]
        public ActionResult MediaItemAdd(int? id)
        {
            var artist = m.ArtistGetById(id.GetValueOrDefault());

            if (artist == null)
            {
                return HttpNotFound();
            }
            else
            {
                var form = new MediaItemAddFormViewModel();
                form.ArtistId = artist.Id;
                form.ArtistInfo = artist.Name;
                return View(form);
            }
        }

        [HttpPost]
        [Route("Artist/{id}/MediaItemAdd")]
        public ActionResult MediaItemAdd(MediaItemAddViewModel newItem)
        {
            var artist = m.ArtistGetById(newItem.ArtistId);

            if (!ModelState.IsValid)
            {
                //var form = new MediaItemAddFormViewModel();
                //form.ArtistId = artist.Id;
                //form.ArtistInfo = artist.Portrayal;
                return View(newItem);
            }
            var addedMedia = m.AddMediaItem(newItem);

            if (addedMedia == null)
            {
                //var form = new MediaItemAddFormViewModel();
                //form.ArtistId = artist.Id;
                //form.ArtistInfo = artist.Portrayal;
                return View(newItem);
            }
            else
            {
                return RedirectToAction("Details", "Artist", new { id = addedMedia.Id });
            }
        }

    }

}
