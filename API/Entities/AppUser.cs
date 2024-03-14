using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class AppUser
{
    //List of the different necessary properties
    //each property refers to a column in the database
    //returhned as Json 
    public int Id {get; set;} //Part of built in conventions, Primary Key
    public string FirstName { get; set;}
    public string LastName {get; set;}

    public string Email {get; set;}
    public string Password { get; set;}

    //The password is only stored as hash and salt in the database- for sercurity purposes
    //public string Password {get; set;}
    public byte[] PasswordHash{get; set;}
    
    public byte[] PasswordSalt{get; set;}
}
