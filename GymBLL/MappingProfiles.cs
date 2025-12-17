using AutoMapper;
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
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName,option => option.MapFrom(src => src.SessionCategory.CategoryName))
                .ForMember(dest => dest.TrainerName,option => option.MapFrom(src => src.SessionTrainer.Name))
                .ForMember(dest => dest.AvailableSlots,option => option.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap< Session, UpdateSessionViewModel>().ReverseMap();

       }
    }
}
