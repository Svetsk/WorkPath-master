using System.ComponentModel.DataAnnotations;

namespace WorkPath.Server.Models;

public class UserAuthModel
{
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
}