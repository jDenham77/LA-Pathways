using Sabio.Data.Providers;
using Sabio.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Sabio.Data;
using Sabio.Models;
using Newtonsoft.Json;
using Sabio.Models.Requests.Locations;
using Sabio.Services.Interfaces.Security;

namespace Sabio.Services
{
    public class LocationsService : ILocationsService
    {
        IDataProvider _data = null;

        public LocationsService(IDataProvider data)
        {
            _data = data;
        }
        public List<LocationOption> GetOptions()
        {
            List<LocationOption> result = null;
            _data.ExecuteCmd("[dbo].[Locations_GetOptions]",
                inputParamMapper: null,
                 singleRecordMapper: delegate (IDataReader reader, short set)
                 {
                     LocationOption model = MapLocationOption(reader);
                     if (result == null)
                     {
                         result = new List<LocationOption>();
                     }
                     result.Add(model);
                 }
            );
            return result;
        }
        public Paged<Location> GetPage(int pageIndex, int pageSize)
        {
            Paged<Location> pagedResult = null;

            List<Location> result = null;

            int totalCount = 0;

            _data.ExecuteCmd("[dbo].[Locations_SelectAll_V2]",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@pageIndex", pageIndex);
                    parameterCollection.AddWithValue("@pageSize", pageSize);

                },
                 singleRecordMapper: delegate (IDataReader reader, short set)
                 {

                     Location model = MapLocations(reader);

                     if (totalCount == 0)
                     {
                         totalCount = reader.GetSafeInt32(17);
                     }

                     if (result == null)
                     {
                         result = new List<Location>();
                     }

                     result.Add(model);
                 }

            );
            if (result != null)
            {
                pagedResult = new Paged<Location>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }
        public Location Get(int Id)
        {
            string procName = "[dbo].[Locations_SelectById]";

            Location locations = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", Id);
            }, delegate (IDataReader reader, short set)
            {
                locations = MapLocations(reader);
            }
            );
            return locations;
        }
        public void Update(LocationsUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Locations_Update]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@Id", model.Id);
                    col.AddWithValue("@CreatedBy", userId);
                    col.AddWithValue("@ModifiedBy", userId);
                },
                returnParameters: null);
        }

        public int Insert(LocationAddRequest model, int userId)
        {
            int id = 0;
            string procName = "[dbo].[Locations_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;
                    col.Add(idOut);
                    AddCommonParams(model, col);
                    col.AddWithValue("@CreatedBy", userId);
                    col.AddWithValue("@ModifiedBy", userId);
                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;
                    int.TryParse(oId.ToString(), out id);
                });
            return id;
        }

        private static void AddCommonParams(LocationAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@LocationTypeId", model.LocationTypeId);
            col.AddWithValue("@LineOne", model.LineOne);
            col.AddWithValue("@LineTwo", model.LineTwo);
            col.AddWithValue("@City", model.City);
            col.AddWithValue("@Zip", model.Zip);
            col.AddWithValue("@StateId", model.StateId);
            col.AddWithValue("@Latitude", model.Latitude);
            col.AddWithValue("@Longitude", model.Longitude);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Locations_Delete]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                },
                returnParameters: null);
        }
        private static Location MapLocations(IDataReader reader)
        {
            Location locations = new Location();

            int startingIndex = 0;

            locations.Id = reader.GetSafeInt32(startingIndex++);
            locations.LocationTypeId = reader.GetSafeInt32(startingIndex++);
            locations.LineOne = reader.GetSafeString(startingIndex++);
            locations.LineTwo = reader.GetSafeString(startingIndex++);
            locations.City = reader.GetSafeString(startingIndex++);
            locations.Zip = reader.GetSafeString(startingIndex++);
            locations.StateId = reader.GetSafeInt32(startingIndex++);
            locations.Latitude = reader.GetSafeDouble(startingIndex++);
            locations.Longitude = reader.GetSafeDouble(startingIndex++);
            locations.DateCreated = reader.GetSafeDateTime(startingIndex++);
            locations.DateModified = reader.GetSafeDateTime(startingIndex++);
            locations.CreatedBy = reader.GetSafeInt32(startingIndex++);
            locations.Username = new Username();

            locations.Username.FirstName = reader.GetSafeString(startingIndex++);
            locations.Username.LastName = reader.GetSafeString(startingIndex++);
            locations.ModifiedBy = reader.GetSafeInt32(startingIndex++);
            locations.Name = reader.GetSafeString(startingIndex++);
            locations.StateName = reader.GetSafeString(startingIndex++);

            return locations;
        }
        private static LocationOption MapLocationOption(IDataReader reader)
        {
            LocationOption locations = new LocationOption();
            int startingIndex = 0;
            locations.Id = reader.GetSafeInt32(startingIndex++);
            locations.Name = reader.GetSafeString(startingIndex++);
            return locations;
        }
        public Paged<Location> GetPage(int pageIndex, int pageSize, string query)
        {
            Paged<Location> pagedResult = null;

            List<Location> result = null;

            int totalCount = 0;

            _data.ExecuteCmd("[dbo].[Locations_Search]",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                    parameterCollection.AddWithValue("@Query", query);
                },
                 singleRecordMapper: delegate (IDataReader reader, short set)
                 {

                     Location model = new Location();
                     int index = 0;
                     model.Id = reader.GetSafeInt32(index++);
                     model.LocationTypeId = reader.GetSafeInt32(index++);
                     model.LineOne = reader.GetSafeString(index++);
                     model.LineTwo = reader.GetSafeString(index++);
                     model.City = reader.GetSafeString(index++);
                     model.Zip = reader.GetSafeString(index++);
                     model.StateId = reader.GetSafeInt32(index++);
                     totalCount = reader.GetSafeInt32(index++);



                     if (result == null)
                     {
                         result = new List<Location>();
                     }

                     result.Add(model);
                 }

            );
            if (result != null)
            {
                pagedResult = new Paged<Location>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

    }
}
