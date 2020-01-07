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
using Sabio.Models.Requests.Surveys;
using Sabio.Services;
using Sabio.Services.Surveys;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/survey/sections")]
    [ApiController]

    public class SurveySectionsApiController : BaseApiController
    {
        private ISectionsService _service = null;
        private IAuthenticationService<int> _authService = null;

        public SurveySectionsApiController(ISectionsService service, ILogger<SurveySectionsApiController> logger,
            IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Insert(SectionsAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int id = _service.Insert(model);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
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

        [HttpGet]
        public ActionResult<ItemResponse<Paged<Section>>> SelectAll(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;//do not declare an instance.

            try
            {
                Paged<Section> page = _service.SelectAll(pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Section>> { Item = page };
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

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Section>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Section survey = _service.GetById(id);

                if (survey == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found");
                }
                else
                {
                    response = new ItemResponse<Section>() { Item = survey };
                }
            }
            catch (SqlException sqlEx)
            {
                iCode = 500;

                response = new ErrorResponse($"SqlException Error: {sqlEx.Message}");

            }

            catch (ArgumentException argEx)
            {
                iCode = 500;

                response = new ErrorResponse($"ArgumentException Error: {argEx.Message}");

            }

            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }


            return StatusCode(iCode, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(SectionsUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int currentId = _authService.GetCurrentUserId();

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