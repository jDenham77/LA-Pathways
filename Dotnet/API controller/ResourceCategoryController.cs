using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.ResourceCategory;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;

namespace Sabio.Web.Api.Controllers
{
    
    [Route("api/resources/Categories"), AllowAnonymous]
    [ApiController]
    public class ResourceCategoryController : BaseApiController
    {
        private IResourceCategoryService _service = null;
        private IAuthenticationService<int> _authService = null;

        public ResourceCategoryController(IResourceCategoryService service
            , ILogger<ResourceCategoryController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        [HttpGet]
        public ActionResult<ItemsResponse<Paged<ResourceCategory>>> GetByPage(int PageIndex, int PageSize)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<ResourceCategory> page = _service.GetByPage(PageIndex, PageSize);

                if (page == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource Not Found.");
                }
                else
                {
                    response = new ItemResponse<Paged<ResourceCategory>> { Item = page };
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
        [HttpGet("{id:int}")]
        public ActionResult GetByResourceId(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                ResourceCategory resourceCategory = _service.GetByResourceId(id);
                if(resourceCategory == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("404 Not Found.");
                }
                else
                {
                    response = new ItemResponse<ResourceCategory> { Item = resourceCategory };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: ${ex.Message}");
            }
            return StatusCode(iCode, response);
        }
        [HttpGet("locations/{id:int}")]
        public ActionResult GetByLocationType(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                ResourceCategory resourceCategory = _service.GetByLocationType(id);
                if (resourceCategory == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("404 Not Found.");
                }
                else
                {
                    response = new ItemResponse<ResourceCategory> { Item = resourceCategory };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: ${ex.Message}");
            }
            return StatusCode(iCode, response);
        }
        [HttpGet("contract/type/{id:int}")]
        public ActionResult GetByContractingType(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                ResourceCategory resourceCategory = _service.GetByContractingType(id);
                if (resourceCategory == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("404 Not Found.");
                }
                else
                {
                    response = new ItemResponse<ResourceCategory> { Item = resourceCategory };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: ${ex.Message}");
            }
            return StatusCode(iCode, response);
        }
        [HttpGet("consulting/{id:int}")]
        public ActionResult GetByConsultingType(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                ResourceCategory resourceCategory = _service.GetByConsultingType(id);
                if (resourceCategory == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("404 Not Found.");
                }
                else
                {
                    response = new ItemResponse<ResourceCategory> { Item = resourceCategory };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: ${ex.Message}");
            }
            return StatusCode(iCode, response);
        }
        [HttpPost]
        public ActionResult<SuccessResponse> Create(ResourceCategoryAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                IUserAuthData user = _authService.GetCurrentUser();
                bool id = _service.Add(model, user.Id);

                ItemResponse<bool> response = new ItemResponse<bool>() { Item = id }; ;

                result = Created201(response);
            }
            catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }
        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<bool>> Update(ResourceCategoryUpdateRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                _service.Update(model);

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
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);

                response = new SuccessResponse();
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
