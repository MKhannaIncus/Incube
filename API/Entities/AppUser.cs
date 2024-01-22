namespace API.Entities;

public class AppUser
{
    //List of the different necessary properties
    //each property refers to a column in the database
    public int Id {get; set;} //Part of built in conventions, Primary Key
    public string FirstName { get; set;}
    public string LastName {get; set;}
    public string Email {get; set;}
    public string Password { get; set;}

}
