using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectPlanner_API.IMethod;
using ProjectPlanner_API.Models;
using Microsoft.Extensions.Logging;

namespace ProjectPlanner_API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _account;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccount account,ILogger<AccountController> logger)
        {
            _account = account;
            _logger = logger;
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

                    listModel = await _account.ValidateUsername(registration.UserName);

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
                _logger.LogError(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> BindDepartments()
        {
            try
            {
                var depts = await _account.GetDepartments();

                if (depts != null)
                {
                    return Ok(depts);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ActionName("AddNewEmployee")]
        public async Task<ActionResult> SaveNewEmployee([FromBody]NewEmployeeModel model)
        {
            string message = string.Empty;
            try
            {
                if (model == null)
                {
                    message = "Data is Invalid";
                    _logger.LogWarning("Data is Invalid");
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        message,
                        isError = false,
                        data = model
                    });
                }
                else
                {
                    NewEmployeeModel savedData = new NewEmployeeModel();
                     savedData =  await _account.SaveNewEmployee(model);

                    if (savedData != null)
                    {
                        message = "New Employee Registered";
                        return StatusCode(StatusCodes.Status201Created, new
                        {
                            message,
                            isError = false,
                            data = model
                        });
                    }
                    else
                    {
                        message = "Error during Registration";
                        return StatusCode(StatusCodes.Status400BadRequest, new
                        {
                            message,
                            isError = true,
                            data = model
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                message = "Something Went Wrong !!!!";
                _logger.LogError(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message });
            }
        }
    }
}
