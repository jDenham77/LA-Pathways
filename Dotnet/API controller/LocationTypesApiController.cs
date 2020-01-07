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
    [Route("api/locationtypes")]
    public class LocationTypesApiController : BaseApiController
    {
        private ILocationTypesService _service = null;
        private IAuthenticationService<int> _authService = null;
        public LocationTypesApiController(ILocationTypesService service
            , ILogger<LocationTypesApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        [HttpGet]
        public ActionResult<ItemsResponse<LocationType>> Get()
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<LocationType> list = _service.Get();

                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {

                    response = new ItemsResponse<LocationType> { Items = list };
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