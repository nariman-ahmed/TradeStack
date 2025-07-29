using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using api.Dtos.Account;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto reg)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = reg.Username,
                    Email = reg.Email
                    //we dont add the password directly in text, we give it to the create async func to be hashed
                };

                //finally use the create async function that takes 2 parameters, the user obj and the password alone.
                //recall abl keda kona bne3mel _context.tableName.action like _context.Stocks.saveChanges()
                //hena nafs el haga bas our user manager is our context directly to one entity (user) so we dont specify.
                //(is that explanation accurate?)
                var createdUser = await _userManager.CreateAsync(appUser, reg.Password);
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
                        return Ok("User Registered");
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