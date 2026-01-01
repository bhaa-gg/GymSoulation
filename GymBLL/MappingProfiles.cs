using AutoMapper;
using GymBLL.ViewModels.MemberViewModels;
using GymBLL.ViewModels.SessionViewModels;
using GymDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            ConfigureSessionMappings();
            ConfigureMemberMappings();


            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>().
                ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));


        }



        private void ConfigureSessionMappings()
        {

            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, option => option.MapFrom(src => src.SessionCategory.CategoryName))
                .ForMember(dest => dest.TrainerName, option => option.MapFrom(src => src.SessionTrainer.Name))
                .ForMember(dest => dest.AvailableSlots, option => option.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }
        private void ConfigureMemberMappings()
        {


            
            
            //CreateMap<CreateMemberViewModel, Member>()
            //    .ForMember(dest=>dest.Adress , opt=>opt.MapFrom(src=> new Adress() { 
            //    BuildingNymber= src.BuildingNumber,
            //    City= src.City,
            //    Street= src.Street,
            //    }));

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Adress, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));


            CreateMap<CreateMemberViewModel, Adress>()
                .ForMember(dest => dest.BuildingNymber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street));

            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();




            CreateMap<Member, MemberViewModel>()
                .ForMember(dest=>dest.Gender   , opt=>opt.MapFrom(src=> src.Gender.ToString()))
                .ForMember(dest=>dest.Address ,opt =>opt.MapFrom(src=> $"{src.Adress.BuildingNymber} - {src.Adress.Street} -  {src.Adress.City}"))
                .ForMember(dest=>dest.DOB   , opt=>opt.MapFrom(src=> src.DOB.ToShortDateString()));




            CreateMap<Member, UpdateMemberViewModel>()
                .ForMember(dist => dist.BuildingNumber, opt => opt.MapFrom(src => src.Adress.BuildingNymber))
                .ForMember(dist => dist.Street, opt => opt.MapFrom(src => src.Adress.Street))
                .ForMember(dist => dist.City, opt => opt.MapFrom(src => src.Adress.City));



            CreateMap<UpdateMemberViewModel, Member>()
                .ForMember(dist => dist.Name, opt => opt.Ignore())
                .ForMember(dist => dist.Photo, opt => opt.Ignore())
                .AfterMap((s, d)=>  // if change in base object
                {
                    d.Adress.BuildingNymber = s.BuildingNumber;
                    d.Adress.Street = s.Street;
                    d.Adress.City = s.City;
                    d.UpdatedAt = DateTime.Now;
                });

        }
    }
}
