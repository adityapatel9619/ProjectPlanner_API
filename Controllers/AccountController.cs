using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectPlanner_API.IMethod;
using ProjectPlanner_API.Models;

namespace ProjectPlanner_API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _account;

        public AccountController(IAccount account)
        {
            _account = account;
        }

        [HttpPost]
        [ActionName("ValidateUserName")]
        public async Task<ActionResult> ValidateUsername(RegistrationModel registration)
        {
            string message = string.Empty;
            try
            {
                if (registration != null && (!string.IsNullOrEmpty(registration.UserName)))
                {
                    List<RegistrationModel> listModel = new List<RegistrationModel>();

                    listModel = _account.ValidateUsername(registration.UserName);

                    if (listModel.Count > 0)
                    {
                        message = "Try Another Username";
                        return StatusCode(StatusCodes.Status302Found, new
                        {
                            message,
                            isUsernameTaken = true,
                            data = listModel
                        });
                    }
                    else
                    {
                        message = "Username Accepted";
                        return Ok(new
                        {
                            message,
                            isUsernameTaken = false,
                            data = registration
                        });
                    }
                }
                else
                {
                    message = "No Content";
                    return StatusCode(StatusCodes.Status204NoContent, new { message });
                }
            }
            catch (Exception ex)
            {
                message = "Something Went Wrong !!!!";
                return StatusCode(StatusCodes.Status500InternalServerError, new { message });
            }
        }
    }
}
