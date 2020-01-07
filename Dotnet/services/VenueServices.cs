using Newtonsoft.Json;
using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Enums;
using Sabio.Models.Requests;
using Sabio.Models.Requests.Venues;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Sabio.Services.Services
{
    public class VenueServices : IVenueServices
    {
        IDataProvider _data = null;
        public VenueServices(IDataProvider data)
        {
            _data = data;
        }

        public int Add(AddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[VenueImages_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;
                    col.Add(idOut);

                    AddWithCommonParams(model, col);
                    col.AddWithValue("@CreatedBy", userId);
                    col.AddWithValue("@FileTypeId", getFileTypeIdFromFileName(model.FileUrl));
                    col.AddWithValue("@CreatedByFiles", userId);

                },
                   returnParameters: delegate (SqlParameterCollection returnCollection)
                   {

                       object oId = returnCollection["@Id"].Value;

                       Int32.TryParse(oId.ToString(), out id);

                   });


            return id;
        }

        public void Update(UpdateRequest model, int userId)
        {
            string procName = "dbo.VenueImages_Update";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddWithCommonParams(model, col);
                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@ModifiedBy", userId);
                col.AddWithValue("@FileId", model.FileId);



            });
        }

        public Paged<Venue> GetPaginatedVenues(int pageIndex, int pageSize)
        {
            Paged<Venue> pagedResult = null;
            List<Venue> result = null;
            int totalCount = 0;


            string procName = "[dbo].[Venues_SelectAllV2]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@pageIndex", pageIndex);
                col.AddWithValue("@pageSize", pageSize);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                Venue venue = MapVenue(reader);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(10);
                }
                if (result == null)
                {
                    result = new List<Venue>();
                }
                result.Add(venue);
            });

            if (result != null)
            {
                pagedResult = new Paged<Venue>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        public Venue GetById(int Id)
        {
            Venue venue = null;

            string procName = "[dbo].[Venues_Select_ById]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", Id);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
              {
                  venue = MapVenue(reader);
              });
            return venue;
        }

        public void Delete(int Id)
        {
            string procName = "[dbo].[Venues_Delete_ById]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
              {
                  col.AddWithValue("@Id", Id);

              }, returnParameters: null);
        }

        private static Venue MapVenue(IDataReader reader)
        {
            Venue venue = new Venue();

            int startingIndex = 0;

            venue.Id = reader.GetSafeInt32(startingIndex++);
            venue.Name = reader.GetSafeString(startingIndex++);
            venue.Description = reader.GetSafeString(startingIndex++);
            venue.LocationId = reader.GetSafeInt32(startingIndex++);
            venue.Url = reader.GetSafeString(startingIndex++);
            venue.CreatedBy = reader.GetSafeInt32(startingIndex++);
            venue.ModifiedBy = reader.GetSafeInt32(startingIndex++);
            venue.DateCreated = reader.GetSafeDateTime(startingIndex++);
            venue.DateModified = reader.GetSafeDateTime(startingIndex++);

            venue.VenueImage = new Image();

            string image = reader.GetSafeString(startingIndex++);
            venue.VenueImage = JsonConvert.DeserializeObject<Image>(image);


            return venue;
        }

        private static void AddWithCommonParams(AddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@LocationId", model.LocationId);
            col.AddWithValue("@Url", model.Url);
            col.AddWithValue("@FileUrl", model.FileUrl);



        }
        private int getFileTypeIdFromFileName(string str)
        {
            string result = null;
            int fileTypeId = 0;


            result = str.Split('.').Last();

            FileTypes fileTypes = new FileTypes();


            Enum.TryParse(result, true, out fileTypes);


            switch (fileTypes)
            {
                case FileTypes.PDF:
                    fileTypeId = 1;
                    break;
                case FileTypes.Text:
                    fileTypeId = 2;
                    break;
                case FileTypes.JPEG:
                    fileTypeId = 3;
                    break;
                case FileTypes.PNG:
                    fileTypeId = 4;
                    break;
                case FileTypes.Word:
                    fileTypeId = 5;
                    break;
                case FileTypes.Excel:
                    fileTypeId = 6;
                    break;
                case FileTypes.Powerpoint:
                    fileTypeId = 7;
                    break;
                case FileTypes.JPG:

                default:
                    fileTypeId = 8;
                    break;
            }

            return fileTypeId;

        }

    }
}
