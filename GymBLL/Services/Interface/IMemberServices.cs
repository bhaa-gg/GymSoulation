using GymBLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBLL.Services.Interface
{
    public interface IMemberServices
    {
        IEnumerable<MemberViewModel> GetAllMembers();

        bool CreateMember(CreateMemberViewModel createMember);

        MemberViewModel? GetMemberDetails(int id);


        HealthRecordViewModel ? GetMemberHealthRecord(int MemberId);


        UpdateMemberViewModel ?GetMemberAndUpdate(int MemberId);

        bool UpdateMember( int id, UpdateMemberViewModel updateMember);
        bool RemoveMember( int id);


    }
}
