using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Event;
using Sabio.Models.Requests.Events;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsApiController : BaseApiController
    {
        private IEventService _service = null;

        public EventsApiController(IEventService service, ILogger<EventsApiController> logger) : base(logger)
        {
            _service = service;
        }
        [HttpGet]
        public ActionResult<ItemResponse<Paged<Event>>> SelectAll(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<Event> page = _service.SelectAll(pageIndex, pageSize);

                response = new ItemResponse<Paged<Event>>() { Item = page };

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse( "Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Event>> { Item = page };
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
        public ActionResult<ItemResponse<Event>> GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Event aEvent = _service.GetById(id);

                if (aEvent == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<Event> { Item = aEvent };
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
        public ActionResult<ItemResponse<int>> Create(EventAddRequest model)
        {
            ObjectResult result = null;
            try
            {
           
                int id = _service.Add(model);
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
        public ActionResult<ItemResponse<int>> Update(EventUpdateRequest model)
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
