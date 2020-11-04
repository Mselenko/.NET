using Assignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment1.Controllers
{
    public class EmployeeController : Controller
    {
        private Manager m = new Manager();

        // GET: Employee
        public ActionResult Index()
        {
            return View(m.EmployeeGetAll());
        }

        // GET: Employee/Details/5
        public ActionResult Details(int? id)
        {
            var obj = m.EmployeeGetById(id.GetValueOrDefault());
            if (obj == null)
                return HttpNotFound();
            else
                return View(obj);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(EmployeeAddViewModel newItem)
        {
            
            if (!ModelState.IsValid)
                return View(newItem);
            try
            {              
                var addedItem = m.EmployeeAdd(newItem);
             
                if (addedItem == null)
                    return View(newItem);
                else
                    return RedirectToAction("Details", new { id = addedItem.EmployeeId });
            }
            catch
            {
                return View(newItem);
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}
