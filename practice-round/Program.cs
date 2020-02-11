using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace google_hashcode_2020
{
	class Program
	{
		static string inputFolder = "input";
		static string outputFolder = "output";

		static string[] inputFiles = {
				"a_example",
				"b_small",
				"c_medium",
				"d_quite_big",
				"e_also_big",
			};

		static string inputExt = "in";
		static string outputExt = "out";

		static int Solve(int maxSlices, int[] data, string fileName)
		{
			int slices = 0;

			List<int> chosenPizzas = new List<int>();

			for (int i = data.Length - 1; i >= 0; i--)
			{
				if ((slices + data[i]) > maxSlices)
					continue;

				slices += data[i];
				chosenPizzas.Add(i);

				if (slices == maxSlices)
					break;
			}

			chosenPizzas.Reverse();

			StreamWriter sw = new StreamWriter(Path.Combine(outputFolder, fileName) + "." + outputExt);
			sw.WriteLine(chosenPizzas.Count());
			sw.WriteLine(string.Join(" ", chosenPizzas));
			sw.Close();

			return slices;
		}

		static void Main(string[] args)
		{
			int totalScore = 0;

			if (!Directory.Exists(outputFolder))
				Directory.CreateDirectory(outputFolder);

			foreach (string f in inputFiles)
			{
				Console.Write(("Solving file '" + f + "' ...").PadRight(40));

				string[] rows = new StreamReader(Path.Combine(inputFolder, f) + "." + inputExt).ReadToEnd().Split('\n');

				int pizzaSlices = Convert.ToInt32(rows[0].Split(' ')[0]);
				int pizzaTypes = Convert.ToInt32(rows[0].Split(' ')[1]);
				int[] data = (from q in rows[1].Split(' ') select Convert.ToInt32(q)).ToArray();

				int score = Solve(pizzaSlices, data, f);
				totalScore += score;

				Console.WriteLine((score + " points").PadLeft(20));
			}

			Console.WriteLine("\nTotal score: " + totalScore);
			Console.WriteLine("\nOK");
		}
	}
}
