using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Requests.Recommendations;
using Sabio.Services;
using Sabio.Services.Interfaces.Security;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using Sabio.Models.Domain;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/recommendations")]
    [ApiController]
    public class RecommendationsApiController : BaseApiController
    {
        private IRecommendationsService _service = null;
        public RecommendationsApiController(IRecommendationsService service
            , ILogger<RecommendationsApiController> logger) : base(logger)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Insert(RecommendationAddRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {

                _service.Insert(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);

        }
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Condition>> GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;
            Condition result = null;
            try
            {
                result = _service.GetById(id);
                if (result == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource Not Found");
                }
                else
                {
                    response = new ItemResponse<Condition> { Item = result };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                
            }
            return StatusCode(code, response);
        }

        [HttpPut("update")]
        public ActionResult<SuccessResponse> Update(RecommendationUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                _service.Update(model);
                response = new SuccessResponse();
            }
            catch(Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

    }
}