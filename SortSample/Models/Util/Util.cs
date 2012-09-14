using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SortSample.Models.Util
{
    public static class Extenstion
    {
        public static IEnumerable<T> Sort<T>(this IEnumerable<T> lst, string sortString) where T : class
        {
            if (sortString.Split(' ').Count() > 1 && sortString.Split(' ')[1].EndsWith("Desc", StringComparison.InvariantCultureIgnoreCase))
            {
                return lst.OrderByDescending(p => p.GetType().GetProperty(sortString.Split(' ')[0]).GetValue(p, null));
            }
            else
            {
                return lst.OrderBy(p => p.GetType().GetProperty(sortString).GetValue(p, null));
            }
        }

        public static IEnumerable<T> Search<T>(this IEnumerable<T> lst, string searchString) where T : class
        {
            var p = lst.Where(i => ComplexTypeContains<T>(i, searchString));
            return p;
        }

        private static bool ComplexTypeContains<T>(T complexObj, string searchString)
        {
            foreach (var pi in complexObj.GetType().GetProperties())
            {
                if (pi.GetValue(complexObj, null).ToString().IndexOf(searchString.ToString(), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class DataGen
    {
        private Random GetRandomizer()
        {
            System.Threading.Thread.Sleep(1);
            return new Random(DateTime.Now.Millisecond);
        }

        public string GenerateFirstName()
        {
            string[] fNames = { "Arjun", "Larry", "Seregy", "Michael", "Albert", "Aishwarya", "Priyanka", "Genilia", "James", "Abdul", "Goerge", "Elaine", "Joey", "Amir", "Penelope", "Rani", "Anna", "Mark", "Jon", "Morgan" };

            return GetRandomValueFromStringArray(fNames);
        }

        public string GenerateLastName()
        {
            string[] lNames = { "Brin", "Page", "Shetty", "Jackson", "Einstein", "Rai", "Chopra", "D'Souza", "Bond", "Taylor",
            "Costanza", "Benes", "Tribbiani", "Khan", "Cruz", "Mukherjee", "Hazare", "Skeet", "Freeman", "Kalam"};

            return GetRandomValueFromStringArray(lNames);
        }

        private string GetRandomValueFromStringArray(string[] sArray)
        {
            Random r = GetRandomizer();

            return sArray[r.Next(sArray.Length)];
        }
    }
}