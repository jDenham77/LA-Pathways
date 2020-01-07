using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Users;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Sabio.Models.Requests;
using System;
using System.Data;

namespace Sabio.Services
{
    public class UserService : IUserService
    {
        private IAuthenticationService<int> _authenticationService;
        private IDataProvider _dataProvider = null;

        public UserService(IAuthenticationService<int> authSerice, IDataProvider dataProvider)
        {
            _authenticationService = authSerice;
            _dataProvider = dataProvider;
        }
        public async Task<bool> LogInAsync(string email, string password)
        {
            bool isSuccessful = false;
            IUserAuthData response = Get(email, password);
            if (response != null)
            {
                await _authenticationService.LogInAsync(response);
                isSuccessful = true;
            }
            return isSuccessful;
        }
        public async Task<bool> LogInTest(string email, string password, int id, string[] roles = null)
        {
            bool isSuccessful = false;
            var testRoles = new[] { "User", "Super", "Content Manager" };
            var allRoles = roles == null ? testRoles : testRoles.Concat(roles);
            IUserAuthData response = new UserBase
            {
                Id = id
                ,
                Name = email
                ,
                Roles = allRoles
                ,
                TenantId = "Acme Corp UId"
            };
            Claim fullName = new Claim("CustomClaim", "Sabio Bootcamp");
            await _authenticationService.LogInAsync(response, new Claim[] { fullName });
            return isSuccessful;
        }

        public void ConfirmAcct(Guid token)
        {
            string procName = "[dbo].[Users_ComfirmedUpdater]";
           
            _dataProvider.ExecuteNonQuery(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Guid", token);
            }
           );
           return;
        }        

        public int Create(UserAddRequest model)
        {
            //make sure the password column can hold long enough string. put it to 100 to be safe
            string procName = "[dbo].[Users_Insert]";
            int id = 0;

            _dataProvider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                
                col.AddWithValue("@Password", getHash(model.Password));
                
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

            //DB provider call to create user and get us a user id

            //be sure to store both salt and passwordHash
            //DO NOT STORE the original password value that the user passed us

        /// <summary>
        /// Gets the Data call to get a give user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        private IUserAuthData Get(string email, string password)
        {
            //make sure the password column can hold long enough string. put it to 100 to be safe
            string passwordFromDb = "";
            UserBase user = null;
            //get user object from db;
            bool isValidCredentials = BCrypt.BCryptHelper.CheckPassword(password, passwordFromDb);

            return user;
        }

        public async Task<UserAuth>  Authenticate(string email, string password)
        {
            UserAuth user= null;
            bool isSuccessful;
            

            UserAuth tempUser  = GetAuth(email);

            if (tempUser != null)
            {
                bool isValidCredentials = BCrypt.BCryptHelper.CheckPassword(password, tempUser.Password);

                if (isValidCredentials)
                {
                    isSuccessful = await LogInUser(tempUser.Email, tempUser.Password, tempUser.Id, tempUser.Role);
                    if (isSuccessful==true)
                    {
                        user = tempUser;
                    }                
                                    
                    
                }

            }

            return  user;
        }

        public UserAuth GetAuth(string email)
        {
            string procName = "dbo.Users_Select_AuthData";
            UserAuth user = null;
            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
             {
                 paramCollection.AddWithValue("@Email", email);

             }, delegate (IDataReader reader, short set)
             {
                 user = MapUser(reader);
 
             }
            );
            return user;

        }
       

        public async Task<bool> LogInUser(string email, string password, int id, string role)
        {
            bool isSuccessful = false;
            
            IUserAuthLoginData response = new UserBaseLogin
            {
                Id = id
                ,
                Name = email
                ,
                Role = role
                ,
                TenantId = "LA Pathways"
            };
            isSuccessful = true;
            Claim fullName = new Claim("CustomClaim", "Sabio Bootcamp");
            await _authenticationService.LogInAsyncData(response, new Claim[] { fullName });
            
            return isSuccessful;
        }
        public async Task<bool> LogOutUser()
        {
            await _authenticationService.LogOutAsync();
            bool isSuccessful = true;
            return isSuccessful;
        }

        private static UserAuth MapUser(IDataReader reader)
        {
            UserAuth user = new UserAuth();
            int startingIdex = 0;
            user.Id = reader.GetSafeInt32(startingIdex++);
            user.Email = reader.GetSafeString(startingIdex++);
            user.Password = reader.GetSafeString(startingIdex++);
            user.Role = reader.GetSafeString(startingIdex++);
            return user;
        }

        private static void AddCommonParams(UserAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Email", model.Email);
            col.AddWithValue("@RoleId", model.RoleId);
            col.AddWithValue("@IsConfirmed", model.IsConfirmed);
        }

        private string getHash(string password)
        {
            string salt = BCrypt.BCryptHelper.GenerateSalt();
            string hashedPassword = BCrypt.BCryptHelper.HashPassword(password, salt);

            return hashedPassword;
        }

        public int ForgotPassword(string email)
        {
            string procName = "[dbo].[Users_SelectByEmail]";
            int id = 0;


            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
              
                col.AddWithValue("@Email", email);
               
            }, delegate (IDataReader reader, short set)
            {
                id = reader.GetSafeInt32(0);
            }
            );

            return id;
        }
        
        public int ResetPassword(UpdatePassword model)
        {

            string procName = "[dbo].[Users_ResetPassword]";
            int id = 0;

            _dataProvider.ExecuteNonQuery(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Guid", model.Token);
                paramCollection.AddWithValue("@Password", getHash(model.Password));
            }
            );
            return id;
        }
    }
}