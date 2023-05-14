using InternetShopWebApp.DTO;
using InternetShopWebApp.Models;
using InternetShopWebApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Security.Claims;
using WebAPI.Models;

namespace ASPNetCoreApp.Controllers
{
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/account/getusers")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            return ListUsersDTO().Result;
        }

        [HttpPost]
        [Route("api/account/register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var users = ListUsers();
                var MaxIndex = users.Max(a => a.NormalCode);
                User user = new() { Email = model.Email, UserName = model.Email, NormalCode=MaxIndex+1, };
                // Добавление нового пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Установка роли User
                    await _userManager.AddToRoleAsync(user, "user");
                    // Установка куки
                    await _signInManager.SignInAsync(user, false);
                    _logger.LogInformation("Register: "+ user.UserName);
                    return Ok(new { message = "Сессия активна", userName = user.UserName, userRole="user", userCode = user?.Id, userID = user.NormalCode, isAuthenticated = true, });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    var errorMsg = new
                    {
                        message = "Пользователь не добавлен",
                        error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                    };
                    return Created("", errorMsg);
                }
            }
            else
            {
                var errorMsg = new
                {
                    message = "Неверные входные данные",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };
                return Created("", errorMsg);
            }
        }

        [HttpPost]
        [Route("api/account/login")]
        //[AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    IList<string>? roles = await _userManager.GetRolesAsync(user);
                    string? userRole = roles.FirstOrDefault();
                    _logger.LogInformation("Login: " + user.UserName);
                    return Ok(new { message = "Сессия активна", userName = user.UserName, userRole, userCode = user?.Id, userID = user.NormalCode, isAuthenticated = true, });
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    var errorMsg = new
                    {
                        message = "Вход не выполнен",
                        error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                    };
                    return Created("", errorMsg);
                }
            }
            else
            {
                var errorMsg = new
                {
                    message = "Вход не выполнен",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };
                return Created("", errorMsg);
            }
        }

        [HttpPost]
        [Route("api/account/logoff")]
        public async Task<IActionResult> LogOff()
        {
            User usr = await this.GetCurrentUserAsync();
            if (usr == null)
            {
                return Unauthorized(new { message = "Сначала выполните вход" });
            }
            // Удаление куки
            await _signInManager.SignOutAsync();
            _logger.LogInformation("LogOff: " + usr.UserName);
            return Ok(new { message = "Выполнен выход", userName = usr.UserName });
        }

        [HttpGet]
        [Route("api/account/isauthenticated")]
        public async Task<IActionResult> IsAuthenticated()
        {
            User usr = await this.GetCurrentUserAsync();
            if (usr == null)
            {
                return Unauthorized(new { message = "Вы Гость. Пожалуйста, выполните вход", isAuthenticated = false, });
            }
            IList<string> roles = await _userManager.GetRolesAsync(usr);
            string? userRole = roles.FirstOrDefault();
            return Ok(new { message = "Сессия активна", userName = usr.UserName, userRole, userCode = usr?.Id, userID=usr.NormalCode, isAuthenticated=true, });

        }

        [Route("api/account/getuserswithroles")]
        [HttpGet]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var users = _userManager.Users.ToList();
            var userRoles = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserDto
                {
                    Id=user.NormalCode,
                    UserName = user.UserName,
                    Roles = roles
                });
            }

            return Ok(userRoles);
        }

        [Route("api/account/updateUserRole/{userId}/{newRole}")]
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUserRole(int userId, string newRole)
        {
            var allUsers = ListUsers();
            // Находим пользователя по ID
            var user = allUsers.Where(a => a.NormalCode == userId).FirstOrDefault();

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Получаем текущие роли пользователя
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Удаляем пользователя из всех текущих ролей
            var removeRoleResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeRoleResult.Succeeded)
            {
                return BadRequest("Failed to remove user from current roles");
            }

            // Добавляем пользователя в новую роль
            var addRoleResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addRoleResult.Succeeded)
            {
                return BadRequest("Failed to add user to new role");
            }

            return Ok("Role updated successfully");
        }

        public List<User> ListUsers()
        {
            var users = _userManager.Users;
            return users.ToList();
        }

        public async Task<List<UserDto>> ListUsersDTO()
        {
            var users = _userManager.Users.ToList();
            var userRoles = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserDto
                {
                    Id = user.NormalCode,
                    UserName = user.UserName,
                    Roles = roles,
                });
            }

            return userRoles;
        }

        [HttpGet]
        [Route("api/account/getid")]
        public async Task<IActionResult> GetUserId()
        {
            User usr = await this.GetCurrentUserAsync();
            if (usr == null)
            {
                return Unauthorized(new { message = "Вы Гость. Пожалуйста, выполните вход" });
            }

            return Ok(new { message = "Сессия активна", userCode = usr.NormalCode});

        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}