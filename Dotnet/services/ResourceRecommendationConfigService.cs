using Newtonsoft.Json;
using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sabio.Services
{
    public class ResourceRecommendationConfigService : IResourceRecommendationConfigService
    {
        IDataProvider _data = null;
        public ResourceRecommendationConfigService(IDataProvider data)
        {
            _data = data;
        }

        public Paged<ResourceRecommendation> GetByInstanceId(int id, int pageIndex, int pageSize)
        {
            string procName = "[dbo].[Resource_Recommendation_Config_Select_ByInstanceId_V3]";
            ResourceRecommendation resource = null;
            List<ResourceRecommendation> resourceList = null;
            Paged  < ResourceRecommendation > pagedResult = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                resource = MapResource(reader);
                totalCount = reader.GetSafeInt32(13);

                if(resourceList == null)
                {
                    resourceList = new List<ResourceRecommendation>();
                }

                resourceList.Add(resource);

            });
                if(resourceList != null)
                {
                    pagedResult = new Paged<ResourceRecommendation>(resourceList, pageIndex, pageSize, totalCount);
                }

            return pagedResult;
        }
        public List<ResourceRecommendation> GetAllByInstanceId(int id)
        {
            string procName = "[dbo].[Resource_Recommendation_Config_SelectAll_ByInstanceId_V3]";
            ResourceRecommendation resource = null;
            List<ResourceRecommendation> resourceList = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                resource = MapResource(reader);

                if (resourceList == null)
                {
                    resourceList = new List<ResourceRecommendation>();
                }

                resourceList.Add(resource);

            });

            return resourceList;
        }
        public ResourceRecommendation GetByResourceId(int id)
        {
            string procName = "[dbo].[Resources_Select_Details_ById_V2]";
            ResourceRecommendation resource = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
             {
                 col.AddWithValue("@Id", id);
 
             }, singleRecordMapper: delegate (IDataReader reader, short set)
             {
                 resource = MapResource(reader);
             });

            return resource;

        }

        private static ResourceRecommendation MapResource(IDataReader reader)
        {
            int index = 0;
            ResourceRecommendation resource = new ResourceRecommendation();

            resource.Id = reader.GetSafeInt32(index++);
            resource.Name = reader.GetSafeString(index++);
            resource.Headline = reader.GetSafeString(index++);
            resource.Description = reader.GetSafeString(index++);
            resource.Logo = reader.GetSafeString(index++);
            resource.LocationId = reader.GetSafeInt32(index++);
            resource.ContactName = reader.GetSafeString(index++);
            resource.ContactEmail = reader.GetSafeString(index++);
            resource.Phone = reader.GetSafeString(index++);
            resource.SiteUrl = reader.GetSafeString(index++);
            resource.DateCreated = reader.GetSafeDateTime(index++);
            resource.DateModified = reader.GetSafeDateTime(index++);
            string resourceCategoryTypes = reader.GetSafeString(index++);
            resource.ResourceCategories = JsonConvert.DeserializeObject<List<ResourceCategoryType>>(resourceCategoryTypes);

            return resource;
        }


    }
}
