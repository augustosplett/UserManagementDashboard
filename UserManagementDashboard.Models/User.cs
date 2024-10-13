using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace UserManagementDashboard.Models;

public class User
{
    [Key]
    [DisplayName("Id")]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    [DisplayName("First Name")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    [DisplayName("Last Name")]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    [DisplayName("Email")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
