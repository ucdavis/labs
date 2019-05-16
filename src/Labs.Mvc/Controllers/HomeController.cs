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
            
            using (var db = new DbManager(conn)) {

                var terms  = await db.Connection.QueryAsync<TermsModel>(Queries.Terms);
                return View(terms.ToList());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Query() {
            var conn = this.configuration.GetConnectionString("DefaultConnection");
            
            using (var db = new DbManager(conn)) {

                var result = db.Connection.Query(Queries.CardholdInfo, new { ids = new [] { "999811562" }});
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
