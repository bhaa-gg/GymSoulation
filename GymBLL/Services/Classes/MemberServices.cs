using AutoMapper;
using GymBLL.Services.AttachmentServices;
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
    public class MemberServices : IMemberServices
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
        public readonly IMapper _mapper;
        private readonly IAttachmentServices _attachment;

        public MemberServices(IUnitOfWork unitOfWork , IMapper mapper , IAttachmentServices attachment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachment= attachment;
        }

        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try {
                if (CheckEmailPhoneExsits(createMember.Email , createMember.Phone)) return false;
                #region oldMapp

                //var newMember = new Member()
                //{
                //    Name = createMember.Name,
                //    Phone = createMember.Phone,
                //    Email = createMember.Email,
                //    DOB = createMember.DOB,
                //    Gender = createMember.Gender.ToString(),
                //    Adress = new Adress()
                //    {
                //        Street = createMember.Street,
                //        City = createMember.City,
                //        BuildingNymber = createMember.BuildingNumber,
                //    },
                //    HealthRecord = new HealthRecord()
                //    {
                //        BloodType = createMember.HealthRecordViewModel.BloodType,
                //        Weight = createMember.HealthRecordViewModel.Weight,
                //        Height = createMember.HealthRecordViewModel.Height,
                //        Note = createMember.HealthRecordViewModel.Note,
                //    },
                //};
                #endregion


                var PhotoName= _attachment.Upload(Constant.Member.ToString(), createMember.PhotoFile);
                if(string.IsNullOrEmpty(PhotoName)) return false;

                var newMember = _mapper.Map<Member>(createMember);
                newMember.Photo = PhotoName;
                _unitOfWork.GetRepository<Member>().Add(newMember) ;
                var isCreated = _unitOfWork.SaveChanges() > 0;
                if(!isCreated)
                {
                    _attachment.Delete(PhotoName, Constant.Member.ToString());
                    return false;
                }
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }
        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var res = _unitOfWork.GetRepository<Member>().GetAll();
            if (res is null || !res.Any()) return [];

            #region old map

            //res.Select(M => new MemberViewModel () { 
            //    Id= M.Id,
            //    Name= M.Name,
            //    Photo= M.Photo,
            //    Email= M.Email,
            //    Phone= M.Phone,
            //    Gender= M.Gender.ToString(),
            //});

            #endregion
            return _mapper.Map<IEnumerable<MemberViewModel>>(res);

        }



        public MemberViewModel ? GetMemberDetails(int id)
        {
            var M = _unitOfWork.GetRepository<Member>().GetById(id);

            if (M is null) return null;


            #region old Map


            //new MemberViewModel()
            //{
            //    Id = M.Id,
            //    Name = M.Name,
            //    Photo = M.Photo,
            //    Email = M.Email,
            //    Phone = M.Phone,
            //    DOB = M.DOB.ToShortDateString(),
            //    Gender = M.Gender.ToString(),
            //    Address = $"{M.Adress.BuildingNymber} - {M.Adress.Street} -  {M.Adress.City}",

            //};
            #endregion

            var viewModel = _mapper.Map<Member, MemberViewModel>(M);

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
            return _mapper.Map<HealthRecordViewModel>(res);
            #region old map
            //new HealthRecordViewModel() {

            //    Height=res.Height,
            //    Weight = res.Weight,
            //    BloodType= res.BloodType,
            //    Note= res.Note,
            //};
            #endregion
        }

        public bool RemoveMember(int id)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(id);
            if (member is null) return false;



            var SessionIds = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(M => M.MemberId == id).Select(S=>S.SessionId);


                var HasActiveSessions = _unitOfWork.GetRepository<Session>()
                .GetAll(S => SessionIds.Contains(S.Id) && S.StartDate > DateTime.Now).Any();

            if (HasActiveSessions) return false;



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
                   var IsDeleted =  _unitOfWork.SaveChanges() > 0;
                if (IsDeleted) _attachment.Delete(member.Photo, Constant.Member.ToString());

                    
                return IsDeleted;
            }
            catch {
                return false;
            }


        }

        public bool UpdateMember(int id, UpdateMemberViewModel updateMember)
        {
            try {
                var constraints = _unitOfWork.GetRepository<Member>()
                    .GetAll(M => (M.Email == updateMember.Email || M.Phone == updateMember.Phone) && M.Id != id);


                if (constraints.Any()) return false;


                var member = _unitOfWork.GetRepository<Member>().GetById(id);
                if (member is null) return false;


                _mapper.Map(updateMember, member);
                #region old map
                //member.Adress.City = updateMember.City;
                //member.Adress.Street = updateMember.Street;
                //member.Adress.BuildingNymber = updateMember.BuildingNumber;
                //member.Email = updateMember.Email;
                //member.Phone = updateMember.Phone;
                //member.UpdatedAt = DateTime.Now;
                #endregion
                _unitOfWork.GetRepository<Member>().Update(member);
                return  _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex) {
                return false;
            }
        }




        public UpdateMemberViewModel? GetMemberAndUpdate(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);


            if (member is null) return null;
            #region old map
            //new UpdateMemberViewModel()
            //{

            //    City = member.Adress.City,
            //    Street = member.Adress.Street,
            //    BuildingNumber = member.Adress.BuildingNymber,
            //    Email = member.Email,
            //    Name = member.Name,
            //    Phone = member.Phone,
            //    Photo = member.Photo,
            //};


            #endregion

            return _mapper.Map < UpdateMemberViewModel >(member);
        }



        #region Helper Method
        private bool CheckEmailPhoneExsits(string? email, string? phone)
           => _unitOfWork.GetRepository<Member>().GetAll(M => M.Email == email || M.Phone == phone).Any();

        #endregion


    }
}
