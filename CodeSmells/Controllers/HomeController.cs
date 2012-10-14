using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using CodeSmells.Models;

namespace CodeSmells.Controllers
{
    public class HomeController : AsyncController
    {
        //
        // GET: /Home/

        public void IndexAsync()
        {
	        const string baseVolume = @"\Work\Dev\";

			var files = System.IO.Directory.EnumerateFiles(baseVolume + "Vx.WebApplication\\edit")
				.Concat(System.IO.Directory.EnumerateFiles(baseVolume + "Vx.Legacy"))
				.Where(
			        f => f.EndsWith(".aspx") || f.EndsWith(".vb")
				);

			var fileList = new List<File>();

			foreach (var f in files)
			{
				AsyncManager.OutstandingOperations.Increment();
				ThreadPool.QueueUserWorkItem(file =>
				{
					fileList.AddRange(Repo.ProcessConcatentation(file.ToString()));
					AsyncManager.OutstandingOperations.Decrement();
				}, f);
			}

	        AsyncManager.Parameters["files"] = (from x in fileList
												where x.Loops.Any()
												orderby x.FileName
												select x);
        }

		public ActionResult IndexCompleted(IEnumerable<File> files)
		{
			return View("Index", files);
		}

    }
}
