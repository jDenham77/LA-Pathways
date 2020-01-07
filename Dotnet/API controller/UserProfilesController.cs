using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/userprofiles")]
    [ApiController]
    public class UserProfilesController : BaseApiController
    {
        private IUserProfileService _service = null;
        private IAuthenticationService<int> _authService = null;

        public UserProfilesController(IUserProfileService service,
            ILogger<UserProfilesController> logger,
            IAuthenticationService<int> authenticationService) : base(logger)
        {
            _service = service;
            _authService = authenticationService;
        }

        [HttpGet]
        public ActionResult <ItemResponse<Paged<UserProfile>>> GetPaginated(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            Paged<UserProfile> page = null;

            try
            {
                page = _service.GetPaginated(pageIndex, pageSize);

                if(page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found");
                }
                else
                {
                    response = new ItemResponse<Paged<UserProfile>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }
        [HttpGet("{id:int}")]
        public ActionResult <ItemResponse<UserProfile>> GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;
            UserProfile result = null;
            try
            {
                result = _service.GetById(id);

                if(result == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");
                }
                else
                {
                    response = new ItemResponse<UserProfile> { Item = result };
                }
            }
            catch (SqlException sqlEx)
            {
                code = 500;
                response = new ErrorResponse($"SqlException Error: ${sqlEx.Message}");
            }
            catch (ArgumentException argEx)
            {
                code = 500;
                response = new ErrorResponse($"ArgumentException Error: ${argEx.Message}");
            }
            catch (Exception ex)
            {
                code = 500;

                base.Logger.LogError(ex.ToString());

                response = new ErrorResponse($"Generic Error: ${ex.Message}");

            }
            return StatusCode(code, response);


        }
       
        [HttpPost]
        public ActionResult<ItemResponse<int>> Add(AddUserProfileRequest model)
        {
            ObjectResult result = null;
            try
            {
                int currentUserId = _authService.GetCurrentUserId();

                int id = _service.Add(model, currentUserId);

                ItemResponse<int> response = new ItemResponse<int> { Item = id };

                result = Created201(response);
            }
            catch(Exception ex)
            {
                base.Logger.LogError(ex.ToString());

                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }
        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(UpdateUserProfileRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int currentUserId = _authService.GetCurrentUserId();

                _service.Update(model, currentUserId);

                response = new SuccessResponse();
            }
            catch(Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpGet("verify/{userId:int}")]
        public ActionResult<ItemResponse<UserProfile>> GetByUserId(int userId)
        {
            int code = 200;
            BaseResponse response = null;
            UserProfile result = null;
            try
            {
                result = _service.GetByUserId(userId);

                if (result == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found for" + userId);
                }
                else
                {
                    response = new ItemResponse<UserProfile> { Item = result };
                }
            }
            catch (SqlException sqlEx)
            {
                code = 500;
                response = new ErrorResponse($"SqlException Error: ${sqlEx.Message}");
            }
            catch (ArgumentException argEx)
            {
                code = 500;
                response = new ErrorResponse($"ArgumentException Error: ${argEx.Message}");
            }
            catch (Exception ex)
            {
                code = 500;

                base.Logger.LogError(ex.ToString());

                response = new ErrorResponse($"Generic Error: ${ex.Message}");

            }
            return StatusCode(code, response);


        }
    }
}