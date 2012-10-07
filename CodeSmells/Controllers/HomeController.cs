using System.Collections.Generic;
using System.Linq;
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
			AsyncManager.OutstandingOperations.Increment();
	        const string baseVolume = @"\Work\Dev\";
			var files = System.IO.Directory.EnumerateFiles(baseVolume + "Vx.WebApplication\\edit").Where(
				f => f.EndsWith(".aspx") || f.EndsWith(".vb"));

	        files = files.Concat(
		        System.IO.Directory.EnumerateFiles(baseVolume + "Vx.Legacy").Where(
			        f => f.EndsWith(".aspx") || f.EndsWith(".vb")));

			var concatFiles = new List<File>();

			foreach (var f in files)
				concatFiles.AddRange(Repo.ProcessConcatentation(f));

	        AsyncManager.Parameters["concatenatedFiles"] = (from x in concatFiles
	                                                        where x.Loops.Any()
															orderby x.FileName
	                                                        select x);
			AsyncManager.OutstandingOperations.Decrement();
        }

		public ActionResult IndexCompleted(IEnumerable<File> concatenatedFiles)
		{
			return View("Index", concatenatedFiles);
		}

    }
}
