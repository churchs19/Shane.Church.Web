using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shane.Church.Web.v2012.Models.Software;

namespace Shane.Church.Web.Controllers
{
	public class SoftwareController : Controller
	{
		public ActionResult WMSmartphone(int? id)
		{
			SoftwareViewModel model = new SoftwareViewModel();
			if (id.HasValue)
			{
				model.Id = id.Value;
			}

			return View(model);
		}

		public ActionResult WMPocketPC(int? id)
		{
			SoftwareViewModel model = new SoftwareViewModel();
			if (id.HasValue)
			{
				model.Id = id.Value;
			}

			return View(model);
		}

		public ActionResult MSWindows(int? id)
		{
			SoftwareViewModel model = new SoftwareViewModel();
			if (id.HasValue)
			{
				model.Id = id.Value;
			}

			return View(model);
		}

		public ActionResult MSPocketPC(int? id)
		{
			SoftwareViewModel model = new SoftwareViewModel();
			if (id.HasValue)
			{
				model.Id = id.Value;
			}

			return View(model);
		}

		public ActionResult Palm()
		{
			return View();
		}

		public ActionResult Win7Gadgets(int? id)
		{
			SoftwareViewModel model = new SoftwareViewModel();
			if (id.HasValue)
			{
				model.Id = id.Value;
			}

			return View(model);
		}

		public ActionResult WindowsPhone(int? id)
		{
			SoftwareViewModel model = new SoftwareViewModel();
			if (id.HasValue)
			{
				model.Id = id.Value;
			}
			return View(model);
		}
	}
}
