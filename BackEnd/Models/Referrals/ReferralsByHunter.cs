namespace Referly.Models.Referrals;

public class ReferralsByHunter {
    public string CompleteName {get; set;}
    public string Area {get; set;}
    public string JobName {get; set;}
    public string ReferredAt {get; set;}
    public string State {get; set;}
    public ReferralsByHunter() {
        CompleteName??="";
        Area??="";
        JobName??="";
        ReferredAt??="";
        State??="";
    }
}