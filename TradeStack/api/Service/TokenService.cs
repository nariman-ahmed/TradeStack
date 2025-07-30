using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;

namespace api.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
            //dont foget the key is used as binary. also we only injected and used _config to extract that key
        }

        //then we are going to put claims in our token
        //the claim obj is already provided by JWT Identity
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                //now we are basically enterring the key value pairs
                //also we need to use these words (JWTRegisteresClaimNames) for the key with each claim as its JWT standard
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)  //"GivenName":"username string"
            };

            //next we need to create the sign in credentials, AKA what type of encryption do you want
            //we pass the key (that we extracted from appsetting in the constructor), amd the type of encryption we want for the key
            //here we used 512 bytes encryption aka the key must be 64 characters long!
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            //finally we actually set up an object representation of the token and return it
            //as we saif, tokens are made of multiple blocks of info, separated by periods
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), //pass the claims
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            //This creates an instance of the built-in JWT handler
            //It's responsible for creating and writing the actual token object.
            var tokenHandler = new JwtSecurityTokenHandler();

            //hena CreateToken di function taba3 object tokenHandler mel JWTSecurityTokenHandler class, msh heya heya 
            //el ehna ben build now.
            //This creates the token object based on the tokenDescriptor
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //we want to return the token not as an actualy object but as a JWT string
            return tokenHandler.WriteToken(token);
        }
    }
}