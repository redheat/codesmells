using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeSmells.Models
{
	public static class Repo
	{
		public static IEnumerable<File> ProcessConcatentation(string f)
		{
			var files = new List<File>();
			var lines = System.IO.File.ReadAllLines(f);
			for (var line = 0; line < lines.Length; line++)
			{
				// Move onto the next row if this does not contain a code smell
				if (!Rules.StringConcatenation.IsMatch(lines[line])) continue;

				// Ignore commented lines
				if (lines[line].Trim().StartsWith("'") || lines[line].Trim().StartsWith("//")) continue;
				
				var file = new File { FileName = f, Lines = lines, Loops = new List<Loop>() };
				var i = line;

				// loop from the matched line to the beginning of the document
				// find if this line is in a loop
				while (i > 0)
				{
					if (lines[i].Contains("For "))
					{
						var loop = new Loop { Start = i, Lines = new List<string>() };

						var j = i;

						// loop from the beginning of the loop to the end of the file
						// we are hoping to find the end of the loop
						while (j < lines.Length)
						{
							if (lines[j].Contains("Next"))
							{
								loop.End = j;
								break; // we've found the end of the loop
							}

							j++;
						}

						// Add relevant loops and files, keeping `List<File>` distinct
						if (loop.Start < line && loop.End > line)
						{
							// if the file isn't in our collection then add it
							if (!files.Any(x => x.FileName == file.FileName))
								files.Add(file);

							file = files.Single(x => x.FileName == file.FileName);

							// if this loop isn't in our collection then add it
							if (!file.Loops.Any(x => x.Start == loop.Start))
								file.Loops.Add(loop);

							loop = file.Loops.Single(x => x.Start == loop.Start);

							// add the new line
							// TODO: turn this into a `Line` object
							loop.Lines.Add(String.Format("{0}: {1}", line, lines[line]));
						}

						break;
					}

					i--;
				}
			}

			return files;
		}
	}
}