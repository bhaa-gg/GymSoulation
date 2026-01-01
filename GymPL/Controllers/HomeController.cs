using GymBLL.Services.Interface;
using GymDAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticService _analyticService;

        public HomeController(IAnalyticService analyticService)
        {
            _analyticService = analyticService;
        }

        public ActionResult Index()
        {
            var Data = _analyticService.GetAnalyticData();

            return View(Data);
        }


        #region TRY


        //[NonAction] // not have route
        //public ViewResult Index()
        //{
        //    return new  ViewResult();
        //}

        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public JsonResult Trainer()
        //{
        //    return Json(
        //        new List<Trainer>()
        //        {
        //            new(){Name ="Bahaa", Phone="01210031428"},
        //            new(){Name ="Ahmed", Phone="01210031428"}

        //        }
        //        );
        //}

        //public RedirectResult Bahaa()
        //{
        //    return Redirect("https://enginerbhaa.netlify.app/");
        //}


        //public ContentResult Privacy()
        //{
        //    return Content("<h1>Heellooo</h1>" , "text/html");
        //}

        //public FileResult DownloadFile()
        //{
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "css", "site.css");
        //    var fileBytes = System.IO.File.ReadAllBytes(filePath);
        //    return File(fileBytes, "text/css", "DownloadbleFile.css");
        //}
        //public ActionResult EmptyAction()
        //{
        //return new EmptyResult();
        //}
        #endregion

    }
}
