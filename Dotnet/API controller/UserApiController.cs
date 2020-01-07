using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using Sabio.Models.Requests.Users;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using SendGrid;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersApiController : BaseApiController
    {
        private IUserService _service = null;
        private IUserTokensService _tokenService = null; 
        private IEmailService _emailService = null;   
        private IAuthenticationService<int> _authService = null;
        private SendGridClientOptions _sendGridKey;

        public UsersApiController(IUserService service,
            IUserTokensService tokenService, 
            IEmailService emailService,
            ILogger<UsersApiController> logger,
            IAuthenticationService<int> authenticationService,
            IOptions<SendGridClientOptions> sendGridKey) : base(logger)
        {
            _tokenService = tokenService; 
            _emailService = emailService;
            _service = service;
            _authService = authenticationService;
            _sendGridKey = sendGridKey.Value;
        }

        [HttpPost("register"), AllowAnonymous]
        public ActionResult<ItemResponse<int>> Create(UserAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                int id = _service.Create(model);
                string email = model.Email;

                Guid token = Guid.NewGuid();

                _tokenService.Add(id, token);

                _emailService.ConfirmEmail(email, token, _sendGridKey);

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

        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<SuccessResponse>> Login(LoginAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                UserAuth user = await _service.Authenticate(model.Email, model.Password);
                if (user != null)
                {
                    result = Ok200(new SuccessResponse());
                }
                else
                {
                    result = StatusCode(404, new ErrorResponse("Email or password do not match our records!"));
                }
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new ErrorResponse(ex.Message));
            }
            return result;
        }

        [HttpGet("logout")]
        public ActionResult<SuccessResponse> Logout()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                _service.LogOutUser(); //logger out service
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpGet("current")]
        public ActionResult<ItemResponse<IUserAuthData>> GetCurrrent()
        {
            IUserAuthData user = _authService.GetCurrentUser();
            ItemResponse<IUserAuthData> response = new ItemResponse<IUserAuthData>();
            response.Item = user;

            return Ok200(response);
        }

        [HttpGet("{token:Guid}")]
        public ActionResult<SuccessResponse> ConfirmAcct(Guid token)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                _service.ConfirmAcct(token);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpGet("forgot"), AllowAnonymous]
        public ActionResult<SuccessResponse> ForgotPassword(string email)
        {
            ObjectResult result = null;
            try
            {

                int id = _service.ForgotPassword(email);

                Guid token = Guid.NewGuid();

                _tokenService.Add(id, token);

                _emailService.ResetEmail(email, token, _sendGridKey);


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

        [HttpPost("reset"), AllowAnonymous]
        public ActionResult<ItemResponse<int>> GetUserByGuid(UpdatePassword model)
        {
            ObjectResult result = null;
            try
            {
                int id =_service.ResetPassword(model);
                                                       
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

    
