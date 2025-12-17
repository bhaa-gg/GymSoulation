using GymBLL.Services.Interface;
using GymBLL.ViewModels.MemberViewModels;
using GymDAL.Entities;
using GymDAL.Entities.Enums;
using GymDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymBLL.Services.Classes
{
    internal class MemberServices : IMemberServices
    {
        #region old
        //private readonly IGenericRepository<Member> _memberRepository;
        //private readonly IGenericRepository<MemberShip> _membershipRepository;
        //private readonly IGenericRepository<HealthRecord> _healthRecordRepository;
        //private readonly IGenericRepository<MemberSession> _memberSessionRepository;
        //private readonly IPlanRepository _planRepository;

        #endregion


        #region old

        //public MemberServices(
        //    IGenericRepository<Member> memberRepository,
        //    IGenericRepository<MemberShip> membershipRepository,
        //    IPlanRepository planRepository,
        //    IGenericRepository<HealthRecord> healthRecordRepository,
        //    IGenericRepository<MemberSession> memberSessionRepository
        //    )
        //{
        //    _memberRepository = memberRepository;
        //    _membershipRepository = membershipRepository;
        //    _healthRecordRepository = healthRecordRepository;
        //    _memberSessionRepository = memberSessionRepository;
        //    _planRepository = planRepository;
        //}

        #endregion

        private readonly IUnitOfWork _unitOfWork;

        public MemberServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try {
               

                if (CheckEmailPhoneExsits(createMember.Email , createMember.Phone)) return false;

                var newMember = new Member()
                {
                    Name = createMember.Name,
                    Phone = createMember.Phone,
                    Email = createMember.Email,
                    DOB = createMember.DOB,
                    Gender = createMember.Gender.ToString(),
                    Adress = new Adress()
                    {
                        Street = createMember.Street,
                        City = createMember.City,
                        BuildingNymber = createMember.BuildingNumber,
                    },
                    HealthRecord = new HealthRecord()
                    {
                        BloodType = createMember.HealthRecordViewModel.BloodType,
                        Weight = createMember.HealthRecordViewModel.Weight,
                        Height = createMember.HealthRecordViewModel.Height,
                        Note = createMember.HealthRecordViewModel.Note,
                    },
                };
                    _unitOfWork.GetRepository<Member>().Add(newMember) ;
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex) {
                return false;
            }
        }
        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var res = _unitOfWork.GetRepository<Member>().GetAll();
            if (res is null || !res.Any()) return []; 

            return res.Select(M => new MemberViewModel () { 
                Id= M.Id,
                Name= M.Name,
                Photo= M.Photo,
                Email= M.Email,
                Phone= M.Phone,
                Gender= M.Gender.ToString(),
            });

        }

        public UpdateMemberViewModel ? GetMemberAndUpdate(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member is null) return null;
            return new UpdateMemberViewModel() {
            
                City=member.Adress.City,
                Street=member.Adress.Street,
                BuildingNumber = member.Adress.BuildingNymber,
                Email = member.Email,
                Name = member.Name,
                Phone= member.Phone,
                Photo= member.Photo,
            };

        }

        public MemberViewModel ? GetMemberDetails(int id)
        {
            var M = _unitOfWork.GetRepository<Member>().GetById(id);

            if (M is null) return null;


            var viewModel = new MemberViewModel() {
                Id = M.Id,
                Name = M.Name,
                Photo = M.Photo,
                Email = M.Email,
                Phone = M.Phone,
                DOB = M.DOB.ToShortDateString(),
                Gender = M.Gender.ToString(),
                Address= $"{M.Adress.BuildingNymber} - {M.Adress.Street} -  {M.Adress.City}",

            };

            var ActiveMemberShip = _unitOfWork.GetRepository<MemberShip>().GetAll(X => X.Status == "Active" && X.Id == id ).FirstOrDefault();

            if (ActiveMemberShip is null) return viewModel;

            viewModel.MembershipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
            viewModel.MembershipEndDate = ActiveMemberShip.EndDate.ToShortDateString();

            var plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId);

            viewModel.PlanName = plan?.Name;
            return viewModel;
            
        }

        public HealthRecordViewModel? GetMemberHealthRecord(int MemberId)
        {
            var res = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (res is null) return null;
            return new HealthRecordViewModel() {

                Height=res.Height,
                Weight = res.Weight,
                BloodType= res.BloodType,
                Note= res.Note,
            };
        }

        public bool RemoveMember(int id)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(id);
            if (member is null) return false;



            var HasActiveSessions = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(M => M.MemberId == id && M.Session.StartDate > DateTime.Now).Any();

            if (!HasActiveSessions) return false;



            var MemberShips= _unitOfWork.GetRepository<MemberShip>()
                .GetAll(M => M.MemberId == id );


            try {
                if (MemberShips.Any())
                {
                    foreach (var item in MemberShips)
                    {
                        _unitOfWork.GetRepository<MemberShip>().Delete(item);
                    }
                }
                    _unitOfWork.GetRepository<Member>().Delete(member) ;
                   return  _unitOfWork.SaveChanges() > 0;
            }
            catch {
                return false;
            }


        }

        public bool UpdateMember(int id, UpdateMemberViewModel updateMember)
        {
            try {

                if (CheckEmailPhoneExsits(updateMember.Email, updateMember.Phone)) return false;


                var member = _unitOfWork.GetRepository<Member>().GetById(id);
                if (member is null) return false;


                member.Adress.City = updateMember.City;
                member.Adress.Street = updateMember.Street;
                member.Adress.BuildingNymber = updateMember.BuildingNumber;
                member.Email = updateMember.Email;
                member.Phone = updateMember.Phone;
                member.UpdatedAt= DateTime.Now;


                _unitOfWork.GetRepository<Member>().Update(member);
                return  _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex) {
                return false;
            }
        }



        #region Helper Method
        private bool CheckEmailPhoneExsits(string? email, string? phone)
           => _unitOfWork.GetRepository<Member>().GetAll(M => M.Email == email || M.Phone == phone).Any();

        #endregion


    }
}
