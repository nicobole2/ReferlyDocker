namespace Referly.Models.JobOffer;

public class JobOfferFilterDTO
{
    public string Keyword { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    
    public JobOfferFilterDTO()
    {
        Keyword ??= "";
    }
}