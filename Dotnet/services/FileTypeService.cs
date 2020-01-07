using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sabio.Services
{
    public class FileTypeService : IFileTypeService
    {
        IDataProvider _data = null;

        public FileTypeService(IDataProvider data)
        {
            _data = data;
        }

        public List<FileType> GetAll()
        {
            string procName = "dbo.FileTypes_SelectAll";
            List<FileType> list = null;
            FileType fileType = null;

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set)
             {
                 fileType = MapFileType(reader);
                if(list == null)
                {
                    list = new List<FileType>();
                }
                list.Add(fileType);
             });
            return list;
        }
        private FileType MapFileType(IDataReader reader)
        {
            FileType fileType = new FileType();
            int index = 0;

            fileType.Id = reader.GetSafeInt32(index++);
            fileType.Name = reader.GetString(index++);

            return fileType;
        }
    }
}
