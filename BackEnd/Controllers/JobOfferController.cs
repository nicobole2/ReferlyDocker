using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Referly.Data;
using Referly.Models.JobOffer;
using Referly.Models.Paged;

namespace Referly.Controllers;

[ApiController]
[Route("[controller]")]
public class JobOfferController : ControllerBase
{
    private readonly DataContextDapper _dapper;
    private readonly ILogger<JobOfferController> _logger;
    
    public JobOfferController(IConfiguration config, ILogger<JobOfferController> logger)
    {
        _dapper = new DataContextDapper(config);
        _logger = logger;
    }

    [HttpGet("getJobOffers")]
    public IActionResult GetReferrals()
    {
        try
        {
            var referrals = _dapper.LoadMethod<JobOfferDTO>("SELECT * FROM Job.JobSearch");
            return Ok(referrals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting job offers");
            return StatusCode(500, "Error retrieving job offers");
        }
    }

    [HttpPost("searchJobOffers")]
    public IActionResult GetJobOfferPaginated([FromBody] JobOfferFilterDTO request)
    {
        try
        {
            _logger.LogInformation("Searching jobs with filter: {@Filter}", request);

            var parameters = new DynamicParameters();
            parameters.Add("@Keyword", request.Keyword ?? string.Empty);
            parameters.Add("@Page", Math.Max(1, request.Page));
            parameters.Add("@PageSize", Math.Max(1, request.PageSize));

            var (items, total) = _dapper.ExecutePaginatedQuery<JobOfferBase>(
                "SearchJobOpportunitiesPaginated",
                parameters);

            _logger.LogInformation("Found {Count} items out of {Total} total", items.Count(), total);

            var result = new PaginatedResult<JobOfferBase>
            {
                Items = items ?? Enumerable.Empty<JobOfferBase>(),
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching job offers");
            return StatusCode(500, "Error searching job offers");
        }
    }

    [HttpPost("searchDetailedJobOffer")]
    public IActionResult SearchDetailedJobOffer([FromBody] JobOfferDetailFilterDTO request)
    {
        try
        {
            var parameters = new DynamicParameters();
            System.Console.WriteLine(request.JobId);
            parameters.Add("@JobId", request.JobId);

            // Consulta para obtener los detalles principales del Job
            var jobDetail = _dapper.LoadMethodWithParameters<JobOfferDetail>("SearchJobOpportunityById", parameters).FirstOrDefault();

            if (jobDetail == null)
            {
                return NotFound("Job offer not found.");
            }

            // Consulta para obtener las responsabilidades
            var responsibilities = _dapper.LoadMethodWithParameters<string>("GetJobResponsibilitiesById", parameters);

            // Consulta para obtener los requisitos
            var requirements = _dapper.LoadMethodWithParameters<string>("GetJobRequirementsById", parameters);

            // Completar el objeto JobOfferDetail
            jobDetail.Responsibilities = responsibilities.ToList();
            jobDetail.Requirements = requirements.ToList();

            var result = new
            {
                Item = jobDetail
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching job offer detail");
            return StatusCode(500, "Error searching job offer detail");
        }
    }


    [HttpPost("searchBaseJobOffers")]
    public IActionResult GetBaseJobOfferPaginated([FromBody] JobOfferFilterDTO request)
    {
        try
        {
            _logger.LogInformation("Searching jobs with filter: {@Filter}", request);

            var parameters = new DynamicParameters();
            parameters.Add("@Keyword", request.Keyword ?? string.Empty);
            parameters.Add("@Page", Math.Max(1, request.Page));
            parameters.Add("@PageSize", Math.Max(1, request.PageSize));

            var (items, total) = _dapper.ExecutePaginatedQuery<JobOfferBase>(
                "SearchJobOpportunitiesBasePaginated",
                parameters);

            _logger.LogInformation("Found {Count} items out of {Total} total", items.Count(), total);

            var result = new PaginatedResult<JobOfferBase>
            {
                Items = items ?? Enumerable.Empty<JobOfferBase>(),
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching job offers");
            return StatusCode(500, "Error searching job offers");
        }
    }
}