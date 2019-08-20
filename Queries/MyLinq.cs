using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queries
{
    public static class MyLinq
    {          
        
        //public static IEnumerable<double> Random()
        //{
        //    var random = new Random();
        //    while (true)
        //    {
        //        yield return random.NextDouble();
        //    }
        //}
        
        //Creating our custom Filter operator (similar to LINQ's Where)
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> predicate) //Basic structure of most LINQ operators
        {
            //One possible implementation. This is NOT the way LINQ actually implements the Where operator
            //var result = new List<T>();

            //foreach (var item in source)
            //{
            //    if(predicate(item))
            //    {
            //        result.Add(item);
            //    }
            //}

            //return result;


            //Making our custom Filter operator to behave same as the LINQ's Where
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item; //Helps us BUILD an IEnumerable<T>
                    //Gives us the behavior known as DEFERRED execution (many LINQ operators are implemented like this)
                    //Deferred is a fancy term meaning LINQ is as LAZY as possible, it does the least amount
                    //of work it can get away with.
                    //Query does NO real work until we force the query to produce a result (with for..each
                    //statement). The line of code ('var query = movies.Filter(m => m.Year > 2000);') is
                    //just DEFINING a query that knows what to do some time in the FUTURE. The filtering
                    //operation does not execute until we try to see the results of the query (we do that
                    //with for..each statement). What else would force a query to execute? Ultimately, any
                    //operation that inspects the results will force the query to execute (ex.: if we serialize
                    //the query results to JSON/XML, if we databind the results into a grid control).

                    //How do you know which operators behave this way? Simple way is to check the MSDN
                    //documentation: https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.where
                    //Under 'Remarks': "This method is implemented by using deferred execution."

                    //How it works:
                    //Execution will start inside this Filter method ONLY when we try to pull something out
                    //of the IEnumerable. Execution will then begin and continue UNTIL we hit the first
                    //'yield' statement. At that point, 'yield return item' yields control back to the caller,
                    //returning an item. The caller can then manipulate that item and when it goes to make the
                    //next iteration and get the next thing out of the IEnumerable, execution will PICK UP
                    //AND RESUME at the point we jumped out of this Filter method.

                    //The example with movies:
                    //We jumped into the Filter method, it executed a predicate against "The Dark Knight" movie,
                    //saw that the year is 2008, yielded that movie BACK to the caller, who can then
                    //Console.Writeln that movie, and then continue through the enumerator to the next thing
                    //in the sequence.
                }
            }
        }
    }
}