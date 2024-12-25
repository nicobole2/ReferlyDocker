namespace Referly.Models.JobOffer;

public class JobOfferDetail
{
    public DateTime Published { get; set; }
    public String Title { get; set; }
    public String Description { get; set; }
    public String Category { get; set; }
    public int ReferenceId { get; set; }
    public Decimal Share { get; set; }
    public String Level { get; set; }  
    public String WorkMode { get; set; }
    public String Location { get; set; }
    public int Vacancies  { get; set; }

    public List<string> Responsibilities { get; set; } = [];
    public List<string> Requirements { get; set; } = [];


    public JobOfferDetail()
    {
    Title ??="";
    Description ??="";
    Category ??="";
    Level??="";
    Location??="";
    WorkMode??="";
    }
}