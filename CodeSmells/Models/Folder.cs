using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeSmells.Models
{
	public class Folder
	{
		public string FolderPath;
		public List<File> Files;
	}
}