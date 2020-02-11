using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			List<int> chosenPizzas = new List<int>();
			int slices = 0;
			int index;

			for (int i = data.Length - 1; i >= 0; i--)
			{
				int sum = 0;
				index = i;
				List<int> tempPizzas = new List<int>();

				for (int j = index; j >= 0; j--)
				{
					int value = data[j];

					int tempsum = sum + value;

					if (tempsum == maxSlices)
					{
						sum = tempsum;
						tempPizzas.Add(j);
						break;
					}
					else if (tempsum < maxSlices)
					{
						sum = tempsum;
						tempPizzas.Add(j);
					}
				}

				if (slices < sum)
				{
					slices = sum;
					chosenPizzas = tempPizzas;
				}
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

			Stopwatch sw = new Stopwatch();
			sw.Start();

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

			sw.Stop();

			Console.WriteLine("\nTotal score: " + totalScore + " (" + sw.ElapsedMilliseconds + " ms)");
			Console.WriteLine("\nOK");
		}
	}
}
