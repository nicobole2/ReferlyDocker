namespace Referly.Models.Referrals;

public class ReferralsFilesDTO {

    public String referralEmail {get; set;}
    public Byte[] pdfFile {get; set;}
    public String pdfFileName {get; set;}
    public String pdfContentType {get; set;}
    public DateTime createdAt {get; set;}

    public ReferralsFilesDTO() {
        referralEmail??="";
        pdfFileName??="";
        pdfContentType??="";
        pdfFile??= [];
    }
}