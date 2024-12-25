namespace Referly.Models.JobOffer;

public class JobOfferBase
{
    public DateTime Published { get; set; }

    public String PositionName { get; set; }

    public int Id  { get; set; }

    public String Description { get; set; }

    public String Category { get; set; }

    public int ReferenceId { get; set; }

    public Decimal Share { get; set; }

    public String State { get; set; }

    public String WorkMode { get; set; }

    public String Location { get; set; }

    public String JobSenority { get; set; }

    public JobOfferBase()
    {
        PositionName??="";
        Description??="";
        Category??="";
        State??="";
        WorkMode??="";
        Location??="";
        JobSenority??="";
    }
}