using Sabio.Data.Providers;
using Sabio.Models.Domain;
using System.Data;
using System.Data.SqlClient;
using Sabio.Data;
using Sabio.Services.Interfaces.Security;
using System.Collections.Generic;

namespace Sabio.Services
{
    public class StatesService : IStatesService
    {
        IDataProvider _data = null;

        public StatesService(IDataProvider data)
        {
            _data = data;
        }
        public List<State> Get(int CountryId)
        {
            string procName = "[dbo].[States_SelectByUSA]";

            List<State> list = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@CountryId", CountryId);
            }, delegate (IDataReader reader, short set)
            {
                State states = MapLocations(reader);

                if (list == null)
                {
                    list = new List<State>();
                }

                list.Add(states);
            }
            );
            return list;
        }
        private static State MapLocations(IDataReader reader)
        {
           State states = new State();

            int startingIndex = 0;
            states.Id = reader.GetSafeInt32(startingIndex++);
            states.CountryId = reader.GetSafeInt32(startingIndex++);
            states.StateProvinceCode = reader.GetSafeString(startingIndex++);
            states.Name = reader.GetSafeString(startingIndex++);

            return states;
        }
    }
}


