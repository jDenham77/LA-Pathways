using Newtonsoft.Json;
using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.CategoriesTypes;
using Sabio.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using System.Text;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
namespace Sabio.Services
{
    public class ResourceService : IResourceService
    {
        IDataProvider _data = null;
        public ResourceService(IDataProvider data)
        {
            _data = data;
        }
        public int Add(ResourceAddRequest model)
        {
            int id = 0;
            string procName = "[dbo].[Resources_Insert_V3]";

            DataTable capitalTypesDT = null;
            if (model.CapitalTypes != null && model.CapitalTypes.Count > 0)
            {
                capitalTypesDT = new DataTable();
                capitalTypesDT.Columns.Add("CapitalTypeId", typeof(int));

                foreach (int item in model.CapitalTypes)
                {
                    capitalTypesDT.Rows.Add(item);
                }
            }
            DataTable complianceTypesDT = null;
            if (model.ComplianceTypes != null && model.ComplianceTypes.Count > 0)
            {
                complianceTypesDT = new DataTable();
                complianceTypesDT.Columns.Add("ComplianceTypeId", typeof(int));

                foreach (int item in model.ComplianceTypes)
                {
                    complianceTypesDT.Rows.Add(item);
                }
            }
            DataTable demographicTypesDT = null;
            if (model.DemographicTypes != null && model.DemographicTypes.Count > 0)
            {
                demographicTypesDT = new DataTable();
                demographicTypesDT.Columns.Add("DemographicTypeId", typeof(int));

                foreach (int item in model.DemographicTypes)
                {
                    demographicTypesDT.Rows.Add(item);
                }
            }
            DataTable specialTopicsTypesDT = null;
            if (model.SpecialTopicsTypes != null && model.SpecialTopicsTypes.Count > 0)
            {
                specialTopicsTypesDT = new DataTable();
                specialTopicsTypesDT.Columns.Add("SpecialTopicsTypeId", typeof(int));

                foreach (int item in model.SpecialTopicsTypes)
                {
                    specialTopicsTypesDT.Rows.Add(item);
                }
            }
            DataTable industryTypesDT = null;
            if (model.IndustryTypes != null && model.IndustryTypes.Count > 0)
            {
                industryTypesDT = new DataTable();
                industryTypesDT.Columns.Add("IndustryTypeId", typeof(int));

                foreach (int item in model.IndustryTypes)
                {
                    industryTypesDT.Rows.Add(item);
                }
            }
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@CapitalTypes", capitalTypesDT);
                    col.AddWithValue("@ComplianceTypes", complianceTypesDT);
                    col.AddWithValue("@DemographicTypes", demographicTypesDT);
                    col.AddWithValue("@SpecialTopicsTypes", specialTopicsTypesDT);
                    col.AddWithValue("@IndustryTypes", industryTypesDT);
                    col.AddWithValue("@ConsultingTypeId", model.ConsultingTypes);
                    col.AddWithValue("@ContractingTypeId", model.ContractingTypes);
                    col.AddWithValue("@LocationTypeId", model.LocationZoneTypes);

                    AddCommonParams(col, model);
                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);
                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;

