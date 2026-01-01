using GymBLL.Services.Interface;
using GymDAL.Entities;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementSystemBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;

namespace GymPL.Controllers
{
    public class TrainerController : Controller 
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService )
        {
            _trainerService = trainerService;
        }

        public ActionResult Index()
        {
            var trainer = _trainerService.GetAllTrainers();
            return View(trainer);
        }


        public ActionResult Create()
        {
            //var trainer = _trainerService.GetAllTrainers();
            return View();
        }


        [HttpPost]
        public ActionResult Create(CreateTrainerViewModel createdTrainer)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Your Data Please.");
                return View(createdTrainer);
            }
            var res = _trainerService.CreateTrainer(createdTrainer);
            if(res) {
                TempData["SuccessMessage"] = "Trainer created successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("DataInvalid", "Unable to create trainer with the provided details.");
            }
                return View();
        }

        public ActionResult TrainerDetails([FromRoute]int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Trainer Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerDetails(id);
            if(trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }


        public ActionResult Edit([FromRoute] int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Trainer Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerToUpdate(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute] int id , TrainerToUpdateViewModel trainer)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Trainer Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Please Check Data And Fields");
                return View(trainer);
            }
            var updateTrainer = _trainerService.UpdateTrainerDetails(trainer,id );
            if (!updateTrainer)
            {
                ModelState.AddModelError("DataInvalid", "Trainer Not Updated Success");
                return View(updateTrainer);
            }
            TempData["SuccessMessage"] = "Trainer Updated Success";

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete([FromRoute] int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Trainer Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = _trainerService.GetTrainerDetails(id);
            if(Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = id;
            ViewBag.TrainerName= Trainer.Name;

            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm]  int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Trainer Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var res = _trainerService.RemoveTrainer(id);
            if(res)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Not Deleted Successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

    }


}
