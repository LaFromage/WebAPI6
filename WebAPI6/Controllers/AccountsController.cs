using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI6.Data;
using WebAPI6.Models;
using WebAPI6.Repositories;

namespace WebAPI6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository accountRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountsController(IAccountRepository repo, UserManager<ApplicationUser> userManager)
        {
            accountRepo = repo;
            this.userManager = userManager;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            var result = await accountRepo.SignUpAsync(signUpModel);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }

            return Unauthorized();
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            var result = await accountRepo.SignInAsync(signInModel);

            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }

            return Ok(result);
        }

        [HttpGet("GetAllUsers")]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            var users = userManager.Users.ToList();
            return Ok(users);
        }

        [HttpPut("UpdateUser/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateInfoModel updateInfoModel)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Cập nhật thông tin người dùng
            user.FirstName = updateInfoModel.FirstName;
            user.LastName = updateInfoModel.LastName;
            user.Email = updateInfoModel.Email;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok("User updated successfully");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

    }
}
