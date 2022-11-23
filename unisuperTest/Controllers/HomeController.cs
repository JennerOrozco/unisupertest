using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using uniSuperTest.Models;
using System.Configuration;

namespace uniSuperTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Report()
        {

            SqlConnection cnn = new SqlConnection("Server=tcp:unisuper.database.windows.net,1433;Initial Catalog=unisuper;Persist Security Info=False;User ID=adminJenner;Password=02Clave**.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SaleReport";
            cnn.Open();

            IList<ReportItem> rep = new List<ReportItem>();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ReportItem repI = new ReportItem();
                    repI.Id =reader.GetString(0);
                    repI.Make = reader.GetString(1);
                    repI.Model = reader.GetString(2);
                    repI.Year = reader.GetInt32(3);
                    repI.Color = reader.GetString(4);
                    repI.Price = reader.GetDecimal(5);

                    if (!reader.IsDBNull(6))
                    {
                        repI.Date = reader.GetDateTime(6);
                    }
                    else {
                        repI.Date = null;
                    }
                    
                    rep.Add(repI); // provided that first (0-index) column is int which you are looking for
                }
            }
            cnn.Close();
            ViewData["Report"] = rep;

            return View();
        }
      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

public class ReportItem
{
    public string Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public decimal Price { get; set; }
    public DateTime? Date { get; set; }
}
