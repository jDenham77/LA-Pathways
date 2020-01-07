using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain;
using Sabio.Services;
using Sabio.Services.Interfaces.Security;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/states")]
    [ApiController]
    public class StatesApiController : BaseApiController
    {
        private IStatesService _service = null;
        private IAuthenticationService<int> _authService = null;
        public StatesApiController(IStatesService service
            , ILogger<StatesApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        [HttpGet("{id:int}")]
        public ActionResult<ItemsResponse<State>> Get(int CountryId)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<State> list = _service.Get(CountryId);

                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {

                    response = new ItemsResponse<State> { Items = list };
                }
            }

            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }

            return StatusCode(iCode, response);
        }
    }
}