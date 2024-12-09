using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Referly.Data;
using Referly.Models;
using Referly.Models.Hunters;
using Referly.Models.Referrals;


namespace Referly.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ReferralsController: ControllerBase {

    private readonly DataContextDapper _dapper;
    public ReferralsController(IConfiguration config) {
        _dapper = new DataContextDapper(config);
    }

[HttpPut("updateReferral")]
  public IActionResult UpdateReferral([FromBody] ReferralsUpdateDTO referral)
  {
    var parameters = new
    {
        Email = referral.Email,
        JobSearch = referral.JobSearchId,
        Hunter = referral.HunterId,
        FirstName = referral.FirstName,
        LastName = referral.LastName
    };

    bool success = _dapper.ExecuteSqlWithParameters("UpdateReferral", parameters);

    if (success)
    {
        return Ok("Registro actualizado correctamente.");
    }
    else
    {
        return NotFound("No se encontró el registro para actualizar.");
    }
}


[HttpGet("getReferrals")]
public IEnumerable<ReferralsDTO> GetReferrals()
{
  IEnumerable<ReferralsDTO> referrals = _dapper.LoadMethod<ReferralsDTO>("SELECT * FROM Job.Referrals");
  return referrals;
}

[HttpPost("getReferralsByFilters")]
  public IEnumerable<ReferralsDTO> GetReferralsByFilters([FromBody] ReferralsFilterDTO filters)
  {
      var parameters = new
      {
          Id = filters.Id,
          ApplicationDate = filters.ApplicationDate,
          JobSearch = filters.JobSearch,
          Hunter = filters.Hunter,
          CvLink = filters.CvLink,
          Description = filters.Description,
          Status = filters.Status,
          LastStatus = filters.LastStatus,
          CvRating = filters.CvRating,
          FirstName = filters.FirstName,
          LastName = filters.LastName,
      };

      return _dapper.LoadMethodWithParameters<ReferralsDTO>("GetReferralsByFilters", parameters);
}

[HttpPost("getReferralsByHunterId")]
  public IEnumerable<ReferralsByHunter> GetReferralsByHunterId()
  {
    
      string userIdString = User.FindFirst("userId")?.Value ?? "";

      var parameters = new
      {
          HunterId = userIdString,
      };

      return _dapper.LoadMethodWithParameters<ReferralsByHunter>("GetAllReferrals", parameters);
}
      

[HttpPost("createReferral")]
public async Task<IActionResult> CreateFileReferral([FromForm] ReferralsCreateDTO referral, [FromForm] IFormFile? pdfFile) 
{
        if (pdfFile == null || pdfFile.Length == 0)
        {
            return NotFound(new { Message = "No se encontró el pdf" });
        }
        if (pdfFile.ContentType != "application/pdf")
        {
            System.Console.WriteLine("pdFile: application/pdf");
            return BadRequest(new { Message = "El archivo debe ser un PDF." });
        }
        var refferResult = "";
        try {

        using var memoryStream = new MemoryStream();
        await pdfFile.CopyToAsync(memoryStream);
        int hunterId = 0;
        string userIdString = User.FindFirst("userId")?.Value ?? "";

        if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int parsedHunterId))
        {
            hunterId = parsedHunterId;
        }
        else
        {
            Console.WriteLine("Invalid or missing userId");
        }
        var parameters = new
            {
                Email = referral.Email,
                JobSearch = referral.JobSearch,
                Hunter = hunterId,
                FirstName =  referral.FirstName,   
                LastName = referral.LastName,
                PdfFile = memoryStream.ToArray(),
                PdfFileName = pdfFile.FileName,
                PdfContentType = pdfFile.ContentType
            };
             System.Console.WriteLine($"parameters {parameters}");

           refferResult = _dapper.ExecuteStoredProcedureWithStringResult("ManageReferral", parameters); 
        }
        catch (Exception ex)
        {
           System.Console.WriteLine($"Error . - Message {ex.Message}");
           refferResult= $"Error . - Message {ex.Message}";
        }

        if(refferResult == "REFERRAL_CREATED_SUCCESSFULLY") {
            return Ok( new { Message = "REFERRAL_CREATED_SUCCESSFULLY"});
        }

        if(refferResult == "REFERRAL_UPDATED_SUCCESSFULLY") {
            return Ok( new { Message = "REFERRAL_UPDATED_SUCCESSFULLY"});
        }

        if(refferResult == "JOB_SEARCH_DONT_EXIST") {
            return StatusCode(404, new { Message = "JOB_SEARCH_DONT_EXIST"});
        }
        
        if(refferResult == "ALREADY_REFFERED_FOR_THIS_JOB") {
            return StatusCode(409,new{Message = "ALREADY_REFFERED_FOR_THIS_JOB"});
        }

        if(refferResult == "HUNTER_NOT_EXIST") {
            return StatusCode(404,new{Message = "HUNTER_NOT_EXIST"});
        }

        return StatusCode(500,new{Message = "UKNOWN ERROR"});
}




[HttpGet("getFileReferral")]
public IActionResult GetFileReferral(string referralEmail)
{
    try
    {
        var files = _dapper.LoadMethodWithParameters<ReferralsFilesDTO>(
            "GetReferralFile",
            new { ReferralEmail = referralEmail }
        );

        var fileData = files.FirstOrDefault();
        if (fileData == null)
        {
            return NotFound(new { Message = "No se encontró un archivo para el email proporcionado." });
        }

        byte[] pdfFile = fileData.pdfFile;
        string pdfFileName = fileData.pdfFileName;
        string pdfContentType = fileData.pdfContentType;

        return File(pdfFile, pdfContentType, pdfFileName);
    }
    catch (Exception ex)
    {
        System.Console.WriteLine($"Error al recuperar el archivo: {ex.Message}\nStackTrace: {ex.StackTrace}");
        return StatusCode(500, new { Message = "Error al recuperar el archivo.", Error = ex.Message });
    }
}




[HttpDelete("deleteReferral")]
public IActionResult DeleteReferral([FromBody] ReferralDeleteDTO referral)
  {
      var parameters = new
      {
          email = referral.email,
      };

      bool success = _dapper.ExecuteSqlWithParameters("DeleteReferral", parameters);
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

