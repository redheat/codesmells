using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeSmells.Models
{
	public class File
	{
		public string FileName;
		public string[] Lines;
		public List<Loop> Loops;
		private string _content;
	}
}