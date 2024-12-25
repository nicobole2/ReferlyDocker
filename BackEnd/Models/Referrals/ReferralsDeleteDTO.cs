namespace Referly.Models;

public class ReferralDeleteDTO
{
    public string email { get; set; }

    public ReferralDeleteDTO() {
         email ??= "";
    }
}
