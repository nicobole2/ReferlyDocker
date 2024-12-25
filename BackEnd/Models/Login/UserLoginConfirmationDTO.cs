namespace Referly.Models {

    public partial class UserForLoginConfirmationDto {
        public byte[] PasswordKey {get; set;}
        public  byte[] PasswordSalt {get; set;}

         public UserForLoginConfirmationDto() {
            PasswordKey = [];
            PasswordSalt = [];
        }
    }
}