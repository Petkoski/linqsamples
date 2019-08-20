using System.Linq;
using System.Collections.Generic;
using System;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            //var numbers = MyLinq.Random().Where(n => n > 0.5).Take(10).OrderBy(n => n);
            //foreach (var number in numbers)
            //{
            //    Console.WriteLine(number);
            //}

            var movies = new List<Movie>
            {
                new Movie { Title = "The Dark Knight",   Rating = 8.9f, Year = 2008 },
                new Movie { Title = "The King's Speech", Rating = 8.0f, Year = 2010 },
                new Movie { Title = "Casablanca",        Rating = 8.5f, Year = 1942 },
                new Movie { Title = "Star Wars V",       Rating = 8.7f, Year = 1980 }                        
            };

            //var query = movies.Where(m => m.Year > 2000);
            //var query = movies.Filter(m => m.Year > 2000);

            //Turn off deferred execution and do something that immediately executes that query
            //and materialize it into a CONCRETE RESULT (ToArray, ToList, ToDictionary).
            //var query = movies.Filter(m => m.Year > 2000).ToList();

            var query = Enumerable.Empty<Movie>();

            //Exceptions and deferred queries (CH04-07)
            try
            {
                query = movies.Filter(m => m.Year > 2000); //Without .ToList(), we are just defining the query here. The exception is thrown in the Count() below.
                //Bottom line is that we really need to use try..catch block in the code that EXECUTES the query.
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //foreach (var movie in query)
            //{
            //    Console.WriteLine(movie.Title);
            //}

            Console.WriteLine(query.Count()); //Forces the query to execute immediately, so that the Count() operator can loop through the results
            var enumerator = query.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Title);
            }

            Console.Read();
        }
    }
}
