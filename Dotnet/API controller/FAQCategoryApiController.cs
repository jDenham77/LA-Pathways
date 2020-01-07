using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain;
using Sabio.Models.Requests.FAQCategory;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/faqsCategories")]
    [ApiController]
    public class FAQCategoryApiController : BaseApiController
    {
        private IFAQCategoryService _service = null;
        private IAuthenticationService<int> _authService;
        public FAQCategoryApiController(IFAQCategoryService service,
            ILogger<FAQCategoryApiController> logger,
            IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet]
        public ActionResult<ItemsResponse<FAQCategory>> Get()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                List<FAQCategory> list = _service.Get();
                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found");
                }
                else
                {
                    response = new ItemsResponse<FAQCategory> { Items = list };
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
        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(FAQCategoryAddRequest model)
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
        public ActionResult<ItemResponse<int>> Update(FAQCategoryUpdateRequest model)
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
        [HttpDelete("{id:int}/delete")]
        public ActionResult<SuccessResponse> Delete(int Id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
               _service.Delete(Id);
                response = new SuccessResponse();
            }
            catch (Exception Ex)
            {
                code = 500;
                response = new ErrorResponse(Ex.Message);
                base.Logger.LogError(Ex.ToString());
            }
            return StatusCode(code, response);
        }
    }
}