using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SortSample.Models.Util
{
    public static class Extenstion
    {
        /// <summary>
        /// An extesion to IEnumberable type that will sort the list according to the sortString
        /// </summary>
        /// <typeparam name="T">The type of the items in the list</typeparam>
        /// <param name="lst">The list which is to be sorted</param>
        /// <param name="sortString">the sortString will specify by which property the list is to be sorted and oder (Asc or Desc)</param>
        /// <returns>The sorted list</returns>
        /// 
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

        /// <summary>
        /// Searches for the string in each property of the individual items of the list
        /// </summary>
        /// <typeparam name="T">Type of the individual item in the list</typeparam>
        /// <param name="lst">List which has to be searched</param>
        /// <param name="searchString">The string which is to searched</param>
        /// <returns>The list which contains the searchString</returns>
        /// 
        public static IEnumerable<T> Search<T>(this IEnumerable<T> lst, string searchString) where T : class
        {
            var p = lst.Where(i => ComplexTypeContains<T>(i, searchString));
            return p;
        }

        /// <summary>
        /// Checks if the string passed is present in any of the properties of the complex object
        /// </summary>
        /// <typeparam name="T">The type of the complex object</typeparam>
        /// <param name="complexObj">the complex object of whose the properties is checked against the string</param>
        /// <param name="searchString">the string which is to be checked against each property</param>
        /// <returns>true if searchString is contained in one of the property else returns false</returns>
        private static bool ComplexTypeContains<T>(T complexObj, string searchString)
        {
            foreach (var pi in complexObj.GetType().GetProperties())
            {
                if ((pi.GetValue(complexObj, null) ?? false).ToString().IndexOf(searchString.ToString(), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }
    }

}