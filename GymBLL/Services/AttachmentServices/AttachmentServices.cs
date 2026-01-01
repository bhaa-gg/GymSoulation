using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBLL.Services.AttachmentServices
{
    public class AttachmentServices : IAttachmentServices
    {
        private readonly string[] AllowedExtension = { ".jpg", ".png", ".jpeg" };
        private readonly long MaxFileSize = 5 * 1024 * 1024;

        //public AttachmentServices(IWebHostEnvironment env)
        //{

        
        //}


        public string? Upload(string folderName, IFormFile file)
        {
            try {
                if (folderName is null || file is null || file.Length <= 0 || file.Length > MaxFileSize) return null;


                var FileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!AllowedExtension.Contains(FileExtension)) return null;


                var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName);
                if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);


                var fileName = Guid.NewGuid().ToString() + FileExtension;

                var FilePath = Path.Combine(FolderPath, fileName);


                using var fileStream = new FileStream(FilePath, FileMode.Create);

                file.CopyTo(fileStream);
                return fileName;
            }
            catch  (Exception ex)
            {
                Console.WriteLine($"Failed To Upload File To Folder = {folderName} : {ex}");
                return null;
            }
        }
        public bool Delete(string fileName, string folderName)
        {
            try
            {

                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return false;

                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images" , folderName,fileName);

                if (!File.Exists(fullPath)) return false;
                File.Delete(fullPath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Delete File To Folder = {folderName} : {ex}");
                return false;
            }
        }


    }
}
