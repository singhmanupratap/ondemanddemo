using Common.Interfaces;
using Common.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApp
{
    public class SolutionController : Controller
    {
        public ISolutionBusinessLayer SolutionBusinessLayer { get; set; }
        public SolutionController(ISolutionBusinessLayer solutionBusinessLayer)
        {
            SolutionBusinessLayer = solutionBusinessLayer;
        }
        // GET: Solution
        public async Task<ActionResult> Index()
        {
            var solutions = await SolutionBusinessLayer.GetSolutionsAsync();
            return View(solutions);
        }

        public async Task<ActionResult> SelectSolution(string solutionId)
        {
            if (ClaimsPrincipal.Current.Identity.IsAuthenticated)
            {
                string userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Name).Value;
                var session = new UserSession
                {
                    UserId = userId,
                    SolutionId = solutionId
                };
                session = await SolutionBusinessLayer.UpdateUserSessionAsync(session);
            }
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
