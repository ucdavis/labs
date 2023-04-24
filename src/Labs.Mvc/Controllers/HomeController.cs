using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Labs.Mvc.Models;
using AnlabMvc.Helpers;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace Labs.Mvc.Controllers
{
    public class HomeController : SuperController
    {
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            var conn = this.configuration.GetConnectionString("DefaultConnection");

            using (var db = new DbManager(conn))
            {

                var terms = await db.Connection.QueryAsync<TermModel>(Queries.Terms);
                var model = new BulkModel();
                model.Terms = terms.ToList();
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(BulkModel model)
        {

            var conn = this.configuration.GetConnectionString("DefaultConnection");

            using (var db = new DbManager(conn))
            {

                var terms = await db.Connection.QueryAsync<TermModel>(Queries.Terms); // TODO: figure out a way to not have to fetch this again
                model.Terms = terms.ToList();
                if (String.IsNullOrWhiteSpace(model.SearchCourses) || String.IsNullOrWhiteSpace(model.SearchTerm))
                {
                    ErrorMessage = "You must select something to search";
                    return View(model);
                }

                const string regex = @"\d{5}";
                var courses = System.Text.RegularExpressions.Regex.Matches(model.SearchCourses, regex).Select(x => x.Value).ToArray();
                var students = await db.Connection.QueryAsync<StudentModel>(Queries.StudentsForCourse(courses, model.SearchTerm));

                var studentIds = students.Select(x => x.Id).ToList().Distinct();

                var result = from student in students.Distinct()
                             select new StudentXCardModel
                             {
                                FirstName = student.FirstName,
                                LastName = student.LastName,
                                Id = student.Id,
                                Email = student.Email,
                                Program = student.Program
                             };
                model.Terms = terms.ToList();
                model.StudentsWithoutCards = result.Where(x => String.IsNullOrEmpty(x.CardId)).ToList();
                return View(model);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Query()
        {
            var conn = this.configuration.GetConnectionString("DefaultConnection");

            using (var db = new DbManager(conn))
            {

                var result = db.Connection.Query(Queries.CardholdInfo, new { ids = new[] { "999811562" } });
                return Json(result);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool checkValid(StudentXCardModel student) {
            if(student.nActive == "0" || student.dtExpirationDate < DateTime.Today)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
