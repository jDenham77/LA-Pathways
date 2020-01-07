
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Surveys;
using Sabio.Models.Requests;
using Sabio.Models.Requests.Surveys;
using Sabio.Services;
using Sabio.Services.Interfaces.Surveys;
using Sabio.Services.Surveys;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/surveys/answers")]
    [ApiController]
    public class SurveyAnswerApiController : BaseApiController
    {
        private IAnswerService _service = null;
        private IInstancesService _instanceService = null;
        private IAuthenticationService<int> _authenticationService = null;

        public SurveyAnswerApiController(IAnswerService service,
            IInstancesService instancesService, 
            IAuthenticationService<int> authService, 
            ILogger<SurveyAnswerApiController> logger) : base(logger)
        {
            _service = service;
            _instanceService = instancesService;
            _authenticationService = authService;
        }
        [HttpGet]
        public ActionResult<ItemResponse<Paged<SurveyAnswer>>> SelectAll(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<SurveyAnswer> page = _service.SelectAll(pageIndex, pageSize);

                response = new ItemResponse<Paged<SurveyAnswer>>() { Item = page };

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<SurveyAnswer>> { Item = page };
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
        public ActionResult<ItemResponse<SurveyAnswer>> GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                SurveyAnswer surveyAnswer = _service.GetById(id);

                if (surveyAnswer == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<SurveyAnswer> { Item = surveyAnswer };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"ArgumentException: {ex.Message }");
            }
            return StatusCode(code, response);

        }
        [HttpPost("graph")]
        public ActionResult<ItemResponse<List<AnswerOptionWithDetails>>> GetByQuestionId(GraphRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                List<AnswerOptionWithDetails> list = _service.GetByQuestionId(model);

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<List<AnswerOptionWithDetails>> { Item = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"ArgumentException: {ex.Message }");
            }
            return StatusCode(code, response);

        }
        [HttpGet("questioninfo")]
        public ActionResult<ItemResponse<List<Question>>> getQuestionInfoFromAnswers()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<Question> list = _service.getQuestionInfoFromAnswers();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<List<Question>>() { Item = list };
                }


            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);

        }
        [HttpGet("questionanswer/{id:int}")]
        public ActionResult<ItemResponse<QuestionAnswer>> GetByQAId(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                QuestionAnswer questionAnswer = _service.GetByQAId(id);

                if (questionAnswer == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<QuestionAnswer> { Item = questionAnswer };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"ArgumentException: {ex.Message }");
            }
            return StatusCode(code, response);
        }
        public ActionResult<SuccessResponse> Add(AnswerAddRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {

                bool idList = _service.Add(model);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);

            }
            return StatusCode(code, response);
        }
        
        [HttpPost("submit"), AllowAnonymous]
        public ActionResult<ItemResponse<int>> AddMultiple(AnswerAddRequestMultiple model)
        {
            int code = 200;
            BaseResponse response = null;
            int currentUserId = 122;
            int instanceId = 0;
            //currentUserId = _authenticationService.GetCurrentUserId();
            try
            {
                
                
                instanceId = _instanceService.AddFromSurveyId(model.SurveyId, currentUserId);
                _service.AddMultiple(model, instanceId);
                response = new ItemResponse<int> { Item = instanceId };
            }
            catch (Exception ex)
            {
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(AnswerUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                _service.Update(model);

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
