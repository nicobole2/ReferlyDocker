using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Referly.Data;
using Referly.Models.Hunters;


namespace Referly.Controllers;

[ApiController]
[Route("[controller]")]
public class HunterController: ControllerBase {

    private readonly DataContextDapper _dapper;
    public HunterController(IConfiguration config) {
        _dapper = new DataContextDapper(config);
    }

[HttpPut("updateHunter")]
  public IActionResult UpdateHunter([FromBody] HuntersUpdateDTO hunter) 
  {
  
      var parameters = new
      {
          Email = hunter.email,
          ApplicationDate = hunter.applicationDate,
          JobSearch = hunter.jobSearch,
          Hunter = hunter.hunter,
          CvLink = hunter.cvLink,
          Description = hunter.description,
          Status = hunter.status,
          LastStatus = hunter.lastStatus,
          CvRating = hunter.cvRating,
          FirstName = hunter.firstName,
          LastName = hunter.lastName
      };

      bool success = _dapper.ExecuteSqlWithParameters("UpdateHunter", parameters);

      if (success)
      {
          return Ok("Registro actualizado correctamente.");
      }
      else
      {
          return NotFound("No se encontró el registro para actualizar.");
      }
  }


[HttpGet("getHunters")]
public IEnumerable<HuntersDTO> GetHunters()
{
    IEnumerable<HuntersDTO> hunters = _dapper.LoadMethod<HuntersDTO>("SELECT * FROM Job.Hunters");
    return hunters;
}

[HttpPost("getHuntersByFilters")]
  public IEnumerable<HuntersDTO> GetHuntersByFilters([FromBody] HuntersUpdateDTO hunter)
{
    var parameters = new
      {
          Email = hunter.email,
          ApplicationDate = hunter.applicationDate,
          JobSearch = hunter.jobSearch,
          Hunter = hunter.hunter,
          CvLink = hunter.cvLink,
          Description = hunter.description,
          Status = hunter.status,
          LastStatus = hunter.lastStatus,
          CvRating = hunter.cvRating,
          FirstName = hunter.firstName,
          LastName = hunter.lastName
      };

    return _dapper.LoadMethodWithParameters<HuntersDTO>("GetHuntersByFilters", parameters);
}


[HttpDelete("deleteHunter")]
public IActionResult DeleteHunter([FromBody] HuntersDTO hunter)
  {
      var parameters = new
      {
          email = hunter.Email,
      };

      bool success = _dapper.ExecuteSqlWithParameters("DeleteHunter", parameters);
      if (success)
      {
          return Ok(new { Message = "Registro eliminado exitosamente." });
      }
      else
      {
          return BadRequest("No se pudo eliminar el registro.");
      }
  }

}

