using System;

namespace Queries
{
    public class Movie
    {
        public string Title { get; set; }
        public float Rating { get; set; }
        //public int Year { get; set; }

        int _year; //private by default
        public int Year
        {
            get
            {
                //throw new Exception("Error!");
                Console.WriteLine($"Returning {_year} for {Title}");
                return _year;
            }
            set
            {
                _year = value;
            }
        }
    }
}