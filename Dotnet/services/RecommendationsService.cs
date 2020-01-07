using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Recommendations;
using Sabio.Services.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sabio.Services
{
    public class RecommendationsService : IRecommendationsService
    {
        IDataProvider _data = null;
        public RecommendationsService(IDataProvider data)
        {
            _data = data;
        }
        public void Insert(RecommendationAddRequest model)
        {

            DataTable dt = null;
            if (model.ResourceId.Count > 0)
            {
                dt = new DataTable();
                dt.Columns.Add("ResourceId", typeof(int));
                foreach (int item in model.ResourceId)
                {
                    dt.Rows.Add(item);
                }
            }
         
            string procName = "[dbo].[ResourceCondition_MultiInsert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@ResourceIds", dt);
                    col.AddWithValue("@Condition", model.Condition);
                    col.AddWithValue("@Query", model.Query);

                });
        }
        public void Update(RecommendationUpdateRequest model)
        {
            string procName = "ResourceConditions_Update";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col) 
            {
                col.AddWithValue("@ResourceId", model.ResourceId);
                col.AddWithValue("@Condition", model.Condition);
                col.AddWithValue("@Query", model.Query);
            });

        }
        public Condition GetById(int id)
        {
            string procName = "[dbo].[ResourceConditions_SelectById]";
            Condition resource = null;
            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                resource = new Condition();
                resource.ResourceId = reader.GetSafeInt32(index++);
                resource.ConditionString = reader.GetSafeString(index++);
                resource.Query = reader.GetSafeString(index++);
            });
            return resource;
        }

    }
}
