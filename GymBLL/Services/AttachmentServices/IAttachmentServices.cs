using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GymBLL.Services.AttachmentServices
{
    public interface IAttachmentServices
    {
        string ?Upload(string  folderName, IFormFile file);

        bool Delete(string fileName , string folderName);
    }
}
