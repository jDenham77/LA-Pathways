using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/files/types")]
    [ApiController]
    public class FileTypesApiController : BaseApiController
    {
        IFileTypeService _service = null;

        public FileTypesApiController(IFileTypeService service, ILogger<FileTypesApiController> logger) : base(logger)
        {
            _service = service;
        }
        [HttpGet]
        public ActionResult<ItemResponse<List<FileType>>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;
            List<FileType> result = null;

            try
            {
                result = _service.GetAll();

                if(result == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");
                    
                }
                else
                {
                    response = new ItemResponse<List<FileType>> { Item = result };
                }
            }
            catch(Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

    }
}