                    Int32.TryParse(oId.ToString(), out id);
                });
            return id;
        }
        public void Update(ResourceUpdateRequest model)
        {
            string procName = "[dbo].[Resources_Update_V3]";

            DataTable capitalTypesDT = null;
            if (model.CapitalTypes != null && model.CapitalTypes.Count > 0)
            {
                capitalTypesDT = new DataTable();
                capitalTypesDT.Columns.Add("CapitalTypeId", typeof(int));

                foreach (int item in model.CapitalTypes)
                {
                    capitalTypesDT.Rows.Add(item);
                }
            }
            DataTable complianceTypesDT = null;
            if (model.ComplianceTypes != null && model.ComplianceTypes.Count > 0)
            {
                complianceTypesDT = new DataTable();
                complianceTypesDT.Columns.Add("ComplianceTypeId", typeof(int));

                foreach (int item in model.ComplianceTypes)
                {
                    complianceTypesDT.Rows.Add(item);
                }
            }
            DataTable demographicTypesDT = null;
            if (model.DemographicTypes != null && model.DemographicTypes.Count > 0)
            {
                demographicTypesDT = new DataTable();
                demographicTypesDT.Columns.Add("DemographicTypeId", typeof(int));

                foreach (int item in model.DemographicTypes)
                {
                    demographicTypesDT.Rows.Add(item);
                }
            }
            DataTable specialTopicsTypesDT = null;
            if (model.SpecialTopicsTypes != null && model.SpecialTopicsTypes.Count > 0)
            {
                specialTopicsTypesDT = new DataTable();
                specialTopicsTypesDT.Columns.Add("SpecialTopicsTypeId", typeof(int));

                foreach (int item in model.SpecialTopicsTypes)
                {
                    specialTopicsTypesDT.Rows.Add(item);
                }
            }
            DataTable industryTypesDT = null;
            if (model.IndustryTypes != null && model.IndustryTypes.Count > 0)
            {
                industryTypesDT = new DataTable();
                industryTypesDT.Columns.Add("IndustryTypeId", typeof(int));

                foreach (int item in model.IndustryTypes)
                {
                    industryTypesDT.Rows.Add(item);
                }
            }

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", model.Id);
                    col.AddWithValue("@CapitalTypes", capitalTypesDT);
                    col.AddWithValue("@ComplianceTypes", complianceTypesDT);
                    col.AddWithValue("@DemographicTypes", demographicTypesDT);
                    col.AddWithValue("@SpecialTopicsTypes", specialTopicsTypesDT);
                    col.AddWithValue("@IndustryTypes", industryTypesDT);
                    col.AddWithValue("@ConsultingTypeId", model.ConsultingTypes);
                    col.AddWithValue("@ContractingTypeId", model.ContractingTypes);
                    col.AddWithValue("@LocationTypeId", model.LocationZoneTypes);
                    AddCommonParams(col, model);
                },
                returnParameters: null);
        }
        public Paged<Resource> GetAll(int pageIndex, int pageSize)
        {
            Paged<Resource> pagedResult = null;
            List<Resource> result = null;
            Resource resources = null;
            int totalCount = 0;
            _data.ExecuteCmd(
                "[dbo].[Resource_Select_WithCatTypes]",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@pageIndex", pageIndex);
                    parameterCollection.AddWithValue("@pageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    resources = MapResources(reader);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(13);
                    }
                    if (result == null)
                    {
                        result = new List<Resource>();
                    }
                    result.Add(resources);
                }
            );
            if (result != null)
            {
                pagedResult = new Paged<Resource>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;
        }
        public List<Condition> GetAllRec()
        {
            List<Condition> list = null;
            string procName = "[dbo].[ResourceConditions_SelectAll]";
            
            _data.ExecuteCmd(procName, inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    Condition aCondition = MapAllConditions(reader);
                    
                    
                    if (list == null)
                    {
                        list = new List<Condition>();
                    }
                    
                    list.Add(aCondition);
                }
                );
            return list;
        }
        public Resource Get(int id)
        {
            string procName = "[dbo].[Resources_Select_ById]";

            Resource resources = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            },

            delegate (IDataReader reader, short set)
                {
                    resources = MapResources(reader);
                }
            );
            return resources;
        }
        public Paged<Resource> Search(int pageIndex, int pageSize, string q)
        {
            string procName = "[dbo].[Resource_Search_WithCatTypes]";
            Paged<Resource> pagedResult = null;
            List<Resource> indexedResult = null;
            int totalCount = 0;
            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@pageIndex", pageIndex);
                paramCollection.AddWithValue("@pageSize", pageSize);
                paramCollection.AddWithValue("@query", q);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                Resource resources = MapResources(reader);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(13);
                }
                if (indexedResult == null)
                {
                    indexedResult = new List<Resource>();
                }
                indexedResult.Add(resources);
            });
            if (indexedResult != null)
            {
                pagedResult = new Paged<Resource>(indexedResult, pageIndex, pageSize, totalCount);
            }
            return pagedResult;
        }
        public static Resource MapResources(IDataReader reader)
        {
            Resource resource = new Resource();

            int startingIndex = 0;

            resource.Id = reader.GetSafeInt32(startingIndex++);
            resource.Name = reader.GetSafeString(startingIndex++);
            resource.Headline = reader.GetSafeString(startingIndex++);
            resource.Description = reader.GetSafeString(startingIndex++);
            resource.Logo = reader.GetSafeString(startingIndex++);
            resource.LocationId = reader.GetSafeInt32(startingIndex++);
            resource.ContactName = reader.GetSafeString(startingIndex++);
            resource.ContactEmail = reader.GetSafeString(startingIndex++);
            resource.Phone = reader.GetSafeString(startingIndex++);
            resource.SiteUrl = reader.GetSafeString(startingIndex++);
            resource.DateCreated = reader.GetSafeDateTime(startingIndex++);
            resource.DateModified = reader.GetSafeDateTime(startingIndex++);
            string categoryJson = reader.GetSafeString(startingIndex++);
            if (categoryJson != null)
            {
                resource.BaseCategoryType = JsonConvert.DeserializeObject<List<BaseCategoryType>>(categoryJson);
            }
            else
            {
                resource.BaseCategoryType = new List<BaseCategoryType>();
            }

            return resource;
        }
        private static Condition MapAllConditions(IDataReader reader)
        {
            Condition resources = new Condition();

            int startingIdex = 0;

            resources.Id = reader.GetSafeInt32(startingIdex++);
            resources.Name = reader.GetSafeString(startingIdex++);
            resources.Headline = reader.GetSafeString(startingIdex++);
            resources.Description = reader.GetSafeString(startingIdex++);
            resources.Logo = reader.GetSafeString(startingIdex++);
            resources.LocationId = reader.GetSafeInt32(startingIdex++);
            resources.ContactName = reader.GetSafeString(startingIdex++);
            resources.ContactEmail = reader.GetSafeString(startingIdex++);
            resources.Phone = reader.GetSafeString(startingIdex++);
            resources.SiteUrl = reader.GetSafeString(startingIdex++);
            resources.DateCreated = reader.GetSafeDateTime(startingIdex++);
            resources.DateModified = reader.GetSafeDateTime(startingIdex++);
            resources.ConditionString = reader.GetSafeString(startingIdex++);
            resources.Query = reader.GetSafeString(startingIdex++);

            return resources;
        }
        public static void AddCommonParams(SqlParameterCollection col, ResourceAddRequest model)
        {
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Headline", model.Headline);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@Logo", model.Logo);
            col.AddWithValue("@LocationId", model.LocationId);
            col.AddWithValue("@ContactName", model.ContactName);
            col.AddWithValue("@ContactEmail", model.ContactEmail);
            col.AddWithValue("@Phone", model.Phone);
            col.AddWithValue("@SiteUrl", model.SiteUrl);
        }
        public Categories GetResourceType()
        {
            Categories categoryTypes = null;
            _data.ExecuteCmd(
               "[dbo].[Resources_SelectAll_V3]",
               inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    switch (set)
                    {
                        case 0:
                            int indexCT = 0;
                            ConsultingTypes consultingType = new ConsultingTypes();
                            consultingType.Id = reader.GetSafeInt32(indexCT++);
                            consultingType.Code = reader.GetSafeString(indexCT++);
                            consultingType.Name = reader.GetSafeString(indexCT++);
                            if (categoryTypes == null)
                            {
                                categoryTypes = new Categories();
                            }
                            if (categoryTypes.ConsultingTypes == null)
                            {
                                categoryTypes.ConsultingTypes = new List<ConsultingTypes>();
                            }
                            categoryTypes.ConsultingTypes.Add(consultingType);
                            break;
                        case 1:
                            int indexConT = 0;
                            ContractingTypes contractingType = new ContractingTypes();
                            contractingType.Id = reader.GetSafeInt32(indexConT++);
                            contractingType.Code = reader.GetSafeString(indexConT++);
                            contractingType.Name = reader.GetSafeString(indexConT++);
                            if (categoryTypes == null)
                            {
                                categoryTypes = new Categories();
                            }
                            if (categoryTypes.ContractingTypes == null)
                            {
                                categoryTypes.ContractingTypes = new List<ContractingTypes>();
                            }
                            categoryTypes.ContractingTypes.Add(contractingType);
                            break;
                        case 2:
                            int indexLZT = 0;
                            LocationTypes locationZoneType = new LocationTypes();
                            locationZoneType.Id = reader.GetSafeInt32(indexLZT++);
                            locationZoneType.Code = reader.GetSafeString(indexLZT++);
                            locationZoneType.Name = reader.GetSafeString(indexLZT++);
                            if (categoryTypes == null)
                            {
                                categoryTypes = new Categories();
                            }
                            if (categoryTypes.LocationTypes == null)
                            {
                                categoryTypes.LocationTypes = new List<LocationTypes>();
                            }
                            categoryTypes.LocationTypes.Add(locationZoneType);
                            break;
                        case 3:
                            int indexCapT = 0;
                            CapitalTypes capitalType = new CapitalTypes();
                            capitalType.Id = reader.GetSafeInt32(indexCapT++);
                            capitalType.Code = reader.GetSafeString(indexCapT++);
                            capitalType.Name = reader.GetSafeString(indexCapT++);
                            if (categoryTypes == null)
                            {
                                categoryTypes = new Categories();
                            }
                            if (categoryTypes.CapitalTypes == null)
                            {
                                categoryTypes.CapitalTypes = new List<CapitalTypes>();
                            }
                            categoryTypes.CapitalTypes.Add(capitalType);
                            break;
                        case 4:
                            int indexCompT = 0;
                            ComplianceTypes complianceType = new ComplianceTypes();
                            complianceType.Id = reader.GetSafeInt32(indexCompT++);
                            complianceType.Code = reader.GetSafeString(indexCompT++);
                            complianceType.Name = reader.GetSafeString(indexCompT++);
                            if (categoryTypes == null)
                            {
                                categoryTypes = new Categories();
                            }
                            if (categoryTypes.ComplianceTypes == null)
                            {
                                categoryTypes.ComplianceTypes = new List<ComplianceTypes>();
                            }
                            categoryTypes.ComplianceTypes.Add(complianceType);
                            break;
                        case 5:
                            int indexDT = 0;
                            DemographicTypes demographicType = new DemographicTypes();
                            demographicType.Id = reader.GetSafeInt32(indexDT++);
                            demographicType.Code = reader.GetSafeString(indexDT++);
                            demographicType.Name = reader.GetSafeString(indexDT++);
                            if (categoryTypes == null)
                            {
                                categoryTypes = new Categories();
                            }
                            if (categoryTypes.DemographicTypes == null)
                            {
                                categoryTypes.DemographicTypes = new List<DemographicTypes>();
                            }
                            categoryTypes.DemographicTypes.Add(demographicType);
                            break;
                        case 6:
                            int indexSPT = 0;
                            SpecialTopicsTypes specialTopicsType = new SpecialTopicsTypes();
                            specialTopicsType.Id = reader.GetSafeInt32(indexSPT++);
                            specialTopicsType.Code = reader.GetSafeString(indexSPT++);
                            specialTopicsType.Name = reader.GetSafeString(indexSPT++);
                            if (categoryTypes == null)
                            {
                                categoryTypes = new Categories();
                            }
                            if (categoryTypes.SpecialTopicsTypes == null)
                            {
                                categoryTypes.SpecialTopicsTypes = new List<SpecialTopicsTypes>();
                            }
                            categoryTypes.SpecialTopicsTypes.Add(specialTopicsType);
                            break;

                        case 7:
                            int indexIT = 0;
                            IndustryTypes industryType = new IndustryTypes();
                            industryType.Id = reader.GetSafeInt32(indexIT++);
                            industryType.Code = reader.GetSafeString(indexIT++);
                            industryType.Name = reader.GetSafeString(indexIT++);
                            if (categoryTypes == null)
                            {
                                categoryTypes = new Categories();
                            }
                            if (categoryTypes.IndustryTypes == null)
                            {
                                categoryTypes.IndustryTypes = new List<IndustryTypes>();
                            }
                            categoryTypes.IndustryTypes.Add(industryType);
                            break;
                    }
                }
            );
            return categoryTypes;
        }
        public void Delete(int id)
        {
            string procName = "[dbo].[Resources_Delete_ById]";

            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            });
        }



    }
}
