using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Files;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sabio.Services
{
    public class FilesService : IFilesService
    {
        IDataProvider _data = null;
        public FilesService(IDataProvider data)
        {
            _data = data;
        }
        public int Add(AddRequest model, int userId)
        {
            int id = 0;
            string procName = "[dbo].[Files_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@CreatedBy", userId);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);

                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;
                    int.TryParse(oId.ToString(), out id);
                    
                });
            return id;
        }



        public void Update(FileUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Files_Update]";
            _data.ExecuteNonQuery(procName,
            inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@CreatedBy", userId);

                col.AddWithValue("@Id", model.Id);

            },
            returnParameters: null);
        }


        public File Get(int id)
        {

            string procName = "[dbo].[Files_Select_ById]";

            File file = null;


            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)
            {
                file = MapFile(reader);
            }

            );
            return file;
        }


        public void Delete(int id)
        {
            string procName = "[dbo].[Files_Delete_ById]";
            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            });
        }



        public Paged<File> Pagination(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[Files_SelectAll]";
            Paged<File> pagedList = null;
            List<File> list = null;
            int totalCount = 0;
   
            _data.ExecuteCmd(
                procName, (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                },
                  delegate (IDataReader reader, short set)

                  {
                      File file = MapFile(reader);
                      if (totalCount == 0)
                      { totalCount = reader.GetSafeInt32(5); }

                      if (list == null)
                      {
                          list = new List<File>();
                      }
                      list.Add(file);
                  }
                );
            if (list != null)
            {
                pagedList = new Paged<File>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<File> PaginationCreatedBy(int userId, int pageIndex, int pageSize)
        {
            string procName = "[dbo].[Files_Select_ByCreatedBy]";
            Paged<File> pagedList = null;
            List<File> list = null;
            int totalCount = 0;
  
            _data.ExecuteCmd(
                procName, (param) =>
                {
                    param.AddWithValue("@CreatedBy", userId);
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                },
                  delegate (IDataReader reader, short set)

                  {
                      File file = MapFile(reader);
                      if (totalCount == 0)
                      { totalCount = reader.GetSafeInt32(5); }

                      if (list == null)
                      {
                          list = new List<File>();
                      }
                      list.Add(file);
                  }
                );
            if (list != null)
            {
                pagedList = new Paged<File>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }



        private static File MapFile(IDataReader reader)
        {
            File aFile = new File();
            int startingIdex = 0;

            aFile.Id = reader.GetSafeInt32(startingIdex++);
            aFile.Url = reader.GetSafeString(startingIdex++);
            aFile.FileTypeId = reader.GetSafeInt32(startingIdex++);
            aFile.CreatedBy = reader.GetSafeInt32(startingIdex++);
            aFile.DateCreated = reader.GetSafeDateTime(startingIdex++);            

            return aFile;
        }



        private static void AddCommonParams(AddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Url", model.Url);
            col.AddWithValue("@FileTypeId", model.FileTypeId);
            
           
        }
    }
}
