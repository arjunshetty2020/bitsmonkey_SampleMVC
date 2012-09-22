using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SampleMVC.Models;
using SampleMVC.Models.Util;
using PagedList;
using SampleMVC.Properties;
using System.Xml.Linq;
using System.IO;

namespace SampleMVC.Controllers
{
    public class PersonController : Controller
    {
        private static int _defaultPageSize = Settings.Default.DefaultPageSize;//gets the configurable page size from the web.config file
        private static string _xmlPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "sampledata.xml");

        static List<Person> _persons;

        /// <summary>
        /// CONSTRUCTOR : 
        /// fill person list from the xml file in the App_Data folder
        /// </summary>
        public PersonController()
        {
            var doc = XDocument.Load(_xmlPath);
            _persons = (from n in doc.Descendants("record")
                        select new Person()
                        {
                            Id = int.Parse(n.Element("Id").Value),
                            FirstName = n.Element("FirstName").Value,
                            LastName = n.Element("LastName").Value
                        }).ToList();
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

        public ActionResult Edit(int id)
        {
            return View(_persons.FirstOrDefault(p => p.Id == id));
        }

        [HttpPost]
        public ActionResult Edit(Person p)
        {
            var xdoc = XDocument.Load(_xmlPath);
            XElement e = xdoc
                .Descendants("record")
                .First(i => int.Parse(i.Element("Id").Value) == p.Id);

            e.SetElementValue("FirstName", p.FirstName);
            e.SetElementValue("LastName", p.LastName);

            xdoc.Save(_xmlPath);

            return RedirectToAction("Index");
        }
    }
}
