using GymBLL.Services.Interface;
using GymBLL.ViewModels.PlanViewModels;
using GymDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymPL.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
        private readonly IPlanServices _planServices;

        public PlanController(IPlanServices planServices)
        {
            _planServices = planServices;
        }
        public IActionResult Index()
        {
            var plans = _planServices.GetAllPlans() ;
            return View(plans);
        }
        public IActionResult Details([FromRoute]int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Plan Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planServices.GetPlanById(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        public IActionResult Edit([FromRoute]int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Plan Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planServices.GetPlanToUpdate(id);
            return View(plan);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id , UpdatePlanViewModel UpdatedPlan)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Plan Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check All Fields");
                return View(UpdatedPlan);
            }


            var planUpdaed = _planServices.UpdatePlan(id , UpdatedPlan);
            if(!planUpdaed)
            {
                ModelState.AddModelError("WrongData", "Unable to Update Plan , Please Try Again");
                return View(UpdatedPlan);
            }

            TempData["SuccessMessage"] = "Plan Updated Successfully";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Activate( int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Plan Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var toggled = _planServices.ToggleStatus(id);
            if(toggled)
                TempData["SuccessMessage"] = "Plan Status Toggled Successfully";
            else
                TempData["ErrorMessage"] = "Unable to Toggle Plan Status , Please Try Again";
            return RedirectToAction("Index");
        }




    }

}
