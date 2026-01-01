using AutoMapper.Execution;
using GymBLL.Services.Interface;
using GymBLL.ViewModels.AnalyticViewModels;
using GymDAL.Entities;
using GymDAL.Repositories.Classes;
using GymDAL.Repositories.Interfaces;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBLL.Services.Classes
{
    public class AnalyticService : IAnalyticService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticService(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork; 
        }
        public AnalyticViewModel GetAnalyticData()
        {
            return new AnalyticViewModel()
            {

                ActiveMembers = _unitOfWork.GetRepository<MemberShip>().GetAll(M=>M.Status == "Active").Count(),
                TotalMembers = _unitOfWork.GetRepository<GymDAL.Entities.Member>().GetAll().Count(),
                TotalTrainers= _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                CompletedSessions = _unitOfWork.GetRepository<Session>().GetAll(S => S.EndDate  < DateTime.Now).Count(),
                UpcomingSessions = _unitOfWork.GetRepository<Session>().GetAll(S => S.StartDate > DateTime.Now).Count(),
                OngoingSessions = _unitOfWork.GetRepository<Session>().GetAll(S => S.StartDate <= DateTime.Now && S.EndDate >= DateTime.Now).Count(),
            };
            }
    }
}
