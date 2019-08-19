using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.Linq
{
    //public static - IMPORTANT
    public static class MyLinq
    {
        /**
         * Extension methods allow us to define a STATIC METHOD that appears to be a member of 
         * another type (any type: classes, interfaces, structs, even sealed types).
         */

        //'this' keyword tells the C# compiler that the method is an extension method.
        //We can have as many params in this method, only the first one has the 'this' modifier.
        //Type of the first param is important. In our case, we are targetting IEnumerable<T> to
        //extend.

        //All LINQ query operators (filter, sort, aggregate) are defined as EXTENSION methods.
        //public static method - IMPORTANT
        public static int Count<T>(this IEnumerable<T> sequence)
        {
            var count = 0;
            foreach (var item in sequence)
            {
                count += 1;
            }
            return count;
        }
    }
}
