using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Features.Linq; //Adding the namespace where the extension method lives (our method, LINQ has Count() method too)

/**
 * IEnumerable<string> filteredList = cities.Where(StartsWithL) //Named method
 * IEnumerable<string> filteredList = cities.Where(delegate(string s) { return s.StartsWith("L"); }); //Anonymous method
 * IEnumerable<string> filteredList = cities.Where(s => s.StartsWith("L")); //Lambda expression
 */

/**
 * Implicit typing
 * var name = "Scott";
 * 'var' keyword introduced to C# at the same time as LINQ. It does work with queries easier in many
 * scenarios. Var allows the compiler to INFER the type of the variable (without explicitly specifying it).
 * Variable is still strongly typed (can't assign integer to the variable 'name').
 * When using this implicit typing, you must initialize the variable. You can't initialize to null.
 */
namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            /**
             * Func type was introduced as an easy way to work with delegates (types that allow
             * creating variables that point to methods).
             * Func<int, string>
             * Last generic type (string in the above case) param describes the RETURN type of a method.
             * Func<int, int> f = Square;
             */

            //Creating a method that can square a number
            Func<int, int> square = x => x * x;

            Func<int, int, int> add = (x, y) =>
            {
                int temp = x + y;
                return temp;
            };

            //Other delegate type is Action (always returns void):
            Action<int> write = x => Console.WriteLine(x); //Accepts int as input, returns void.

            write(square(add(3, 5)));

            //var developers = new Employee[]
            //{
            //    new Employee { Id = 1, Name= "Scott" },
            //    new Employee { Id = 2, Name= "Chris" }
            //};

            IEnumerable<Employee> developers = new Employee[]
            {
                new Employee { Id = 1, Name = "Scott" },
                new Employee { Id = 2, Name = "Chris" }
            };

            var sales = new List<Employee>()
            {
                new Employee { Id = 3, Name = "Alex" }
            };

            /**
             * Both of these collections have:
             * sales.GetEnumerator()
             * developers.GetEnumerator()
             * That special method exists because both (list & array) implement an interface
             * which is IEnumerable<T>.
             */

            //You can hide any sort of data structure or operation behind this interface
            //Everytime MoveNext() is called, we don't know if the underlying enumerator just
            //has to access a new element in an array, or index into the next item of a list,
            //or follow the pointer in a tree, or even make a callback to the db server to fetch
            //the next record out of a table.
            //So IEnumerable is the PERFECT interface for hiding the source of data.
            //That's why 98% of LINQ features/operators can work agains IEnumerable<T>.
            //The only thing we can do is GetEnumerator() that can walk through each item in
            //a collection of objects.
            IEnumerator<Employee> enumerator = developers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Name);
            }
            Console.WriteLine(developers.Count());

            Console.WriteLine("***");

            //1. Named method:
            //foreach (var employee in developers.Where(NameStartsWithS))

            //2. Anonymous method:
            //foreach (var employee in developers.Where(
            //    delegate(Employee employee)
            //    {
            //        return employee.Name.StartsWith("S");
            //    })
            //)

            //3. Lambda expression:
            //Just another way of creating executable code
            //foreach (var employee in developers.Where(e => e.Name.StartsWith("S")))
            //foreach (var employee in developers
            //    .Where(e => e.Name.Length == 5)
            //    .OrderBy(e => e.Name))
            //{
            //    Console.WriteLine(employee.Name);
            //}

            //Method syntax approach
            //var query = developers.Where(e => e.Name.Length == 5)
            //                      .OrderBy(e => e.Name);

            //Query syntax approach
            //Query syntax ALWAYS starts with the 'from' keywoard. Think of it as a for..each loop 
            //("for each developer in developers")
            //Query ends with 'select' (to tell the compiler what shape the result looks like) or 
            //'group' (to group the result).
            //The reason the C# language designers use this approach ('select' appearing at the end)
            //is because when we first specify where the data is coming from, then VS & C# compiler
            //know what might data looks like. And we can have IntelliSense help in the rest of the
            //query.
            //Not every LINQ operator is available in this query syntax, sometimes we must use
            //the method syntax.

            var query2 = from developer in developers
                         where developer.Name.Length == 5
                         orderby developer.Name
                         select developer;

            foreach (var employee in query2)
            {
                Console.WriteLine(employee.Name);
            }

            Console.Read();
        }

        private static int Square(int arg)
        {
            throw new NotImplementedException();
        }

        //In case of using named method for filtering
        private static bool NameStartsWithS(Employee employee)
        {
            return employee.Name.StartsWith("S");
        }
    }
}
