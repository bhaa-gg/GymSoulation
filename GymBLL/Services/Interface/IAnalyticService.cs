using GymBLL.ViewModels.AnalyticViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBLL.Services.Interface
{
    public interface IAnalyticService
    {
        AnalyticViewModel GetAnalyticData();
    }
}
