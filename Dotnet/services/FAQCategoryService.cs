using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain;
using Sabio.Models.Requests.FAQCategory;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sabio.Services
{
    public class FAQCategoryService : IFAQCategoryService
    {
        IDataProvider _data = null;
        public FAQCategoryService(IDataProvider data)
        {
            _data = data;
        }

        public int Add(FAQCategoryAddRequest model)
        {
            int id = 0;

            string procName = "[dbo].[FAQCategories_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;
                    col.Add(idOut);

                    col.AddWithValue("@Name", model.Name);
                }, returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;

                    int.TryParse(oId.ToString(), out id);

                });
            return id;
        }

        public void Update(FAQCategoryUpdateRequest model)
        {
            string procName = "[dbo].[FAQCategories_Update]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", model.Id);
                    col.AddWithValue("@Name", model.Name);
                }, returnParameters: null);
        }

        public List<FAQCategory> Get()
        {
            List<FAQCategory> list = null;
            FAQCategory faqCategory = null;
            _data.ExecuteCmd("[dbo].[FAQCategories_SelectAll]"
            , inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                faqCategory = MapFAQCategory(reader);
                if (list == null)
                {
                    list = new List<FAQCategory>();
                }
                list.Add(faqCategory);
            }
            );
            return list;
        }
        public void Delete(int Id)
        {
            _data.ExecuteNonQuery("[dbo].[FAQCategories_Delete]"
            , inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", Id);
            }
            , returnParameters: null

            );
        }

        private static FAQCategory MapFAQCategory(IDataReader reader)
        {
            FAQCategory faqCategory = new FAQCategory();
            int index = 0;
            faqCategory.Id = reader.GetSafeInt32(index++);
            faqCategory.Name = reader.GetSafeString(index++);
            return faqCategory;
        }

    }
}
