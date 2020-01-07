using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sabio.Models.Requests;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using SendGrid;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/contact/email"),AllowAnonymous]
    public class ContactApiController : BaseApiController
    {
        private IEmailService _service = null;
        private IAuthenticationService<int> _authService;
        private SendGridClientOptions _sendGridKey;

        public ContactApiController(IEmailService service
            , IAuthenticationService<int> authService
            , ILogger<PingApiController> logger
            , IOptions<SendGridClientOptions> sendGridKey) : base(logger)
        {
            _service = service;
            _authService = authService;
            _sendGridKey = sendGridKey.Value;
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponse>> Contact(ContactAddRequest model)
        {
            ObjectResult response = null;
            try
            {

                Response sendGridResp = await _service.ContactEmail(model, _sendGridKey);

                if (sendGridResp.StatusCode == HttpStatusCode.Accepted)
                {
                    response = StatusCode(201, new SuccessResponse());
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