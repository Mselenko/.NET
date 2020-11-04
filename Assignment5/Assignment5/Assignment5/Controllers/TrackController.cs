using Assignment5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment5.Controllers
{
    public class TrackController : Controller
    {
        private Manager m = new Manager();

        [Authorize]
        public ActionResult Index()
        {
            return View(m.GetAllTracks());
        }

        public ActionResult Details(int? id)
        {
            var obj = m.GetTrackById(id.GetValueOrDefault());

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
        public ActionResult Edit(int? id)
        {
           
            var o = m.GetTrackById(id.GetValueOrDefault());

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                var form = new TrackEditFormViewModel();
                form.Name = o.Name;

                return View(form);
            }
        }


        // POST: Track/Edit/5
        [Authorize(Roles = "Clerk")]
        [HttpPost]
        public ActionResult Edit(int? id, TrackEditViewModel newItem)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("edit", new { id = newItem.Id });
            }

            if (id.GetValueOrDefault() != newItem.Id)
            {
                return RedirectToAction("index");
            }

         
            var editedItem = m.TrackEdit(newItem);

            if (editedItem == null)
            {
                return RedirectToAction("edit", new { id = newItem.Id });
            }
            else
            {
                return RedirectToAction("details", new { id = newItem.Id });
            }
        }


        // GET: Track/Delete/5
        [Authorize(Roles = "Clerk")]
        public ActionResult Delete(int? id)
        {
            var itemToDelete = m.GetTrackById(id.GetValueOrDefault());

            if (itemToDelete == null)
            {
                return RedirectToAction("index");
            }
            else
            {
                return View(itemToDelete);
            }
        }

        // POST: Track/Delete/5

        [Authorize(Roles = "Clerk")]
        [HttpPost]
        public ActionResult Delete(int? id, HttpPostedFileBase file)
        {
            var result = m.TrackDelete(id.GetValueOrDefault());

            
            return RedirectToAction("index");
        }
    }

}

