using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;

namespace google_hashcode_2020
{
	public class Book
	{
		public int ID = 0;
		public int Score = 0;

		public bool alreadyShipped = false;

		public Book(int id, int score)
		{
			this.ID = id;
			this.Score = score;
		}
	}

	public class Library
	{
		public int ID = 0;
		public int SubscriptionTime = 0;
		public int NBooks = 0;

		public int BooksPerDay = 0;

		public List<Book> Books;

		public int Weight = 0;

		// get the library weight in term of maximum books shipper in N days
		public int getWeight(int days)
		{

			// *** SOLUTION 1
			// *** get some sort of 'weight' for the library
			// int weight = 0;

			// days -= this.SubscriptionTime;

			// this.Books = (from q in this.Books orderby q.Score descending select q).ToList();

			// int index = 0;
			// for (int i = 0; i < days; i++)
			// {
			// 	for (int j = 0; j < this.BooksPerDay; j++)
			// 	{
			// 		if (index >= this.Books.Count())
			// 			break;

			// 		weight += this.Books[index].Score;

			// 		index++;
			// 	}
			// }

			// this.Weight = weight;


			// *** SOLUTION 2
			// *** return just the SubscriptionTime
			// this.Weight = this.SubscriptionTime;


			// *** SOLUTION 3
			// *** return just the SubscriptionTime * BooksPerDay
			// this.Weight = this.SubscriptionTime * this.BooksPerDay;


			// *** SOLUTION 4
			// some sort of weight * subscriptiontime
			// int weight = 0;

			// days -= this.SubscriptionTime;

			// this.Books = (from q in this.Books orderby q.Score descending select q).ToList();

			// int index = 0;
			// for (int i = 0; i < days; i++)
			// {
			// 	for (int j = 0; j < this.BooksPerDay; j++)
			// 	{
			// 		if (index >= this.Books.Count())
			// 			break;

			// 		weight += this.Books[index].Score;

			// 		index++;
			// 	}
			// }

			// this.Weight = weight * this.SubscriptionTime;


			// *** SOLUTION 5
			this.Weight = this.SubscriptionTime * this.Books.Count();


			// *** RETURN
			return this.Weight;
		}

		public List<int> getBooks(int days)
		{
			return (from q in this.Books where !q.alreadyShipped select q.ID).Take((days - this.SubscriptionTime) * this.BooksPerDay).ToList();
		}

		public void markAsShipped(List<int> books)
		{
			var asd = (from q in this.Books where books.Contains(q.ID) select q).ToList();
			foreach (Book a in asd)
				a.alreadyShipped = true;

			// foreach (Book b in this.Books)
			// 	foreach (int b2 in books)
			// 		if (b.ID == b2)
			// 			b.alreadyShipped = true;
		}

		public Library(int id, int time, int nbooks)
		{
			this.ID = id;
			this.SubscriptionTime = time;
			this.NBooks = nbooks;
		}

		public Library()
		{ }
	}

	class Program
	{
		static string inputFolder = "../input";
		static string outputFolder = "../output";

		static string[] inputFiles = {
				"a_example",
				"b_read_on",
				"c_incunabula",
				"d_tough_choices",
				"e_so_many_books",
				"f_libraries_of_the_world"
			};

		static string inputExt = ".txt";
		static string outputExt = ".out";

		static void Main(string[] args)
		{
			if (!Directory.Exists(outputFolder))
				Directory.CreateDirectory(outputFolder);

			foreach (string f in inputFiles)
			{
				// *** PARSE

				Console.Write(("Solving file '" + f + "' ...").PadRight(40));

				string[] rows = new StreamReader(Path.Combine(inputFolder, f) + inputExt).ReadToEnd().Split('\n');

				int[] firstRow = (from q in rows[0].Split(' ') select Convert.ToInt32(q)).ToArray();
				int numberOfBooks = firstRow[0];
				int numberOfLibraries = firstRow[1];
				int numberOfDays = firstRow[2];

				int index = 0;
				List<Book> books = (from q in rows[1].Split(' ') select new Book(index++, Convert.ToInt32(q))).ToList();

				List<Library> libraries = new List<Library>();

				for (int i = 0; i < numberOfLibraries * 2; i += 2)
				{
					Library l = new Library();
					int[] libraryRow = (from q in rows[2 + i].Split(' ') select Convert.ToInt32(q)).ToArray();
					l.ID = i / 2;
					l.NBooks = libraryRow[0];
					l.SubscriptionTime = libraryRow[1];
					l.BooksPerDay = libraryRow[2];

					l.Books = (from q in rows[2 + (i + 1)].Split(' ') select new Book(Convert.ToInt32(q), books[Convert.ToInt32(q)].Score)).ToList();

					libraries.Add(l);
				}

				// *** SOLVE

				// sort libraries by their weight
				List<Library> librariesWeighted = (from q in libraries orderby q.getWeight(numberOfDays) descending select q).ToList();
				// List<Library> librariesWeighted = (from q in libraries orderby q.getWeight(numberOfDays) select q).ToList();

				int n = 0;

				List<string> asd = new List<string>();

				foreach (Library l in librariesWeighted)
				// foreach (Library l in libraries)
				{
					List<int> indexes = l.getBooks(numberOfDays);

					if (indexes.Count() == 0)
						continue;

					indexes.Sort();

					// mark as 'already shipped' the same book in other librearies
					foreach (Library l2 in libraries)
						l2.markAsShipped(indexes);

					asd.Add(string.Format("{0:d} {1:d}", l.ID, indexes.Count()));
					asd.Add(string.Join(' ', indexes));

					n++;
				}

				StreamWriter sw = new StreamWriter(Path.Combine(outputFolder, f) + outputExt);
				sw.WriteLine(n);
				foreach (string s in asd)
					sw.WriteLine(s);
				sw.Close();

				Console.WriteLine("done!");
			}

			Console.WriteLine("\nOK");
		}
	}
}
