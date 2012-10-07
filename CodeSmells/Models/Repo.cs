using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeSmells.Models
{
	public static class Repo
	{
		public static List<File> ProcessConcatentation(string f)
		{
			var concatFiles = new List<File>();
			var lines = System.IO.File.ReadAllLines(f);
			for (var line = 0; line < lines.Length; line++)
			{
				var file = new File
				{
					FileName = f,
					Lines = lines,
					Loops = new List<Loop>()
				};

				if (!Rules.StringConcatenation.IsMatch(lines[line])) continue;
				var i = line;
				while (i > 0)
				{
					if (lines[i].Contains("For "))
					{
						var loop = new Loop
						{
							Start = i,
							Lines = new List<string>()
						};

						var j = i;
						while (j < lines.Length)
						{
							if (lines[j].Contains("Next"))
							{
								loop.End = j;
								break;
							}

							j++;
						}

						if (loop.Start < line && loop.End > line)
						{
							if (!concatFiles.Any(x => x.FileName == file.FileName))
								concatFiles.Add(file);

							file = concatFiles.Single(x => x.FileName == file.FileName);

							if (!file.Loops.Any(x => x.Start == loop.Start))
								file.Loops.Add(loop);

							loop = file.Loops.Single(x => x.Start == loop.Start);

							loop.Lines.Add(String.Format("{0}: {1}", line, lines[line]));
						}

						break;
					}

					i--;
				}
			}

			return concatFiles;
		}
	}
}