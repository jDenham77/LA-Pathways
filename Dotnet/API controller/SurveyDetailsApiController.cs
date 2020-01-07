using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/surveydetails")]
    public class SurveyDetailsApiController : BaseApiController
    {
        private ISurveyDetailsService _service = null;
        private IAuthenticationService<int> _authService = null;
        public SurveyDetailsApiController(ISurveyDetailsService service,
            ILogger<SurveyDetailsApiController> logger,
            IAuthenticationService<int> authenticationService) : base(logger)
        {
            _service = service;
            _authService = authenticationService;
        }
        
        [HttpGet("{id:int}"), AllowAnonymous]
        public ActionResult<ItemResponse<SurveyDetail>> GetByIdDetails(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                SurveyDetail result = _service.GetByIdDetails(id);
                if (result == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<SurveyDetail> { Item = result };
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
