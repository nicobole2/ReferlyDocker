using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace Referly.Helper;

public class AuthHelper {

    private readonly IConfiguration _config;

    public AuthHelper(IConfiguration config) {
            _config = config;
    }

    public Dictionary<string,string> getDictionary(string userId) {
       return new Dictionary<string,string> {
            {"token", CreateToken(userId)}
        };
    }

    public byte[] GetPasswordHash(string password, byte[] passwordSalt) {
        string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(passwordSalt);

            byte[] passwordHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
                );

        return passwordHash;
     }

    public string CreateToken(string userId) {
        Claim[] claims = [
            new Claim(
                "userId", userId
            )
        ];
        
        SymmetricSecurityKey  symmetricSecurityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:TokenKey").Value)
        );

        SigningCredentials credencials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

        SecurityTokenDescriptor descriptor = new() {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credencials,
            Expires = DateTime.Now.AddDays(1)
        };

        JwtSecurityTokenHandler jwtHandler = new();

        SecurityToken token = jwtHandler.CreateToken(descriptor);

        return jwtHandler.WriteToken(token);

    }

}