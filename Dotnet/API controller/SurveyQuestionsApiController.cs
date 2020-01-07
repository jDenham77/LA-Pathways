using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Surveys;
using Sabio.Models.Requests;
using Sabio.Services;
using Sabio.Services.Surveys;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/surveys/questions")]
    [ApiController]
    public class SurveyQuestionsApiController : BaseApiController
    {
        private IQuestionServices _service = null;
        private IAuthenticationService<int> _authService = null;
        public SurveyQuestionsApiController(IQuestionServices service, ILogger<SurveyQuestionsApiController> logger
             , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet]
        public ActionResult<ItemResponse<Paged<QuestionDetails>>> GetQuestions(int pageIndex, int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<QuestionDetails> page = _service.GetQuestions(pageIndex, pageSize);

                if (page == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<QuestionDetails>> { Item = page };
                }

            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(iCode, response);
        }
        [HttpGet("all")]
        public ActionResult<ItemsResponse<Question>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                List<Question> list = _service.GetAll();
                response = new ItemsResponse<Question>() { Items = list };
                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<Question> { Items = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }
        [HttpGet("details")]
        public ActionResult<ItemResponse<QuestionOptionDetails>> GetQuestionOptionDetails()
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                QuestionOptionDetails qOD = _service.GetQuestionOptionDetails();

                if (qOD == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<QuestionOptionDetails> { Item = qOD };
                }

            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(iCode, response);
        }
        [HttpPost]
        public ActionResult<ItemResponse<int>> Add(QuestionAddRequest model)
        {

            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                int id = _service.Add(model, userId);

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
        public ActionResult<ItemResponse<SuccessResponse>> Update(QuestionUpdateRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                _service.Update(model, userId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(iCode, response);
        }
        [HttpDelete("{id:int}")]
        public ActionResult<ItemResponse<SuccessResponse>> Delete(int Id)
        {
            BaseResponse response = null;
            int iCode = 200;

            try
            {
                _service.Delete(Id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(iCode, response);
        }
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Question>> GetById(int Id)
        {
            BaseResponse response = null;
            int iCode = 200;
            Question question = null;

            try
            {
                question = _service.GetById(Id);

                if (question == null)
                {
                    iCode = 400;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<Question> { Item = question };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

    }

}