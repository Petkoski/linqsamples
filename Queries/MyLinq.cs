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
            //foreach (var item in source)
            //{
            //    if (predicate(item))
            //    {
            //        yield return item;
            //    }
            //}

            //One possible implementation. This is NOT the way LINQ actually implements the Where operator
            var result = new List<T>();

            foreach (var item in source)
            {
                if(predicate(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}