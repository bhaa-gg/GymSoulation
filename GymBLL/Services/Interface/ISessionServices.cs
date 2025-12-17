using GymBLL.ViewModels.SessionViewModels;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBLL.Services.Interface
{
    public interface ISessionServices
    {
        IEnumerable<SessionViewModel> GetAllSession();
        SessionViewModel ?GetSessionById(int sessionId);


        bool CreateSession(CreateSessionViewModel model);
        bool RemoveSession(int sessionId);


        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);
        bool UpdateSession(int sessionId , UpdateSessionViewModel updatedSession );

    }
}
