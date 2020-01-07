using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using SendGrid;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sabio.Web.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class EmailAPIController : BaseApiController
    {
        private IEmailService _service = null;
        private IAuthenticationService<int> _authService;
        private SendGridClientOptions _sendGridKey = null;

        public EmailAPIController(IEmailService service
            , IAuthenticationService<int> authService
            , ILogger<PingApiController> logger,
            IOptions<SendGridClientOptions> sendGridKey) : base(logger)
        {
            _service = service;
            _authService = authService;
            _sendGridKey = sendGridKey.Value;
        }
        [HttpGet("confirm/{token}")]
        public async Task<ActionResult<SuccessResponse>> Confirm(EmailAddRequest model)
        {
            ObjectResult response = null;
            try
            {
                Guid token = Guid.NewGuid();
                
                Response sendGridResp = await _service.ConfirmEmail(model.To, token, _sendGridKey);

                if (sendGridResp.StatusCode == HttpStatusCode.Accepted) 
                {
                    response = StatusCode(202, new SuccessResponse()); 
                }
                else  
                {
                    response = StatusCode(404, new ErrorResponse("Not Found, Try Again"));
                }
            }
            catch (Exception ex) 
            {
                response = StatusCode(500, new ErrorResponse(ex.Message));
            }

            return response;
        }
    }
}