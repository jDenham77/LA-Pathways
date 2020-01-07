using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sabio.Web.Controllers;
using Sabio.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Sabio.Web.Models.Responses;
using Microsoft.AspNetCore.Http;
using Sabio.Models.Requests.Files;
using Sabio.Services;
using Sabio.Models.AppSettings;
using Microsoft.Extensions.Options;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class S3BucketFileUploadController : BaseApiController
    {

        IS3Service _service = null;
        IAuthenticationService<int> _authService = null;
        AWSCredentials _awsCredentials = null;

        public S3BucketFileUploadController(IS3Service service,
            IAuthenticationService<int> authenticationSerivce,
            IOptions<AWSCredentials> awsCredentials,
            ILogger<S3BucketFileUploadController> logger) : base(logger)
        {
            _service = service;
            _authService = authenticationSerivce;
            _awsCredentials = awsCredentials.Value;
        }


        [HttpPost]
        public ActionResult<ItemResponse<List<string>>> AddFile(List<IFormFile> files)
        {
            int code = 200;
            BaseResponse response = null;
            List<string> result = null;

            try
            {

                if (files == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");
                };

                if (HttpContext.Request.ContentLength > 0)
                {
                    foreach (var file in files)
                    {
                        Task<string> value = null;

                        value = _service.UploadFileAsync(file);

                        string url = value.Result;
                        
                        if(result == null)
                        {
                            result = new List<string>();
                        }

                        result.Add(url);
                    }

                    if (result == null)
                    {
                        code = 404;
                        response = new ErrorResponse("Files were not uploaded.");
                    }
                    else
                    {
                        response = new ItemResponse<List<string>> { Item = result };
                    }

                }
            }

            catch (Exception ex)
            {
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpPost("db")]
        public ActionResult<ItemResponse<bool>> UploadFileToDB(FileAddRequest model)
        {
            ObjectResult result = null;
            bool success = true;
            int currentUserId = 0;
            try
            {
                currentUserId = _authService.GetCurrentUserId();
                success = _service.Add(model, currentUserId);
                ItemResponse<bool> response = new ItemResponse<bool> { Item = success };
                result = Created201(response);
            }
            catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }
            return result;
        }
    }

}