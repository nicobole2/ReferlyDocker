namespace Referly.Models.Hunters;

public partial class HunterForLoginDTO {
    public string Email {get; set;}
    public string Password {get; set;}
    public HunterForLoginDTO() {
        Email ??= "";
        Password ??= "";
    }
}