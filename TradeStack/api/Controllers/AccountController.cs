using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using api.Dtos.Account;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using api.interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManager;
        private readonly ITokenService _tokenService;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signinManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _tokenService = tokenService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            //data validation for annotations
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //first find user (haygeeb kol tafaseelo msh bas username)
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null)
            {
                return Unauthorized("Invalid Username");
            }

            //this function byakhod el user kolo, w yakhod el password w yekarenha bel password el dakhel mel login dto
            //the false bool is because the function has a lockedOutOnFailure attribute and we dont want this to happen
            //so we set it as false
            var checkPassResult = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            //like discussed before, this func returns an object that we can just use the see the status of the login
            //so we stored it in checkPassResult 
            if (!checkPassResult.Succeeded)
            {
                return Unauthorized("Incorrect Password");
            }

            //if all succeeded just return the new user. USE THE USER OBJECT WE RETRIEVED FROM DB, not login dto
            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto regDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = regDto.Username,
                    Email = regDto.Email
                    //we dont add the password directly in text, we give it to the create async func to be hashed
                };

                //finally use the create async function that takes 2 parameters, the user obj and the password alone.
                //recall abl keda kona bne3mel _context.tableName.action like _context.Stocks.saveChanges()
                //hena nafs el haga bas our user manager is our context directly to one entity (user) so we dont specify.
                //(is that explanation accurate?)
                var createdUser = await _userManager.CreateAsync(appUser, regDto.Password);
                //create async returns an object (createUser) that has attributes that we can use to
                //check if its status (success/fail/..)

                if (createdUser.Succeeded)
                {
                    //now the user doesnt explicitly enter his role, but we will make it connected to frontend
                    //so that any user registering through main registration we set it in code as "User"
                    //and again, any _userManager fun takes 2 parameters, the appUser obj always, then the relevant param.
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                    //again roleResult is also just an object created after db access to hold status
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);   //or use BadRequest 3ady
                    }
                }
                else {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}