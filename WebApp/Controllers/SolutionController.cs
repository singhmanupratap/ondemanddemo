using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class SolutionController : Controller
    {
        // GET: Solution
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Types()
        {
            return View();
        }

        // GET: Solution/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Solution/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Solution/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Solution/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Solution/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Solution/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Solution/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
