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

namespace Labs.Mvc.Controllers
{
    public class HomeController : Controller
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
            if (String.IsNullOrWhiteSpace(model.SearchCourses) || String.IsNullOrWhiteSpace(model.SearchTerm))
            {
                throw new Exception();
            }

            var conn = this.configuration.GetConnectionString("DefaultConnection");

            const string regex = @"\d{5}";
            var courses = System.Text.RegularExpressions.Regex.Matches(model.SearchCourses, regex).Select(x => x.Value).ToArray();
            using (var db = new DbManager(conn))
            {

                var terms = await db.Connection.QueryAsync<TermModel>(Queries.Terms);
                var students = await db.Connection.QueryAsync<StudentModel>(Queries.StudentsForCourse(courses, model.SearchTerm));

                var studentIds = students.Select(x => x.Id).ToList().Distinct();
                var cards = await db.Connection.QueryAsync<CardModel>(Queries.CardholdInfo, new { ids = studentIds });
                var result = from student in students.Distinct()
                             join card in cards on student.Id equals card.strEmployeeId into cardGroup
                             from item in cardGroup.DefaultIfEmpty(new CardModel { CardsId = String.Empty })
                             select new StudentXCardModel
                             {
                                FirstName = student.FirstName,
                                LastName = student.LastName,
                                Id = student.Id,
                                CardId = item.CardsId,
                                nCardholderId = item.nCardholderId,
                                Department = item.Department,
                                strEncodedCardNumber = item.strCardFormatName,
                                dtExpirationDate = item.dtExpirationDate,
                                nActive = item.nActive,
                                Access1 = item.Access1,
                                Access2 = item.Access2,
                                nFacilityCode = item.nFacilityCode,
                                strCardFormatName = item.strCardFormatName,
                                Email = student.Email,
                                Program = student.Program
                             };
                model.Terms = terms.ToList();
                model.StudentsWithCards = result.Where(x => !String.IsNullOrEmpty(x.CardId)).ToList();
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
    }
}
