using Sabio.Models.Domain;
using Sabio.Models.Requests.FAQs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Sabio.Data.Providers;
using Sabio.Data;
using Sabio.Services.Interfaces;
using Sabio.Models;

namespace Sabio.Services
{
    public class FAQsService : IFAQsService
    {
        IDataProvider _data = null;
        public FAQsService(IDataProvider data)
        {
            _data = data;
        }

        public FAQ Get(int id)
        {
            string procName = "[dbo].[FAQ_SelectById]";


            FAQ faq = null;


            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)
            {

                faq = MapFAQ(reader);

            }

            );
            return faq;
        }

        public void Update(FAQUpdateRequest model, int userId)
        {
            string procName = "[dbo].[FAQ_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@ModifiedBy", userId);

            },
            returnParameters: null);

        }

        public int Add(FAQAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[FAQ_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@CreatedBy", userId);
                col.AddWithValue("@ModifiedBy", userId);

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

        public Paged<FAQ> Paginate(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[FAQ_SelectAll]";
            Paged<FAQ> pagedList = null;
            List<FAQ> list = null;
            int totalCount = 0;
            _data.ExecuteCmd(
                procName, (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    FAQ instance = MapFAQ(reader);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(9);
                    }
                    
                    if (list == null)
                    {
                        list = new List<FAQ>();
                    }
                    list.Add(instance);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<FAQ>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[FAQ_DeleteById]";
            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            });
        }

        private static FAQ  MapFAQ(IDataReader reader)
        {
            FAQ aFAQ = new FAQ();
            int startingIndex = 0;

            aFAQ.Id = reader.GetSafeInt32(startingIndex++);
            aFAQ.Question = reader.GetSafeString(startingIndex++);
            aFAQ.Answer = reader.GetSafeString(startingIndex++);
            aFAQ.CategoryId = reader.GetSafeInt32(startingIndex++);
            aFAQ.SortOrder = reader.GetSafeInt32(startingIndex++);
            aFAQ.CreatedBy = reader.GetSafeInt32(startingIndex++);
            aFAQ.ModifiedBy = reader.GetSafeInt32(startingIndex++);
            aFAQ.DateCreated = reader.GetDateTime(startingIndex++);
            aFAQ.DateModified = reader.GetDateTime(startingIndex++);

            return aFAQ;
        }

        private static void AddCommonParams(FAQAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Question", model.Question);
            col.AddWithValue("@Answer", model.Answer);
            col.AddWithValue("@CategoryId", model.CategoryId);
            col.AddWithValue("@SortOrder", model.SortOrder);

        }


    }
}
