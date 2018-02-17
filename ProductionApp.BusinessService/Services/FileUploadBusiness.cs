using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class FileUploadBusiness: IFileUploadBusiness
    {
        IFileUploadRepository _fileRepository;
        public FileUploadBusiness(IFileUploadRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        public FileUpload InsertAttachment(FileUpload fileUploadObj)
        {
            return _fileRepository.InsertAttachment(fileUploadObj);
        }
        public List<FileUpload> GetAttachments(Guid ID)
        {
            return _fileRepository.GetAttachments(ID);
        }
        public object DeleteFile(Guid ID)
        {
            return _fileRepository.DeleteFile(ID);
        }
    }
}
