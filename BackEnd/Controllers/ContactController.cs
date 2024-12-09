using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Referly.Data;
using Referly.Models.Contact;
using Referly.Models.Hunters;


namespace Referly.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactController: ControllerBase {

    private readonly DataContextDapper _dapper;
    public ContactController(IConfiguration config) {
        _dapper = new DataContextDapper(config);
    }

[HttpPut("setContact")]
  public IActionResult SetContact([FromBody] Contact contact) 
  {
  
      var parameters = new
      {
          Email = contact.Email,
          Subject = contact.Subject,
          Name = contact.Name,
          Message = contact.Message
      };

      bool success = _dapper.ExecuteSqlWithParameters("SetContact", parameters);
      if (success)
      {
          return Ok("Registro actualizado correctamente.");
      }
      else
      {
          return NotFound("No se encontró el registro para actualizar.");
      }
  }
}

