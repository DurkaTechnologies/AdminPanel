using AdminPanel.Web.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Areas.Entities.Controllers
{
	[Area("Entities")]
	public class WorkerController : BaseController<WorkerController>
	{
		// GET: WorkerController
		public IActionResult Index()
		{
			return View();
		}

		// GET: WorkerController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: WorkerController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: WorkerController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: WorkerController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: WorkerController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: WorkerController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: WorkerController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
