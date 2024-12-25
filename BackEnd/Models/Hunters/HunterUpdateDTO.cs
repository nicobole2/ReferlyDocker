namespace Referly.Models.Hunters;

public partial class HuntersUpdateDTO
{
    public string email { get; set; }
    public DateTime? applicationDate { get; set; }
    public string? jobSearch { get; set; }
    public string? hunter { get; set; }
    public string? cvLink { get; set; }
    public string? description { get; set; }
    public string? status { get; set; }
    public string? lastStatus { get; set; }
    public float? cvRating { get; set; }
    public string? firstName { get; set; }
    public string? lastName { get; set; }


    public HuntersUpdateDTO()
    {
        email ??="";
    }
}
