using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sabio.Services
{
    public class UserProfileService : IUserProfileService
    {
        IDataProvider _data = null;

        public UserProfileService(IDataProvider data)
        {
            _data = data;
        }
        public Paged<UserProfile> GetPaginated(int pageIndex, int pageSize)
        {
            Paged<UserProfile> pagedResult = null;
            List<UserProfile> pagedUserProfiles = null;
            UserProfile userProfile = null;
            string procName = "[dbo].[UserProfiles_SelectAll]";
            int totalCount = 0;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@pageIndex", pageIndex);
                paramCollection.AddWithValue("@pageSize", pageSize);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                userProfile = MapUserProfile(reader);

                if (totalCount == 0)
                {
                    totalCount = reader.GetInt32(8);
                }

                if (pagedUserProfiles == null)
                {
                    pagedUserProfiles = new List<UserProfile>();
                }

                pagedUserProfiles.Add(userProfile);

            });
            if (pagedUserProfiles != null)
            {
                pagedResult = new Paged<UserProfile>(pagedUserProfiles, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }
        public UserProfile GetById(int id)
        {
            string procName = "[dbo].[UserProfiles_SelectById]";
            UserProfile userProfile = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                userProfile = MapUserProfile(reader);
            });
            return userProfile;
        }
        public int Add(AddUserProfileRequest model, int currentUserId)
        {
            string procName = "[dbo].[UserProfiles_Insert]";
            int id = 0;

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@UserId", currentUserId);
                AddCommonParams(model, col);

                SqlParameter IdOut = new SqlParameter("@Id", SqlDbType.Int);
                IdOut.Direction = ParameterDirection.Output;

                col.Add(IdOut);
            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;
                int.TryParse(oId.ToString(), out id);
            }
            );

            return id;
        }
        public void Update(UpdateUserProfileRequest model, int currentUserId)
        {
            string procName = "[dbo].[UserProfiles_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@UserId", currentUserId);
                AddCommonParams(model, col);

            }, returnParameters: null);
        }
        private static void AddCommonParams(AddUserProfileRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@FirstName", model.FirstName);
            col.AddWithValue("@LastName", model.LastName);
            col.AddWithValue("@Mi", model.Mi);
            col.AddWithValue("@AvatarUrl", model.AvatarUrl);
        }
        private UserProfile MapUserProfile(IDataReader reader)
        {
            UserProfile userProfile = new UserProfile();

            int index = 0;

            userProfile.Id = reader.GetInt32(index++);
            userProfile.UserId = reader.GetInt32(index++);
            userProfile.FirstName = reader.GetString(index++);
            userProfile.LastName = reader.GetString(index++);
            userProfile.Mi = reader.GetString(index++);
            userProfile.AvatarUrl = reader.GetString(index++);
            userProfile.DateCreated = reader.GetDateTime(index++);
            userProfile.DateModified = reader.GetDateTime(index++);

            if (userProfile.Mi == null)
            {
                userProfile.Mi = new string("");
            }

            return userProfile;
        }
        public UserProfile GetByUserId(int userId)
        {
            string procName = "[dbo].[UserProfiles_SelectByUserId_Verify]";
            UserProfile userProfile = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@UserId", userId);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                userProfile = MapUserProfile(reader);
            });
            return userProfile;
        }
    }
}
