using GymBLL.Services.Interface;
using GymBLL.ViewModels.PlanViewModels;
using GymDAL.Entities;
using GymDAL.Repositories.Classes;
using GymDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymBLL.Services.Classes
{
    public class PlanServices : IPlanServices
    {
        private readonly IUnitOfWork _unitOfWork;


        public PlanServices( IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans is null ||!plans.Any() ) return [];
            return plans.Select(P => new PlanViewModel()
            {
                Id = P.Id,
                Name = P.Name,
                Description = P.Description,
                Price = P.Price,
                DurationDays = P.DurationDays,
                IsActive = P.IsActive,
            });
        }

        public PlanViewModel? GetPlanById(int id)
        {
            var P= _unitOfWork.GetRepository<Plan>().GetById(id);
            if (P is null) return null;
            return new PlanViewModel()
            {
                Id = P.Id,
                Name = P.Name,
                Description = P.Description,
                Price = P.Price,
                DurationDays = P.DurationDays,
                IsActive = P.IsActive,
            };
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int id)
        {
            var P = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (P is null || !P.IsActive || HasActiveMemberShips(id)) return null;
            return new UpdatePlanViewModel()
            {
                Description = P.Description,
                Price = P.Price,
                DurationDays = P.DurationDays,
                PlanName = P.Name,
            };
        }
        public bool UpdatePlan(int id, UpdatePlanViewModel plan)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (Plan is null || HasActiveMemberShips(id)) return false;

            try { 
            ( Plan.Description, Plan.Price , Plan.DurationDays  , Plan.UpdatedAt) 
                = (plan.Description , plan.Price,plan.DurationDays , DateTime.Now);
            _unitOfWork.GetRepository<Plan>().Update(Plan);
            return _unitOfWork.SaveChanges() > 0;

            } catch {return false;}
        }

        public bool ToggleStatus(int id)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (Plan is null || HasActiveMemberShips(id)) return false;
            try { 
            Plan.IsActive = !Plan.IsActive;
            _unitOfWork.GetRepository<Plan>().Update(Plan);
            return _unitOfWork.SaveChanges() > 0;
            }
            catch { return false; }
        }



        #region Helpers

        private bool HasActiveMemberShips(int id) =>_unitOfWork.GetRepository<MemberShip>()
                .GetAll(M => M.PlanId == id && M.Status == "Active").Any();

        #endregion
    }
}
