using AutoMapper;
using GymBLL.Services.Interface;
using GymBLL.ViewModels.SessionViewModels;
using GymDAL.Entities;
using GymDAL.Repositories.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBLL.Services.Classes
{
    public class SessionServices : ISessionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;


        public SessionServices(IUnitOfWork unitOfWork, IMapper mapper) {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public IEnumerable<SessionViewModel> GetAllSession()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
            if (!Sessions.Any() || Sessions is null) return [];


            var MappSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Sessions);

            foreach (var item in MappSessions)
            {
                item.AvailableSlots = item.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(item.Id);
            }

            //MappSessions.Select(S => 
            //{
            //    S.AvailableSlots = S.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(S.Id);
            //    return S;
            //});

            return MappSessions;
            #region Old Map

            //return Sessions.Select(S => new SessionViewModel()
            //{
            //    Id = S.Id,
            //    StartDate = S.StartDate,
            //    EndDate = S.EndDate,
            //    Capacity = S.Capacity,
            //    CategoryName = S.SessionCategory.CategoryName,
            //    Description = S.Description,
            //    TrainerName = S.SessionTrainer.Name,
            //    AvailableSlots = S.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(S.Id)
            //});
            #endregion
        }


        public SessionViewModel? GetSessionById(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCategory(sessionId);
            if (session is null) return null;
            var res = _mapper.Map<Session, SessionViewModel>(session);
            res.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            return res;


            #region Old Map

            //return new SessionViewModel()
            //{
            //    Id = session.Id,
            //    StartDate = session.StartDate,
            //    EndDate = session.EndDate,
            //    Capacity = session.Capacity,
            //    CategoryName = session.SessionCategory.CategoryName ,
            //    Description = session.Description,
            //    TrainerName = session.SessionTrainer.Name,
            //    AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id)
            //};
            #endregion
        }

        public bool CreateSession(CreateSessionViewModel model)
        {
            try
            {
                if (model.Capacity < 0 || model.Capacity > 25 || !IsTrainnerExists(model.TrainerId) || !IsCategoryExists(model.CategoryId) || !IsValidDateRange(model.StartDate, model.EndDate))
                    return false;

                var SessionEntity = _mapper.Map<Session>(model);
                _unitOfWork.GetRepository<Session>().Add(SessionEntity);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var Session = _unitOfWork.GetRepository<Session>().GetById(sessionId);
            if(Session is null || !IsSessionAvilableForUpdating(Session)) return null;
            return _mapper.Map<UpdateSessionViewModel>(Session);
        }

        public bool UpdateSession(int sessionId, UpdateSessionViewModel updatedSession)
        {


            try
            {

                var Session = _unitOfWork.GetRepository<Session>().GetById(sessionId);
                if (Session is null || !IsSessionAvilableForUpdating(Session)) return false;


                if (!IsTrainnerExists(updatedSession.TrainerId) || !IsValidDateRange(updatedSession.StartDate, updatedSession.EndDate))
                    return false;

                _mapper.Map(updatedSession , Session);
                Session.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepository<Session>().Update(Session);
                return _unitOfWork.SaveChanges() > 0;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public bool RemoveSession(int sessionId)
        {
            try
            {
                var Session = _unitOfWork.GetRepository<Session>().GetById(sessionId);
                if (Session is null || !IsSessionAvilableForDelete(Session)) return false;




                _unitOfWork.GetRepository<Session>().Delete(Session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }




        public IEnumerable<TrainerSelectViewModel> GetAllTrainersForSelect()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public IEnumerable<CategorySelectViewModel> GetAllCategoriesForSelect()
        {
            var Category = _unitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(Category);
        }



        #region Helpers

        private bool IsTrainnerExists(int TrainerId) => _unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;
        private bool IsCategoryExists(int CategoryId) => _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        private bool IsSessionAvilableForUpdating(Session session) {

            if(session is null ) return false;

            if(session.EndDate < DateTime.Now) return false;

            if (session.StartDate <= DateTime.Now) return false;

            var HasActiveBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            return HasActiveBookings ? false : true;
        }

        private bool IsSessionAvilableForDelete(Session session)
        {

            if (session is null) return false;


            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;


            if (session.StartDate > DateTime.Now) return false;

            var HasActiveBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            return HasActiveBookings ? false : true;
        }

        private bool IsValidDateRange(DateTime StartDate, DateTime EndDate) => StartDate < EndDate;

       

        #endregion

    }
}
