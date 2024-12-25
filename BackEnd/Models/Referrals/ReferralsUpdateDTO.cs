namespace Referly.Models;

public class ReferralsUpdateDTO
{
    public string Email { get; set; }
    public string JobSearchId { get; set; }
    public string HunterId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public ReferralsUpdateDTO() {
        Email ??= "";
        JobSearchId ??= "";
        HunterId??="";
        FirstName??="";
        LastName??="";
    }
}
