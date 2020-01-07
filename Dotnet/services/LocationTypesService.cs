using Sabio.Data.Providers;
using Sabio.Models.Domain;
using System.Data;
using System.Data.SqlClient;
using Sabio.Data;
using Sabio.Services.Interfaces.Security;
using System.Collections.Generic;
using Sabio.Models;

namespace Sabio.Services
{
    public class LocationTypesService : ILocationTypesService
    {
        IDataProvider _data = null;

        public LocationTypesService(IDataProvider data)
        {
            _data = data;
        }
        public List<LocationType> Get()
        {
            string procName = "[dbo].[LocationTypes_SelectAll]";

            List<LocationType> list = null;

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set)

            {
                LocationType locationTypes = MapLocationTypes(reader);

                if (list == null)
                {
                    list = new List<LocationType>();
                }

                list.Add(locationTypes);
            }
            );
            return list;
        }
        private static LocationType MapLocationTypes(IDataReader reader)
        {
            LocationType locationTypes = new LocationType();

            int startingIndex = 0;
            locationTypes.Id = reader.GetSafeInt32(startingIndex++);
            locationTypes.Name = reader.GetSafeString(startingIndex++);

            return locationTypes;
        }
        
    }
}
