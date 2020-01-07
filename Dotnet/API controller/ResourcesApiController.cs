using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.CategoriesTypes;
using Sabio.Models.Requests;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using SendGrid;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/resources")]
    [ApiController]
    public class ResourcesApiController : BaseApiController
    {
        private IResourceService _service = null;
        private IEmailService _emailService = null;
        private IAuthenticationService<int> _authService = null;
        private SendGridClientOptions _sendGridKey = null;
        public ResourcesApiController(IResourceService service
            , ILogger<ResourcesApiController> logger
            , IEmailService emailService 
            , IAuthenticationService<int> authService
            , IOptions<SendGridClientOptions> sendGridKey) : base(logger)
        {
            _service = service;
            _authService = authService;
            _emailService = emailService;
            _sendGridKey = sendGridKey.Value;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Add(ResourceAddRequest model)
        {
            ObjectResult result = null;
            int code = 200;

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
            return StatusCode(code, result);
        }
        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<SuccessResponse>> Update(ResourceUpdateRequest model)
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
        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Resource>>> GetAll(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<Resource> page = _service.GetAll(pageIndex, pageSize);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Resource>> { Item = page };
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
        [HttpGet("all")]
        public ActionResult<ItemsResponse<List<Condition>>> GetAllRec()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                List<Condition> list = _service.GetAllRec();
                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found");
                }
                else
                {
                    response = new ItemsResponse<Condition> { Items = list };
                }
            }
            catch (Exception Ex)
            {
                code = 500;
                response = new ErrorResponse(Ex.Message);
                base.Logger.LogError(Ex.ToString());
            }
            return StatusCode(code, response);
        }
        [HttpGet("search")]
        public ActionResult<ItemResponse<Resource>> Search(int pageIndex, int pageSize, string q)
            {
                int iCode = 200;
                BaseResponse response = null;
                try
                {
                    Paged<Resource> resource = _service.Search(pageIndex, pageSize, q);
                    if (resource == null)
                    {
                        iCode = 404;
                        response = new ErrorResponse("Application Resource not found.");
                    }
                    else
                    {
                        response = new ItemResponse<Paged<Resource>> { Item = resource };
                    }
                }
                catch (Exception ex)
                {
                    iCode = 500;
                    response = new ErrorResponse(ex.Message);
                }
                return StatusCode(iCode, response);
            }
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Resource>> Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                Resource resource = _service.Get(id);
                if (resource == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Resource> { Item = resource };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse($"ArgumentException Error: ${ex.Message}");
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(iCode, response);

        }
        [HttpDelete("{id:int}")]
        public ActionResult<ItemResponse<SuccessResponse>> Delete(int id)
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
        [HttpGet("types")]
        public ActionResult<ItemResponse<Categories>> GetResourceType()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
               Categories categories = _service.GetResourceType();
                if (categories == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Categories> { Item = categories };
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
        [HttpPost("emails")]
        public ActionResult<ItemResponse<int>> SendEmails(EmailListAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                _emailService.SendEmails(model, _sendGridKey);
                result = Ok200(new SuccessResponse());
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new ErrorResponse(ex.Message));
            }
            return result;
        }

    }
}