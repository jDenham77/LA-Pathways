using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.ResourceCategory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sabio.Services
{
    public class ResourceCategoryService : IResourceCategoryService
    {
        IDataProvider _data = null;

        public ResourceCategoryService(IDataProvider data)
        {
            _data = data;
        }
        public bool Add(ResourceCategoryAddRequest model, int userId)
        {
            bool hasInserted = false;
            string procName = "[dbo].[ResourceCategories_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);

            }, returnParameters: null
            );
            hasInserted = true;
            return hasInserted;
        }
        public void Update(ResourceCategoryUpdateRequest model)
        {
            string procName = "[dbo].[ResourceCategories_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
            },
            returnParameters: null);
        }
        public List<ResourceCategory> GetAll()
        {
            List<ResourceCategory> list = null;
            string procName = "[dbo].[ResourceCategories_SelectAll]";

            _data.ExecuteCmd(procName, inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    ResourceCategory aResourceCategory = MapResourceCategory(reader);

                    if (list == null)
                    {
                        list = new List<ResourceCategory>();
                    }

                    list.Add(aResourceCategory);
                }
                );
            return list;
        }
       

        public Paged<ResourceCategory> GetByPage(int PageIndex, int PageSize)
        {
            Paged<ResourceCategory> pagedResult = null;
            List<ResourceCategory> result = null;
            int totalCount = 0;
            _data.ExecuteCmd("[dbo].[ResourceCategories_SelectAll]",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@pageIndex", PageIndex);
                    parameterCollection.AddWithValue("@pageSize", PageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    ResourceCategory model = MapResourceCategory(reader);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(5);
                    }
                    if (result == null)
                    {
                        result = new List<ResourceCategory>();
                    }
                    result.Add(model);
                }
             );
            if (result != null)
            {
                pagedResult = new Paged<ResourceCategory>(result, PageIndex, PageSize, totalCount);
            }
            return pagedResult;
        }
        public ResourceCategory GetByLocationType(int id)
        {
            string procName = "[dbo].[ResourceCategories_Select_LocationType]";

            ResourceCategory resourceCategory = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@locationZoneTypeId", id);
            },
            delegate (IDataReader reader, short set)
            {
                resourceCategory = MapResourceCategory(reader);
            }
            );

            return resourceCategory;
        }

        public ResourceCategory GetByResourceId(int id)
        {
            string procName = "[dbo].[ResourceCategories_Select_ByResourceId]";

            ResourceCategory resourceCategory = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@ResourceId", id);
            },
            delegate (IDataReader reader, short set)
            {
                resourceCategory = MapResourceCategory(reader);
            }
            );

            return resourceCategory;
        }
        public ResourceCategory GetByContractingType(int id)
        {
            string procName = "[dbo].[ResourceCategories_Select_ByContractingType]";

            ResourceCategory resourceCategory = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@contractingTypeId", id);
            },
            delegate (IDataReader reader, short set)
            {
                resourceCategory = MapResourceCategory(reader);
            }
            );

            return resourceCategory;
        }
        public ResourceCategory GetByConsultingType(int id)
        {
            string procName = "[dbo].[ResourceCategories_Select_ByConsultingType]";

            ResourceCategory resourceCategory = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@consultingTypeId", id);
            },
            delegate (IDataReader reader, short set)
            {
                resourceCategory = MapResourceCategory(reader);
            }
            );

            return resourceCategory;
        }
        public void Delete(int id)
        {
            string procName = "[dbo].[ResourceCategories_Delete_ByResourceId]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }, returnParameters: null);
        }

        private ResourceCategory MapResourceCategory(IDataReader reader)
        {
            ResourceCategory aResourceCategory = new ResourceCategory();

            int startingIndex = 0;

            aResourceCategory.ResourceId = reader.GetSafeInt32(startingIndex++);
            aResourceCategory.ConsultingTypeId = reader.GetSafeInt32(startingIndex++);
            aResourceCategory.ContractingTypeId = reader.GetSafeInt32(startingIndex++);
            aResourceCategory.LocationZoneTypeId = reader.GetSafeInt32(startingIndex++);
            aResourceCategory.IsNetworking = reader.GetSafeBool(startingIndex++);
            return aResourceCategory;
        }

        private static void AddCommonParams(ResourceCategoryAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@ResourceId", model.ResourceId);
            col.AddWithValue("@ConsultingTypeId", model.ConsultingTypeId);
            col.AddWithValue("@ContractingTypeId", model.ContractingTypeId);
            col.AddWithValue("@LocationZoneTypeId", model.LocationZoneTypeId);
            col.AddWithValue("@IsNetworking", model.IsNetworking);
        }
    }
}
