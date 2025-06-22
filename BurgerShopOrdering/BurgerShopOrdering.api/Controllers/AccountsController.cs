using BurgerShopOrdering.api.Dtos.Accounts;
using BurgerShopOrdering.api.Dtos.Common;
using BurgerShopOrdering.core.Entities;
using BurgerShopOrdering.core.Services.Interfaces;
using BurgerShopOrdering.core.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BurgerShopOrdering.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(UserManager<ApplicationUser> userManager, IAccountService accountService, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IAccountService _accountService = accountService;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto registerUserRequestDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ApiResponse<object>.FailureResponse("Ongeldige invoer.", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))); }

            ApplicationUser newUser = new ApplicationUser(
                registerUserRequestDto.FirstName,
                registerUserRequestDto.LastName,
                registerUserRequestDto.Email,
                true);

            var user = await _userManager.FindByEmailAsync(registerUserRequestDto.Email);

            if (user != null)
            {
                return BadRequest(ApiResponse<object>.FailureResponse("Deze gebruiker is reeds geregistreerd."));
            }

            IdentityResult result = await _userManager.CreateAsync(newUser, registerUserRequestDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(ApiResponse<object>.FailureResponse("Gebruiker aanmaken mislukt.", errors));
            }

            var role = await _roleManager.FindByNameAsync("Client");

            if (role == null)
            {
                return StatusCode(500, ApiResponse<object>.FailureResponse("Rol 'Client' bestaat niet in het systeem."));
            }

            await _userManager.AddToRoleAsync(newUser, role.Name);

            return Ok(ApiResponse<object>.SuccessResponse(null, "Gebruiker werd geregistreerd"));

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginUserRequestDto login)
        {
            if (!ModelState.IsValid) { return BadRequest(ApiResponse<object>.FailureResponse("Ongeldige invoer.", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))); }

            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
            {
                return Unauthorized(ApiResponse<object>.FailureResponse("Gebruiker werd niet gevonden."));
            }

            var result = await _signInManager.PasswordSignInAsync(user, login.Password, isPersistent: false, lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                return Unauthorized(ApiResponse<object>.FailureResponse("Je account is tijdelijk geblokkeerd wegens te veel mislukte inlogpogingen. Probeer het later opnieuw."));
            }

            if (!result.Succeeded)
            {
                return Unauthorized(ApiResponse<object>.FailureResponse("Ongeldige inloggegevens."));
            }

            JwtSecurityToken token = await _accountService.GenerateTokenAsync(user);
            string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);

            var loginResponse = new LoginUserResponseDto
            {
                Token = serializedToken
            };

            return Ok(ApiResponse<LoginUserResponseDto>.SuccessResponse(loginResponse, "Login succesvol"));
        }
    }
}
