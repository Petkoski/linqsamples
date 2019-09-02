using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CarDb>());
            InsertData();
            //QueryData();

            Console.Read();
        }

        private static void QueryData()
        {
            var db = new CarDb();

            db.Database.Log = Console.WriteLine;

            var query =
                from car in db.Cars
                group car by car.Manufacturer into manufacturer
                select new
                {
                    Name = manufacturer.Key,
                    Cars = (from car in manufacturer
                           orderby car.Combined descending
                           select car).Take(2)
                };
                

            foreach (var group in query)
            {
                Console.WriteLine(group.Name);
                foreach (var car in group.Cars)
                {
                    Console.WriteLine($"\t{car.Name}: {car.Combined}");
                }
            }           
        }

        private static void InsertData()
        {
            var cars = ProcessCars("fuel.csv");
            //var db = new CarDb();

            //ThenBy() & ThenByDescending() - used for secondary sort
            //Tertiary sort is possible by adding another ThenBy() / ThenByDescending()
            var query = cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
                            .OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name); //Secondary sort

            var queryTop = cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
                            .OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name)
                            .First(); //Returns a single car

            var queryTop2 = cars
                            .OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name)
                            .First(c => c.Manufacturer == "BMW" && c.Year == 2016); //Returns a single car

            //queryTop is more efficient because it first filters, then sorts

            //First has 2 overloads:
            //First()
            //First(takes Func<Car, bool> as param) - gives first result where condition is true
            //There is also Last() operator (also has 2 overloads)

            //Both First() and Last() have an alternate version:
            //First - throws an exception if it cannot produce an item (ex. sequence contains no matching element)
            //FirstOrDefault - returns a default value if it cannot produce an item (default value for a Car variable is - null, for integer - 0)

            Console.WriteLine(queryTop.Name);
            Console.WriteLine(queryTop2.Name);

            //Query syntax
            var query2 = from car in cars
                        where car.Manufacturer == "BMW" && car.Year == 2016
                        orderby car.Combined descending, car.Name ascending
                        select car; //Identity projection (for every car, select that car [without trying to transform it])

            var query2Top = (from car in cars
                         where car.Manufacturer == "BMW" && car.Year == 2016
                         orderby car.Combined descending, car.Name ascending
                         select car).First();

            //Reverse() operator - reverses the order of the items

            //CH05-07
            //Quantifiers (can tell us if anything matches a predicate, or if the data set just contains a specific item)
            //All of these operators return true/false, they do not offer deferred execution

            var quan1 = cars.Any(); //Is there anything in this data set?
            var quan2 = cars.Any(c => c.Manufacturer == "Ford"); //Is there any car from "Ford" manufacturer?
            var quan3 = cars.All(c => c.Manufacturer == "Ford"); //Do all of the cars have a manufacturer of "Ford"?
            //var quan4 = cars.Contains() //Check if an item is in collection
            Console.WriteLine(quan1); //True
            Console.WriteLine(quan2); //True
            Console.WriteLine(quan3); //False

            foreach (var car in query.Take(10))
            {
                Console.WriteLine($"{car.Manufacturer} {car.Name} : {car.Combined}");
            }

            //if (!db.Cars.Any())
            //{
            //    foreach (var car in cars)
            //    {
            //        db.Cars.Add(car);
            //    }
            //    db.SaveChanges();
            //}
        }

        private static void QueryXml()
        {
            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";
            var document = XDocument.Load("fuel.xml");

            var query =
                from element in document.Element(ns + "Cars")?.Elements(ex + "Car")
                                                       ?? Enumerable.Empty<XElement>()
                where element.Attribute("Manufacturer")?.Value == "BMW"
                select element.Attribute("Name").Value;

            foreach (var name in query)
            {
                Console.WriteLine(name);
            }
        }

        private static void CreateXml()
        {
            var records = ProcessCars("fuel.csv");

            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";
            var document = new XDocument();
            var cars = new XElement(ns + "Cars",

                from record in records
                select new XElement(ex + "Car",
                                new XAttribute("Name", record.Name),
                                new XAttribute("Combined", record.Combined),
                                new XAttribute("Manufacturer", record.Manufacturer))
            );

            cars.Add(new XAttribute(XNamespace.Xmlns + "ex", ex));

            document.Add(cars);
            document.Save("fuel.xml");
        }

        private static List<Car> ProcessCars(string path)
        {
            //Skip() and Take() are useful when doing paging operations.
            //Skip - skips the first n items
            //Take - takes n items

            //var query = File.ReadAllLines(path) //Returns string[]
            //        .Skip(1) //Paging operation - avoid processing the header line (that contains the column names)
            //        .Where(line => line.Length > 1) //Line must have some length
            //        .Select(Car.ParseFromCsv);
            //        //.ToCar();

            var query = from line in File.ReadAllLines(path).Skip(1)
                        where line.Length > 1
                        select Car.ParseFromCsv(line);

            return query.ToList();
        }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query =
                   File.ReadAllLines(path)
                       .Where(l => l.Length > 1)
                       .Select(l =>
                       {
                           var columns = l.Split(',');
                           return new Manufacturer
                           {
                               Name = columns[0],
                               Headquarters = columns[1],
                               Year = int.Parse(columns[2])
                           };
                       });
            return query.ToList();
        }
    }

    public class CarStatistics
    {
        public CarStatistics()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;
        }
        
        public CarStatistics Accumulate(Car car)
        {
            Count += 1;
            Total += car.Combined;
            Max = Math.Max(Max, car.Combined);
            Min = Math.Min(Min, car.Combined);
            return this;
        }

        public CarStatistics Compute()
        {
            Average = Total / Count;
            return this;
        }

        public int Max { get; set; }
        public int Min { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }
        public double Average { get; set; }

    }

    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');

                yield return new Car
                {
                    Year = int.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = double.Parse(columns[3]),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Highway = int.Parse(columns[6]),
                    Combined = int.Parse(columns[7])
                };
            }
        }
    }
}
