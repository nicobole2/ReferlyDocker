namespace Referly.Models.Hunters;


public partial class HuntersDTO
{
    public string Email { get; set; }

    public DateTime RegistrationDate { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string CountryOfResidence { get; set; }
    public string LinkedinProfile { get; set; }
    public string Status { get; set; }
    public string ShortSummary { get; set; }
    public float Rating { get; set; }
    public string BankAccount { get; set; }
    public string Phone { get; set; }
    public HuntersDTO()
    {
        Email ??= "";
        FirstName ??= "";
        LastName ??= "";
        CountryOfResidence ??= "";
        LinkedinProfile ??= "";
        Status ??= "";
        ShortSummary ??= "";
        BankAccount ??= "";
        Phone ??= "";
    }
}
