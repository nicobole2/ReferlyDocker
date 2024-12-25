namespace Referly.Models;

public class ReferralsCreateDTO
{
    public string JobSearch { get; set; }
    //public int Hunter { get; set; }
    //public string CvLink { get; set; }
    //public string Description { get; set; }
    //public string Status { get; set; }
    //public string LastStatus { get; set; }
    //public float CvRating { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    //public Byte[] CvPdfFile {get; set;}

    public ReferralsCreateDTO() {
        JobSearch??="";
        //CvLink??="";
        //Description??="";
        //Status??="";
        //LastStatus??="";
        FirstName??="";
        LastName??="";
        Email??="";
        //CvPdfFile??= [];
    }

}
