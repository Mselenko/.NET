using Assignment2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment2.Controllers
{
    public class PlaylistController : Controller
    {

        private Manager m = new Manager();
        // GET: Playlist
        public ActionResult Index()
        {
            return View(m.PlaylistGetAll());
        }

        // GET: Playlist/Details/5
        public ActionResult Details(int? id)
        {
            var obj = m.PlaylistGetById(id.GetValueOrDefault());

            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(obj);
            }
        }

        // GET: Playlist/Edit/5
        public ActionResult Edit(int? id)
        {
            var obj = m.PlaylistGetById(id.GetValueOrDefault());

            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                var form = m.mapper.Map<PlaylistEditTracksFormViewModel>(obj);

                var selectedValues = obj.Tracks.Select(c => c.TrackId);

                form.TrackList = new MultiSelectList
                    (items: m.TrackGetAll(),
                    dataValueField: "TrackId",
                    dataTextField: "NameShort",
                    selectedValues: selectedValues);

                return View(form);
            }
        }



        // POST: Playlist/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, PlaylistEditTracksViewModel newitem)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("edit", new { id = newitem.PlaylistId });
            }

            if (id.GetValueOrDefault() != newitem.PlaylistId)
            {
                return RedirectToAction("index");
            }

            var editedItem = m.PlaylistEditTracks(newitem);

            if (editedItem == null)
            {
                return RedirectToAction("edit", new { id = newitem.PlaylistId });
            }
            else
            {
                return RedirectToAction("Details", new { id = newitem.PlaylistId });
            }
        }
    }
}
