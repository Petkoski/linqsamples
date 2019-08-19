using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduction
{
    class Program
    {
        /**
         * LINQ - Language-Integrated Query
         * How to query and manipulate data from d ifferent data sources using C# LINQ's features
         * 
         * Working with data (previously):
         * Object data: Generics, Algorithms
         * Relational data: ADO.NET (to send SQL commands to the db), SQL
         * XML data: XmlDocument, Xpath / ESLT
         * 
         * Now:
         * LINQ - consistent API for ALL these different sources
         * Gives us strongly typed compile-time checks on queries that can execute against 
         * in-memory data, relational data, and XML data (objects, MongoDB, CSV files, file system, 
         * SQL db, HL7 XML, JSON)
         * 
         * Course overview:
         * 1. C# features for LINQ
         * 2. Basic queries
         * 3. Filtering, sorting, projecting
         * 4. Grouping, joining, aggregating
         * 5. Working with XML, SQL DB
         */

        static void Main(string[] args)
        {
            string path = @"C:\Windows";
            ShowLargeFilesWithoutLinq(path);
            Console.WriteLine("***");
            ShowLargeFilesWithLinq(path);
            Console.Read();
        }

        private static void ShowLargeFilesWithLinq(string path)
        {
            //Technique 1:
            //Even looks like a query you would write in SQL for a relational db
            var q1 = from file in new DirectoryInfo(path).GetFiles()
                    orderby file.Length descending
                    select file;
            foreach (var file in q1.Take(5))
            {
                Console.WriteLine($"{file.Name,-30} : {file.Length,10:N0}");
            }

            Console.WriteLine("***");

            //Technique 2:
            //Using a series of method calls (slightly different syntax)
            var q2 = new DirectoryInfo(path).GetFiles()
                            .OrderByDescending(f => f.Length)
                            .Take(5);

            foreach (var file in q2)
            {
                Console.WriteLine($"{file.Name,-30} : {file.Length,10:N0}");
            }
        }

        private static void ShowLargeFilesWithoutLinq(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path); //DirectoryInfo gives info about the files inside a specifed dir
            FileInfo[] files = directory.GetFiles();
            Array.Sort(files, new FileInfoComparer()); //Explicitly telling how to sort the array (alphabetically, or by size, etc.) [by passing an object that will COMPARE 2 FileInfo objects and determine which is greater]

            for (int i = 0; i < 5; i++)
            {
                FileInfo file = files[i];
                Console.WriteLine($"{file.Name, -30} : {file.Length, 10:N0}");
                //Left justify file.Name inside of 30 spaces
                //Right justify file.Length inside of 10 spaces column
                //N - format file.Length as a number (with commas)
                //0 - use 0 positions after the decimal point
            }
        }
    }

    //Comparer class must implement IComparer<> interface of FileInfo type
    public class FileInfoComparer : IComparer<FileInfo>
    {
        public int Compare(FileInfo x, FileInfo y)
        {
            //Insert logic here that returns (by ANY criteria when comparing):
            //-1 if first file is less than the second
            //0 if both are equal
            //+1 if first file is greater than the second

            return y.Length.CompareTo(x.Length); //Comparing them by LENGTH here
        }
    }
}
