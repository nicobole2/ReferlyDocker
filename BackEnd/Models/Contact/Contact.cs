namespace Referly.Models.Contact;

public class Contact {
    public string Name {get; set;}
    public string Email {get; set;}
    public string Subject {get; set;}
    public string Message {get; set;}

    public Contact() {
        Name??="";
        Email??="";
        Subject??="";
        Message??="";
    }
}