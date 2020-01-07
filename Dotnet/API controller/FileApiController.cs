    using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Files;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using SendGrid;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FileApiController : BaseApiController
    {
        private IFilesService _service = null;
        private IAuthenticationService<int> _authService = null;
        private IEmailService _emailService = null;
        private SendGridClientOptions _sendGridKey = null;
        public FileApiController(IFilesService service
            , ILogger<FileApiController> logger
            , IEmailService emailService
            , IAuthenticationService<int> authService,
            IOptions<SendGridClientOptions> sendGridKey) : base(logger)
            
        {
            _service = service;
            _authService = authService;
            _emailService = emailService;
            _sendGridKey = sendGridKey.Value;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<File>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                File file = _service.Get(id);

                if (file == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Resource not found.");

                }
                else
                {
                    response = new ItemResponse<File> { Item = file };
                }
            }

            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Errors:{ ex.Message }");

            }

            return StatusCode(iCode, response);
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<File>>> Pagination(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<File> page = _service.Pagination(pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<File>> { Item = page };
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

        [HttpGet("paginate/current")]
        public ActionResult<ItemResponse<Paged<File>>> PaginationCreatedBy( int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                Paged<File> page = _service.PaginationCreatedBy(userId, pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<File>> { Item = page };
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

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
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
        
        
        [HttpPost("pdf"), AllowAnonymous]
        public ActionResult<SuccessResponse> UploadFileToDB(IFormFile file,string email)
        {
            int code = 200;
            BaseResponse response = null;
           
            try
            {
                _emailService.EmailResourcePdf(file, email, _sendGridKey);
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