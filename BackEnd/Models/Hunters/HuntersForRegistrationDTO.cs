namespace Referly.Models.Hunters;


public partial class HuntersForRegistrationDTO
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string LinkedinProfile { get; set; }
    public string ShortSummary { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }

    public HuntersForRegistrationDTO()
    {
        Email ??= "";
        FirstName ??= "";
        LastName ??= "";
        LinkedinProfile ??= "";
        ShortSummary ??= "";
        Phone ??= "";
        Password ??= "";
        PasswordConfirm ??= "";
    }
}
