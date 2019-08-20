using System.Linq;
using System.Collections.Generic;
using System;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var movies = new List<Movie>
            {
                new Movie { Title = "The Dark Knight",   Rating = 8.9f, Year = 2008 },
                new Movie { Title = "The King's Speech", Rating = 8.0f, Year = 2010 },
                new Movie { Title = "Casablanca",        Rating = 8.5f, Year = 1942 },
                new Movie { Title = "Star Wars V",       Rating = 8.7f, Year = 1980 }                        
            };

            //var query = movies.Where(m => m.Year > 2000);

            //Taking advantage of deferred execution (CH04-05)
            //var query = movies.Filter(m => m.Year > 2000);


            //Avoiding pitfalls of deferred execution (CH04-06)
            //Turn off deferred execution and do something that immediately executes that query
            //and materialize it into a CONCRETE RESULT (ToArray, ToList, ToDictionary).
            //var query = movies.Filter(m => m.Year > 2000).ToList();


            //Exceptions and deferred queries (CH04-07)
            //var query = Enumerable.Empty<Movie>();
            //try
            //{
            //    query = movies.Filter(m => m.Year > 2000); //Without .ToList(), we are just defining the query here. The exception is thrown in the Count() below.
            //    //Bottom line is that we really need to use try..catch block in the code that EXECUTES the query.
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            //foreach (var movie in query)
            //{
            //    Console.WriteLine(movie.Title);
            //}


            //All about streaming operators (CH04-08)
            //Operators that offer deferred execution can either be streaming or non-streaming operators.
            //A streaming operator (like Where()) only needs to read through a source of data up until the
            //point it produces a result. At that point it will yield the result and execution can jump
            //out of the Where() method and we can process that single item.

            //OrderByDescending still offers deferred execution, but once it starts to execute, it needs to
            //go through the ENTIRE incoming sequence of items. That means when we first call to MoveNext()
            //to get the first item out of the query, OrderByDescending() has to look at everything to
            //figure out the ordering (to figure out which item to return).

            //When you introduce a non-streaming operator (sth. like OrderByDescending), now you know
            //you are going to be looking at all the items in the collection (not efficient), which is
            //why it makes sense to filter before ordering (highly improves the performance of a query 
            //that operates against in-memory data).
            var query = movies.Where(m => m.Year > 2000).OrderByDescending(m => m.Rating);


            //Querying infinity (CH04-09)
            //Classification of standard query pperators by manner of execution
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/classification-of-standard-query-operators-by-manner-of-execution

            //Take(10) to avoid the infinite loop in MyLinq.Random()
            var numbers = MyLinq.Random().Where(n => n > 0.5).Take(10).OrderBy(n => n);
            foreach (var number in numbers)
            {
                Console.WriteLine(number);
            }

            //Console.WriteLine(query.Count()); //Forces the query to execute immediately, so that the Count() operator can loop through the results
            var enumerator = query.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Title);
            }

            Console.Read();
        }
    }
}
