using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Surveys;
using Sabio.Models.Requests.Surveys;
using Sabio.Services;
using Sabio.Services.Surveys;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;


namespace Sabio.Web.Api.Controllers
{
    [Route("api/surveys/questions/answeroptions")]
    [ApiController]
    public class AnswerOptionApiController : BaseApiController
    {
        private IAnswerOptionsService _service = null;
        private IAuthenticationService<int> _authService = null;
        public AnswerOptionApiController(IAnswerOptionsService service
            , ILogger<PingApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        
        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(AnswerOptionAddRequest model)
        {
            
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {

                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);
                return StatusCode(500, response);
            }
            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(AnswerOptionUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.Update(model, userId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);

            }
            return StatusCode(code, response);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<AnswerOption>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                AnswerOption answerOptions = _service.Get(id);

                if (answerOptions == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Resource not found.");

                }
                else
                {
                    response = new ItemResponse<AnswerOption> { Item = answerOptions };
                }
            }
           
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Errors:{ ex.Message }");

            }

            return StatusCode(iCode, response);
        }

        [HttpGet("questionid/{Qid:int}")]
        public ActionResult<ItemsResponse<List<AnswerOption>>> GetQuestionId(int QId)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
              List<AnswerOption> answer = _service.GetQuestionId(QId);

                if (answer == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Resource not found.");

                }
                else
                {
                    response = new ItemsResponse<AnswerOption> { Items = answer };
                }
            }

            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Errors:{ ex.Message }");

            }

            return StatusCode(iCode, response);
        }

        [HttpGet]
        public ActionResult<ItemResponse<Paged<AnswerOption>>> Pagination(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<AnswerOption> page = _service.Pagination(pageIndex, pageSize);
                response = new ItemResponse<Paged<AnswerOption>>() { Item = page };
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<AnswerOption>> { Item = page };
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

        [HttpGet("paginate/current")]
        public ActionResult<ItemResponse<Paged<AnswerOption>>> PaginationCreatedBy(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            
            try
            {
                int userId = _authService.GetCurrentUserId();

                Paged<AnswerOption> page = _service.PaginationCreatedBy(userId,pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<AnswerOption>> { Item = page };
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

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;


            try
            {
                _service.Delete(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);

            }
            return StatusCode(code, response);
        }

        [HttpPut("multiple/update")]
        public ActionResult<SuccessResponse> UpdateMultiple(List<AnswerOptionUpdateMultipleRequest> model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                
                _service.UpdateMultiple(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);

            }
            return StatusCode(code, response);
        }

        [HttpPost("multiple")]
        public ActionResult<ItemResponse<int>> CreateMultiple(List<AnswerOptionAddMultipleRequest> model)
        {
         
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.Add_Multiple(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {

                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);
                return StatusCode(500, response);
            }
            return result;
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

        [HttpGet("paginate/qamultiple")]
        public ActionResult<ItemResponse<Paged<QuestionAnswer>>> GetQAMultiple(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<QuestionAnswer> page = _service.GetQAMultiple(pageIndex, pageSize);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<QuestionAnswer>> { Item = page };
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
        [HttpGet("alloptiontypes")]
        public ActionResult<ItemsResponse<OptionType>> GetAllOptionTypes()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                List<OptionType> list = _service.GetAllOptionTypes();
                response = new ItemsResponse<OptionType>() { Items = list };
                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<OptionType> { Items = list };
                }
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