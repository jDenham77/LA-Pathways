using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Venues;
using Sabio.Services;
using Sabio.Services.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/venues")]
    [ApiController]
    public class VenuesApiController : BaseApiController
    {
        private IVenueServices _service = null;
        private IAuthenticationService<int> _authService = null;
        public VenuesApiController(IVenueServices service, ILogger<VenuesApiController> logger
             , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Add(AddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                int Id = _service.Add(model, userId);

                ItemResponse<int> response = new ItemResponse<int> { Item = Id };

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

        [HttpGet]
        public ActionResult<Paged<Venue>> GetPaginatedVenues(int pageIndex, int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<Venue> page = _service.GetPaginatedVenues(pageIndex, pageSize);

                if (page == null)
                {
                    iCode = 404;

                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Venue>> { Item = page };
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
        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<SuccessResponse>> Update(UpdateRequest model)
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
        public ActionResult<ItemResponse<Venue>> GetById(int Id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Venue venue = _service.GetById(Id);

                if (venue == null)
                {
                    iCode = 400;

                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<Venue> { Item = venue };
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