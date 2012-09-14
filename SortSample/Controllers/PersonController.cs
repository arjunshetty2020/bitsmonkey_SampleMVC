using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SortSample.Models;
using SortSample.Models.Util;
using PagedList;
using SortSample.Properties;
using System.Xml.Linq;
using System.IO;

namespace SortSample.Controllers
{
    public class PersonController : Controller
    {
        private static int _defaultPageSize = Settings.Default.DefaultPageSize;//gets the configurable page size from the web.config file

        static List<Person> _persons;

        /// <summary>
        /// CONSTRUCTOR : 
        /// fill person list from the xml file in the App_Data folder
        /// </summary>
        public PersonController()
        {
            var doc = XDocument.Load(Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "sampledata.xml"));
            _persons = (from n in doc.Descendants("record") select new Person() { FirstName = n.Element("FirstName").Value, LastName = n.Element("LastName").Value }).ToList();
        }

        //
        // GET: /Person/

        /// <summary>
        /// Index view for Person
        /// </summary>
        /// <param name="q">Search string</param>
        /// <param name="s">Name of thr property by which the list is to be sorted</param>
        /// <param name="p">Page number</param>
        /// <returns>A view with paged list of Person object</returns>
        /// 
        public ActionResult Index(string q = "", string s = "FirstName", int p = 1)
        {
            var pl = _persons.Search<Person>(q).Sort<Person>(s).ToPagedList(p, _defaultPageSize);

            return View(pl);
        }

    }
}
