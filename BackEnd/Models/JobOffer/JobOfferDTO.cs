namespace Referly.Models.JobOffer;

public class JobOfferDTO
{
    public DateTime openDate { get; set; }

    public String positionName { get; set; }

    public int Id  { get; set; }

    public String positionDescription { get; set; }

    public String category { get; set; }

    public String subCategory { get; set; }

    public int referenceId { get; set; }

    public String link { get; set; }

    public String currency { get; set; }

    public int companyContactId { get; set; }

    public Decimal minSalary { get; set; }

    public Decimal maxSalary { get; set; }

    public String state { get; set; }

    public String workMode { get; set; }

    public String city { get; set; }

    public String jobLevel { get; set; }

    public JobOfferDTO()
    {
        state ??= "";
        workMode ??= "";
        city ??= "";
        jobLevel ??= "";
        positionDescription ??= "";
        positionName ??= "";
        category ??= "";
        subCategory ??= "";
        link ??= "";
        currency ??= "";
    }
}