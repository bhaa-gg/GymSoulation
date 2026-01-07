using GymBLL.Services.Interface;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymPL.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionServices _sessionServices;

        public SessionController(ISessionServices sessionServices )
        {
            _sessionServices = sessionServices;
        }
        public IActionResult Index()
        {
            var Sessions = _sessionServices.GetAllSession();
            return View(Sessions);
        }

        public IActionResult Details([FromRoute]int id) {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Session Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var Session = _sessionServices.GetSessionById(id);
            if (Session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Session);
        }



        public IActionResult Create()
        {
            PopulateCategories();
            PopulateTrainers();

            return View();
        }


        [HttpPost]
        public IActionResult Create(CreateSessionViewModel newSession)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("ErrField", "Invalid Data Check it Again");
                PopulateCategories();
                PopulateTrainers();
                return View(newSession);
            }

            var isCreated = _sessionServices.CreateSession(newSession);

            if (!isCreated)
            {
                
                ModelState.AddModelError("ErrField", "Can't Create Session , Try Again");
                PopulateCategories();
                PopulateTrainers();

                return View(newSession);    
            }

            TempData["SuccessMessage"] = "Session Created Successfully";
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int  id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Session Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var sessionToUpdate = _sessionServices.GetSessionToUpdate(id);

            if (sessionToUpdate is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            PopulateTrainers();
            return View(sessionToUpdate);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute]int id , UpdateSessionViewModel UpdatedSession)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Session Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("ErrField", "Invalid Data Check it Again");
                return View(UpdatedSession);
            }

            var res = _sessionServices.UpdateSession(id , UpdatedSession);
            if (!res)
            {
                ModelState.AddModelError("ErrField", "Can't Update Session , Try Again");
                return View(UpdatedSession);

            }
            TempData["SuccessMessage"] = "Session Updated Successfully";
            return RedirectToAction(nameof(Index));
        }





        public IActionResult Delete([FromRoute]int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Session Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var sessionToDelete = _sessionServices.GetSessionById(id);
            if (sessionToDelete is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;
            return  View();
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Session Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var res = _sessionServices.RemoveSession(id);

            if (!res) TempData["ErrorMessage"] = "Can't Delete Session , Try Again";
            else TempData["SuccessMessage"] = "Session Deleted Success ";

            return RedirectToAction(nameof(Index));
        }

        #region Helpers

        private void PopulateTrainers()
        {
            var Trainers = _sessionServices.GetAllTrainersForSelect();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }
        private void PopulateCategories()
        {
            var Categories = _sessionServices.GetAllCategoriesForSelect();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }

        #endregion
    }
}
