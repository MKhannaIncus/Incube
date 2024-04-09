using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

public class AppUser
{
    [Key]
    [Column("idusers")]
    public int Id { get; set; } // Custom column name mapping

    [Column("firstname")]
    public string FirstName { get; set; }

    [Column("lastname")]
    public string LastName { get; set; }

    [Column("email")]
    public string Email { get; set; }

    // The Password property seems to be unused and insecure. Consider removing or ignoring it.
    // EF Core configuration can use Fluent API to ignore it if needed.

    [Column("Password")]
    public string Password { get; set; }

    [Column("PasswordHash")]
    public byte[] PasswordHash { get; set; }

    [Column("PasswordSalt")]
    public byte[] PasswordSalt { get; set; }
}