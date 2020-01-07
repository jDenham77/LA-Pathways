using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/resource/recommendations")]
    [ApiController]
    public class ResourceRecommendationConfigApiController : BaseApiController
    {
        IResourceRecommendationConfigService _service = null;
        public ResourceRecommendationConfigApiController(IResourceRecommendationConfigService service, ILogger<ResourceRecommendationConfigApiController> logger) : base(logger)
        {
            _service = service;
        }
        
        [HttpGet("{id:int}"),AllowAnonymous]
        
        public ActionResult<ItemResponse<Paged<ResourceRecommendation>>> Get(int id, int pageIndex, int pageSize)
        {
            int code = 200;
            Paged<ResourceRecommendation> result = null;
            BaseResponse response = null;

            try
            {
                result = _service.GetByInstanceId(id, pageIndex, pageSize);

                if(result == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource found");
                }
                else
                {
                    response = new ItemResponse<Paged<ResourceRecommendation>>{ Item = result };
                }

            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);

        }
        
        [HttpGet("all/{id:int}"), AllowAnonymous]
        
        public ActionResult<ItemResponse<List<ResourceRecommendation>>> GetAllById(int id)
        {
            int code = 200;
            List<ResourceRecommendation> result = null;
            BaseResponse response = null;

            try
            {
                result = _service.GetAllByInstanceId(id);

                if (result == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource found");
                }
                else
                {
                    response = new ItemResponse<List<ResourceRecommendation>> { Item = result };
                }

            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);

        }
        [HttpGet("viewmore/{id:int}"), AllowAnonymous]
        
        public ActionResult<ItemResponse<ResourceRecommendation>> GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;
            ResourceRecommendation result = null;

            try
            {
                result = _service.GetByResourceId(id);
                if(result == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found");
                }
                else
                {
                    response = new ItemResponse<ResourceRecommendation> { Item = result };
                }
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