using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace CodeSmells.Models
{
	public static class Rules
	{
		public static Regex StringConcatenation = new Regex(@"\b([\w]+) = \1 & ", RegexOptions.Compiled);
	}
}