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
	public class CommunityController : BaseController<CommunityController>
	{
		// GET: CommunityController
		public ActionResult Index()
		{
			return View();
		}

		// GET: CommunityController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: CommunityController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: CommunityController/Create
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

		// GET: CommunityController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: CommunityController/Edit/5
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

		// GET: CommunityController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: CommunityController/Delete/5
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
