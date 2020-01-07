using Sabio.Data.Providers;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sabio.Services
{
    public class UserTokensService : IUserTokensService
    {
        IDataProvider _data = null;

        public UserTokensService(IDataProvider data)
        {
            _data = data;
        }
        public void Add(int userId, Guid token)
        { 

            string procName = "[dbo].[UserTokens_ConfirmEmailToken]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    int tokenType = 1; //hard coded

                    col.AddWithValue("@UserId", userId); // change this 
                    col.AddWithValue("@Token", token);
                    col.AddWithValue("@TokenType", tokenType); //would be for new user

                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@UserId"].Value;
                    int.TryParse(oId.ToString(), out userId);
                    Console.WriteLine("");
                });
        }
    }
}
