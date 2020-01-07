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
using Sabio.Models.Domain.Surveys;
using Sabio.Models.Request;
using Sabio.Models.Request.Surveys;
using Sabio.Models.Requests;
using Sabio.Models.Requests.Surveys;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Services.Interfaces.Surveys;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
namespace Sabio.Web.Api.Controllers
{
    [Route("api/surveys")]
    [ApiController]
    public class SurveysAPIController : BaseApiController
    {
        private ISurveyService _service = null;
        private IAuthenticationService<int> _authService = null;
        public SurveysAPIController(ISurveyService service,
            ILogger<SurveysAPIController> logger,
            IAuthenticationService<int> authenticationService) : base(logger)
        {
            _service = service;
            _authService = authenticationService;
        }
        [HttpGet]
        public ActionResult<ItemResponse<Paged<Survey>>> GetPage(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<Survey> page = _service.GetPaginated(pageIndex, pageSize);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Survey>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);

        }
        [HttpGet("status_type")]
        public ActionResult<ItemResponse<Status_Type>> GetAllTypes_Statuses()
        {
            int code = 200;
            BaseResponse response = null;
            Status_Type result = null;

            try
            {
                result = _service.GetAllTypes_Statuses();

                if (result == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found");
                }
                else
                {
                    response = new ItemResponse<Status_Type> { Item = result };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Survey>> GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Survey result = _service.GetById(id);
                if (result == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Survey> { Item = result };
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
        [HttpGet("details/{id:int}")]
        public ActionResult<ItemResponse<SurveyDetails>> GetDetailsById(int id)
        {
            int code = 200;
            BaseResponse response = null;
            SurveyDetails result = null;

            try
            {
                result = _service.GetDetailsById(id);

                if(result == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");
                }
                else
                {
                    response = new ItemResponse<SurveyDetails> { Item = result };
                }
            }
            catch(Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(AddRequest model)
        {
            ObjectResult result = null;
            try
            {
                int currentUserId = _authService.GetCurrentUserId();
                int id = _service.Add(model, currentUserId);
                ItemResponse<int> response = new ItemResponse<int> { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }
            return result;
        }
        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(UpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int currentUserId = _authService.GetCurrentUserId();
                _service.Update(model, currentUserId);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }
        [HttpPut("{id:int}/{statusId:int}")]
        public ActionResult<SuccessResponse> UpdateStatusId(UpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int currentUserId = _authService.GetCurrentUserId();
                _service.Update(model, currentUserId);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }


        [HttpPost("addsurvey")]
        public ActionResult<SuccessResponse> AddSurvey(SurveyAddRequest model)
        {

            int code = 200;
            BaseResponse response = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.AddSurvey(model, userId);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }


    }

}