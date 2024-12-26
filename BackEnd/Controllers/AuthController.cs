using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Referly.Data;
using Referly.Helper;
using Referly.Models;
using Referly.Models.Hunters;



namespace Referly.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthController: ControllerBase {


    private readonly DataContextDapper _dapper;

    private readonly IConfiguration _config;

    private readonly AuthHelper authHelper;
    public AuthController(IConfiguration config) {
        _dapper = new DataContextDapper(config);
        _config = config;
        authHelper = new(config);
    }


    [AllowAnonymous]
    [HttpPost("Register")]
    public IActionResult Register(HuntersForRegistrationDTO user)
    {
    System.Console.WriteLine($"user password {user.Password}");
    if(user.Password == user.PasswordConfirm) {
            string sqlCheckUserExist = "SELECT Email FROM Job.Auth WHERE Email = '"+user.Email+"'";
            IEnumerable<string> result =  _dapper.LoadMethod<string>(sqlCheckUserExist);
            if(!result.Any()) {
                byte[] passwordSalt = new byte[128/8];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) {
                    rng.GetNonZeroBytes(passwordSalt);
                }
                string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(passwordSalt);

                byte[] passwordHash = KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: System.Text.Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
                );
            
                var parameters = new
                {
                    Email = user.Email,
                    PasswordKey = passwordHash,
                    PasswordSalt = passwordSalt
                };
                var id = _dapper.ExecuteSqlWithReturnValue("ManageAuth", parameters);
                if(id > 0) {
                    var parametersForRegistration = new {
                        Email = user.Email,
                        FirstName =user.FirstName,
                        LastName= user.LastName,
                        LinkedinProfile=user.LinkedinProfile,
                        ShortSummary=user.ShortSummary,
                        Phone = user.Phone
                        };


                        var userId = _dapper.ExecuteSqlWithReturnValue("ManageHunter", parametersForRegistration);
                        if(userId > 0) {
                                Console.WriteLine("userID" + userId);
                                return Ok(authHelper.getDictionary(userId.ToString()));
                        } else {
                            return StatusCode(500,"Unable to create a user" );               
                        }
                    
                } else {
                return StatusCode(500,"Unable to create a user" );     
                }
            } 
            return StatusCode(401,"User with that email already exist" );
        }
             return BadRequest("Passwords are differents");
        }
        
        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken()
        {
            string userId = User.FindFirst("userId")?.Value ?? "";

            string userSql = "SELECT [id] FROM Job.Hunters WHERE id = " + userId;

            int userIdFromDb = _dapper.LoadSingleMethod<int>(userSql);

            return Ok(authHelper.getDictionary(userId.ToString()) );
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(HunterForLoginDTO user)
        {
           
            try{ 
                 string sqlForHashAndSalt = @"
                    SELECT [Email],
                    [PasswordKey],
                    [PasswordSalt] FROM Job.Auth  WHERE Email = '"+user.Email+"'";

                var userId = 0;
                UserForLoginConfirmationDto userForConfirmation = _dapper.LoadSingleMethod<UserForLoginConfirmationDto>(sqlForHashAndSalt);
                userId = _dapper.LoadSingleMethod<int>("SELECT id FROM Job.Hunters WHERE email = '"+user.Email+"'");
                System.Console.WriteLine($"userId {userId}");       
                byte[] passwordHash = authHelper.GetPasswordHash(user.Password, userForConfirmation.PasswordSalt);
                for(int index = 0; index < passwordHash.Length; index++) {
                    if(passwordHash[index] != userForConfirmation.PasswordKey[index]) {
                        return StatusCode(401, "Incorrect password");
                    }
                }
            return Ok(authHelper.getDictionary(userId.ToString()));
            } catch {
                return NotFound();
            }
        
        }
    
}

