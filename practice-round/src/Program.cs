using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace google_hashcode_2020
{
	class Program
	{
		static string inputFolder = "../input";
		static string outputFolder = "../output";

		static string[] inputFiles = {
				"a_example",
				"b_small",
				"c_medium",
				"d_quite_big",
				"e_also_big",
			};

		static string inputExt = ".in";
		static string outputExt = ".out";

		static Tuple<int, List<int>> Solve(int maxSlices, int[] data, int startIndex)
		{
			int slices = 0;
			int index = 0;

			List<int> chosenPizzas = new List<int>();

			for (int i = startIndex; i >= startIndex - (data.Length - 1); i--)
			{
				index = i;
				if (index < 0)
					index = data.Length + i;

				if ((slices + data[index]) > maxSlices)
					continue;

				slices += data[index];
				chosenPizzas.Add(index);

				if (slices == maxSlices)
					break;
			}

			chosenPizzas.Sort();

			return new Tuple<int, List<int>>(slices, chosenPizzas);
		}

		static Tuple<int, List<int>> Solve(int maxSlices, int[] data, string fileName)
		{
			Tuple<int, List<int>> bestSolution = new Tuple<int, List<int>>(0, new List<int>());
			for (int i = data.Length - 1; i >= 0; i--)
			{
				var result = Solve(maxSlices, data, i);
				if (result.Item1 >= bestSolution.Item1)
					bestSolution = result;
			}

			StreamWriter sw = new StreamWriter(Path.Combine(outputFolder, fileName) + outputExt);
			sw.WriteLine(bestSolution.Item2.Count());
			sw.WriteLine(string.Join(" ", bestSolution.Item2));
			sw.Close();

			return bestSolution;
		}

		static void Main(string[] args)
		{
			int totalScore = 0;

			if (!Directory.Exists(outputFolder))
				Directory.CreateDirectory(outputFolder);

			Stopwatch sw = new Stopwatch();
			sw.Start();

			foreach (string f in inputFiles)
			{
				Console.Write(("Solving file '" + f + "' ...").PadRight(40));

				string[] rows = new StreamReader(Path.Combine(inputFolder, f) + inputExt).ReadToEnd().Split('\n');

				int pizzaSlices = Convert.ToInt32(rows[0].Split(' ')[0]);
				int pizzaTypes = Convert.ToInt32(rows[0].Split(' ')[1]);
				int[] data = (from q in rows[1].Split(' ') select Convert.ToInt32(q)).ToArray();

				var result = Solve(pizzaSlices, data, f);
				totalScore += result.Item1;

				Console.WriteLine((result.Item1 + " points").PadLeft(20));
			}

			sw.Stop();

			Console.WriteLine("\nTotal score: " + totalScore + " (" + sw.ElapsedMilliseconds + " ms)");
			Console.WriteLine("\nOK");
		}
	}
}
