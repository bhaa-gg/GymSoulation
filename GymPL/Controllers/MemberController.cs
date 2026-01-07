using GymBLL.Services.Classes;
using GymBLL.Services.Interface;
using GymBLL.ViewModels.MemberViewModels;
using GymDAL.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymPL.Controllers
{
    [Authorize(Roles = "SUPERADMIN")]
    public class MemberController : Controller
    {
        private readonly IMemberServices _memberServices;
        #region old
        //public IActionResult Index(int id)
        //{
        //    //return RedirectToAction(nameof(GetMembers));
        //    return Content("Home/Index/" + id);
        //}
        //public ActionResult GetMembers()
        //{
        //    return Content("<h1>GetMembers</h1>", "text/html");
        //}

        //public ActionResult CreateMember()
        //{
        //    return View();
        //}
        #endregion


        public MemberController( IMemberServices MemberServices)
        {
            _memberServices = MemberServices;
        }


        // get all members
        public ActionResult Index(int id) {
            var Members = _memberServices.GetAllMembers();
            return View(Members);


        }

        public ActionResult MemberDetails(int id)
        {
            if (id <= 0) {
                TempData["ErrorMessage"] = "Member Id Can't be 0 or Negative Number";
            return RedirectToAction(nameof(Index));
            }
            var Member = _memberServices.GetMemberDetails(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }
        
        public ActionResult HealthRecordDetails(int id)
            {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Member Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberServices.GetMemberHealthRecord(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
            }

     
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel NewMember)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Please Check Data And Fields");
                return View(nameof(Create), NewMember);
            }


            var res  = _memberServices.CreateMember(NewMember);

            if(!res) TempData["ErrorMessage"] = "Member Not Created";
            else TempData["SuccessMessage"] = "Member Created Successfully";

            return RedirectToAction(nameof(Index));
        }

        public ActionResult EditMember(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Member Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberServices.GetMemberAndUpdate(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }


        [HttpPost]
        public ActionResult EditMember([FromRoute]int id  , UpdateMemberViewModel Member) {

           

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Member Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Please Check Data And Fields");
                return View(Member);
            }
            var updateMember = _memberServices.UpdateMember(id , Member);
            if (!updateMember)
            {
                ModelState.AddModelError("DataInvalid", "Member Not Updated Success Check Constrain");
                return View(Member);
            }

            TempData["SuccessMessage"] = "Member Updated Success";
            return RedirectToAction(nameof(Index));
        }




        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Member Id Can't be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var memberExist = _memberServices.GetMemberDetails(id);

            if(memberExist is null) {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MemberId = memberExist.Id;
            ViewBag.MemberName= memberExist.Name;
            return View();
          
        }


        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var deleteMember = _memberServices.RemoveMember(id);
            if (!deleteMember)TempData["ErrorMessage"] = "Member Not Deleted Success";
            else TempData["SuccessMessage"] = "Member Deleted Success";
            return RedirectToAction(nameof(Index));
        }

    }
}
